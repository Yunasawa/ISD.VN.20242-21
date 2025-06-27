using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
namespace YNL.JAMOS
{
    public class DataManager : MonoBehaviour
    {
        public static string ProductImageURL = string.Empty;
        public static string ProductStreamURL = string.Empty;

        public bool EnableLoadingImageDatabase = false;

        private DatabaseContainerSO _database => Main.Database;

        [SerializeField] private SerializableDictionary<Product.Type, string> _productDatabaseURLs = new();
        [SerializeField] private string _feedbackDatabaseURL;
        [SerializeField] private string _configDatabaseURL;

        private string _savingPath => $"{Application.persistentDataPath}/Save.ysf";

        private void Awake()
        {
            Marker.OnRuntimeSavingRequested += SaveRuntimeData;
        }

        private void OnDestroy()
        {
            Marker.OnRuntimeSavingRequested -= SaveRuntimeData;
        }

        private void Start()
        {
            Main.Runtime.Reset();

            LoadSavedData();

            InitializeDatabases().Forget();
        }

        public void SaveRuntimeData()
        {
            SaveNewtonJson(Main.Runtime.Data, _savingPath);
        }

        private async UniTaskVoid InitializeDatabases()
        {
            await InitializeConfigDatabase();
            await InitializeProductDatabase();
            if (EnableLoadingImageDatabase) await InitializeImageDatabase();
            await InitializeFeedbackDatabase();

            Marker.OnDatabaseSerializationDone?.Invoke();
            Main.IsSystemStarted = true;
        }

        public void LoadSavedData()
        {
            Main.Runtime.Data = MJson.LoadNewtonJson<RuntimeData>(_savingPath, null, ResaveData);

            if (Main.Runtime.Data == null) ResaveData(null);

            void ResaveData(string message)
            {
                Main.Runtime.Data = new();
                SaveNewtonJson(Main.Runtime.Data, _savingPath);
            }
        }

        private async UniTask InitializeImageDatabase()
        {
            var downloadTasks = new List<UniTask>();

            foreach (var kvp in Main.Database.Products)
            {
                var uid = kvp.Key;
                var url = uid.GetImageURL();

                if (string.IsNullOrEmpty(url)) continue;

                downloadTasks.Add(DownloadAndCacheImage(uid, url));
            }

            await UniTask.WhenAll(downloadTasks);

            async UniTask DownloadAndCacheImage(UID uid, string url)
            {
                int maxRetries = 2;
                int attempt = 0;
                float retryDelay = 1f;

                while (attempt < maxRetries)
                {
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
                    {
                        var operation = uwr.SendWebRequest();
                        await UniTask.WaitUntil(() => operation.isDone);

                        if (uwr.result == UnityWebRequest.Result.Success)
                        {
                            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

                            if (!Main.Database.Images.ContainsKey(uid))
                                Main.Database.Images.Add(uid, texture);
                            else
                                Main.Database.Images[uid] = texture;

                            return;
                        }
                        else
                        {
                            Debug.LogWarning($"Image download failed for UID {uid}, attempt {attempt + 1}: {uwr.error}");
                            attempt++;
                            await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
                        }
                    }
                }

                Debug.LogError($"Failed to download image for UID {uid} after {maxRetries} attempts.");
            }
        }

        private async UniTask InitializeConfigDatabase()
        {
            var content = await _configDatabaseURL.GetRawDatabaseAsync();

            string[] lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            ProductImageURL = lines[0];
            ProductStreamURL = lines[1];
        }

        private async UniTask InitializeFeedbackDatabase()
        {
            var content = await _feedbackDatabaseURL.GetRawDatabaseAsync();

            _database.Feedbacks.Clear();

            string[] lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int l = 1; l < lines.Length; l++)
            {
                var fields = lines[l].SplitCSV();

                if (string.IsNullOrEmpty(fields[1])) break;

                _database.SerializeFeedbackDatabase(fields);
            }
        }

        private async UniTask InitializeProductDatabase()
        {
            _database.Products.Clear();
            var products = new List<(UID ID, Product.Data Product)>();

            foreach (Product.Type type in Enum.GetValues(typeof(Product.Type)))
            {
                if (type == Product.Type.None) continue;

                var content = await _productDatabaseURLs[type].GetRawDatabaseAsync();
                var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                for (int l = 1; l < lines.Length; l++)
                {
                    var fields = lines[l].SplitCSV();

                    if (string.IsNullOrEmpty(fields[1])) break;

                    products.SerializeProductDatabase(type, fields);
                }
            }

            products.Shuffle();

            foreach (var product in products)
            {
                _database.Products.Add(product.ID, product.Product);
            }
        }

        public static bool SaveNewtonJson<T>(T data, string path, Action saveDone = null)
        {
            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path)) { } // Properly closes the file
                Debug.LogWarning("Target json file doesn't exist! Created a new file.");
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Open file with proper read/write permissions
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif

            return true;
        }
    }
}