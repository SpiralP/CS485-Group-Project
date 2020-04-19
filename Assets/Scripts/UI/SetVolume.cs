using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    void Start()
    {
        mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol", 1f));
        slider.value = PlayerPrefs.GetFloat("MusicVol");
    }

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat ("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
    }
}
