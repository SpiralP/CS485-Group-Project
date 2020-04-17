using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat ("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        PlayerPrefs.Save();
    }
}
