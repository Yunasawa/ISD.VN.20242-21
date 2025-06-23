using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using Sirenix.OdinInspector;

public class AudioStreamer : MonoBehaviour
{
    public string audioUrl = "https://example.com/audio.mp3"; // Replace with your actual URL
    private AudioSource audioSource;

    [Button]
    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        DownloadAndPlayAudio(audioUrl).Forget();
    }

    private async UniTaskVoid DownloadAndPlayAudio(string url)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            var operation = await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Failed to download audio: " + request.error);
            }
        }
    }
}