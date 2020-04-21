using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolumeSFX : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    void Start()
    {
        mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVolumeLevel", 1f));
        slider.value = PlayerPrefs.GetFloat("SFXSliderValue");
    }

    public void SetLevel(float sliderValue)
    {
        float value = Mathf.Log10(sliderValue) * 20;
        AudioManager.instance.AdjustSFXVolume(value);
        PlayerPrefs.SetFloat("SFXSliderValue", sliderValue);
        PlayerPrefs.Save();
    }
}