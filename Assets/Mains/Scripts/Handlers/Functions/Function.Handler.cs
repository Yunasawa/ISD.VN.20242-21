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
using System.Reflection;

namespace YNL.JAMOS
{
    public static partial class Function
    {
        private static SerializableDictionary<UID, uint> _orderedAmounts => Main.Runtime.OrderedAmounts;
        private static SerializableDictionary<UID, List<UID>> _productCollection => Main.Runtime.Data.ProductCollection;

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

        public static void ApplyCloudImageAsync(this VisualElement element, UID id)
        {
            element.SetBackgroundImage(Main.Database.Images[id]);
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
                if (currentAmount == 1)
                {
                    _orderedAmounts.Remove(id);
                }
                else
                {
                    _orderedAmounts[id]--;
                }
            }
        }

        public static void SetCartButtonStatus(this UID id, Button addToCartButton)
        {
            var existCartedList = Main.Runtime.Data.CartedProducts.TryGetValue(Main.Runtime.Data.AccountID, out var cartedList);
            var existCollection = Main.Runtime.Data.ProductCollection.TryGetValue(Main.Runtime.Data.AccountID, out var productCollection);

            var product = Main.Database.Products[id];

            var isCarted = existCartedList ? cartedList.Contains(id) : false;
            var isCollected = existCollection ? productCollection.Contains(id) : false;

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

        public static void Insert<TKey, TValue>(this SerializableDictionary<TKey, TValue> dict, int index, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                throw new ArgumentException($"Key '{key}' already exists.");

            if (index < 0 || index > dict.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of bounds.");

            var temp = new List<KeyValuePair<TKey, TValue>>(dict);
            dict.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                if (i == index)
                {
                    dict.Add(key, value);
                }

                dict.Add(temp[i].Key, temp[i].Value);
            }

            // If index == temp.Count, we never hit the insert point inside the loop
            if (index == temp.Count)
            {
                dict.Add(key, value);
            }


        }
    }
}