using Sirenix.OdinInspector;
using UnityEngine;

namespace YNL.JAMOS
{
    public class Test : MonoBehaviour
    {
        public string URL;

        [Button]
        public async void Run()
        {
            var content = await URL.GetRawDatabaseAsync();

            if (!string.IsNullOrEmpty(content))
            {
                Debug.Log("File Contents: " + content);
            }
            else
            {
                Debug.LogError("Failed to load file!");
            }
        }
    }
}