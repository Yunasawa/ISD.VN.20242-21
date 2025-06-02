using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;
using System.Collections;
using System.Text.RegularExpressions;
using YNL.Utilities.Addons;
using System.Collections.Generic;

namespace YNL.JAMOS
{
    public static partial class Function
    {
        private static SerializableDictionary<UID, uint> _orderedAmounts => Main.Runtime.OrderedAmounts;

        public static async UniTask<string> GetRawDatabaseAsync(this string url)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                return null;
            }
        }

        public static bool FuzzyContains(this string source, string target, int maxDistance = 1)
        {
            if (string.IsNullOrEmpty(target)) return true;

            source = source.ToLower();
            target = target.ToLower();

            int targetLength = target.Length;

            for (int i = 0; i <= source.Length - targetLength; i++)
            {
                string substring = source.Substring(i, targetLength);

                if (LevenshteinDistance(substring, target) <= maxDistance)
                {
                    return true;
                }
            }

            return false;
        }

        public static string AddSpace(this string input)
        {
            return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }

        public static void SetTimeText(this Label element, float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            element.SetText($"{minutes:00}:{seconds:00}");
        }

        private static int LevenshteinDistance(string s, string t)
        {
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;

            int[,] distance = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) distance[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) distance[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[s.Length, t.Length];
        }

        public static void ApplyCloudImageAsync(this VisualElement element, string url)
        {
            Texture2D nullTexture = null;
            element.SetBackgroundImage(nullTexture);
            ApplyCloudImage(element, url).Forget();

            async UniTaskVoid ApplyCloudImage(VisualElement element, string url)
            {
                int maxRetries = 10;
                int attempt = 0;
                float retryDelay = 1f; // Delay between retries (seconds)

                while (attempt < maxRetries)
                {
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
                    {
                        var operation = uwr.SendWebRequest();
                        await UniTask.WaitUntil(() => operation.isDone);

                        if (uwr.result == UnityWebRequest.Result.Success)
                        {
                            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                            element.SetBackgroundImage(texture);
                            return;
                        }
                        else
                        {
                            Debug.LogWarning($"Attempt {attempt + 1} failed: {uwr.error}");
                            attempt++;
                            await UniTask.Delay(TimeSpan.FromSeconds(retryDelay)); // Wait before retrying
                        }
                    }
                }

                Debug.LogError($"Failed to load texture after {maxRetries} attempts.");
            }
        }
    
        public static void RebuildListView(this ListView list, IList source)
        {
            list.itemsSource = null;
            list.itemsSource = source;
            list.RefreshItems();
        }

        public static void LoadCloudAudioAsync(this UID id, Action<AudioClip> onCompleted = null)
        {
            DownloadAudio(id.GetStreamURL(), onCompleted).Forget();

            async UniTask DownloadAudio(string url, Action<AudioClip> onComplete)
            {
                using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
                {
                    await request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                        onComplete?.Invoke(clip); // Fire the action with the downloaded clip
                    }
                    else
                    {
                        Debug.LogError("Failed to download audio: " + request.error);
                        onComplete?.Invoke(null); // Invoke with null to indicate failure
                    }
                }
            }
        }

        public static void AdjustOrderedAmount(this UID id, bool isAdded)
        {
            var product = Main.Database.Products[id];

            if (isAdded)
            {
                var valueOrDefault = _orderedAmounts.GetValueOrDefault(id);

                if (valueOrDefault < product.Quantity)
                {
                    _orderedAmounts[id] = valueOrDefault + 1;
                }
            }
            else if (_orderedAmounts.TryGetValue(id, out var currentAmount) && currentAmount >= 1)
            {
                _orderedAmounts[id]--;
            }
        }

        public static void SetCartButtonStatus(this UID id, Button addToCartButton)
        {
            var product = Main.Database.Products[id];

            var isCarted = Main.Runtime.Data.CartedProducts.Contains(id);
            var isCollected = Main.Runtime.Data.ProductCollection.Contains(id);

            if (isCollected)
            {
                SetAsNegativeButton(addToCartButton, "Available in collection");
            }
            else if (product.IsFree)
            {
                SetAsPositiveButton(addToCartButton, "Add to collection");
            }
            else if (isCarted)
            {
                SetAsNegativeButton(addToCartButton, "Remove from cart");
            }
            else
            {
                SetAsPositiveButton(addToCartButton, "Add to cart");
            }

            void SetAsPositiveButton(Button button, string label)
            {
                button.SetText(label);
                button.SetBackgroundColor("#DEF95D");
                button.SetColor("#202020");
            }

            void SetAsNegativeButton(Button button, string label)
            {
                button.SetText(label);
                button.SetBackgroundColor(Color.clear);
                button.SetColor("#DEF95D");
            }
        }
    }
}