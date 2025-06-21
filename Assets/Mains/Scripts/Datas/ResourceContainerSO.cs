using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    [CreateAssetMenu(fileName = "ResourceContainerSO", menuName = "YNL - Checkotel/ResourceContainerSO")]
    public class ResourceContainerSO : ScriptableObject
    {
        public SerializableDictionary<string, Texture2D> Icons = new();
        public SerializableDictionary<string, StyleSheet> Styles = new();
        public SerializableDictionary<string, FontAsset> Fonts = new();
    }
}