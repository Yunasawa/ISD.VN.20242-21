#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;

public class TextureFade : MonoBehaviour
{
    public Texture2D originalTexture;
    public Color fadeColor = Color.red;
    [Range(0f, 1f)] public float size = 0.5f;

    [Button]
    public void Start()
    {
        if (originalTexture != null)
        {
            Texture2D fadedTexture = ApplyFade(originalTexture, fadeColor, size);
            SaveTexture(fadedTexture, originalTexture);
        }
    }

    Texture2D ApplyFade(Texture2D texture, Color color, float size)
    {
        int width = texture.width;
        int height = texture.height;
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        for (int y = 0; y < height; y++)
        {
            float alphaFade = Mathf.Clamp01(1f - (y / (height * size))); // Fade control
            for (int x = 0; x < width; x++)
            {
                Color pixel = texture.GetPixel(x, y);
                Color fadeOverlay = new Color(color.r, color.g, color.b, alphaFade); // Fade overlay

                // If pixel is fully transparent, apply fade color instead
                if (pixel.a == 0)
                    pixel = fadeOverlay;
                else
                    pixel = Color.Lerp(pixel, fadeOverlay, alphaFade);

                newTexture.SetPixel(x, y, pixel);
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    void SaveTexture(Texture2D texture, Texture2D original)
    {
        byte[] bytes = texture.EncodeToPNG();
        string path = AssetDatabase.GetAssetPath(original);
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Replace(".png", "_Faded.png");
            File.WriteAllBytes(path, bytes);
            Debug.Log("Saved faded texture to: " + path);
        }
    }
}
#endif