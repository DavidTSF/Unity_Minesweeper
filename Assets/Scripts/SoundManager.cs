using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject SliderVolume;
    private Slider slider;
    public AudioSource explosionSound;
    public AudioSource backgroundMusic;
    void Start()
    {
        slider = SliderVolume.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        explosionSound.volume = slider.value;
        backgroundMusic.volume = slider.value;
    }

    public void PlayExplosionSound()
    {
        explosionSound.Play();
    }

}
