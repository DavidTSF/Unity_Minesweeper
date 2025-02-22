using UnityEngine;

public class ToggleMute : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ToggleAudio()
    {
        audioSource.mute = !audioSource.mute;
    }
}