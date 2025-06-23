using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;

namespace YNL.JAMOS
{
    public class Test : MonoBehaviour
    {
        public TextAsset _address;
        public DatabaseContainerSO _database;

        [Button]
        public void Start()
        {
            string path = AssetDatabase.GetAssetPath(_address);
            string[] lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                Debug.LogError("Address file is empty!");
                return;
            }

            int index = 0;
            foreach (var account in _database.Accounts)
            {
                string line = lines[index % lines.Length]; // Loop through addresses
                index++;

                string[] parts = line.Split('|');

                if (parts.Length == 2)
                {
                    account.Value.Address = new AccountAddress
                    {
                        Address = parts[0].Trim(),
                        City = parts[1].Trim()
                    };
                }
                else
                {
                    Debug.LogWarning($"Invalid address format at index {index}: {line}");
                }
            }

            Debug.Log($"Assigned {index} addresses to {_database.Accounts.Count} accounts (looped as needed).");
        }
    }
}