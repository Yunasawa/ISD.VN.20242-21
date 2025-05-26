using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public static partial class Function
    {
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

        public static void ApplyCloudImageAsync(VisualElement element, string url)
        {
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
    }
}