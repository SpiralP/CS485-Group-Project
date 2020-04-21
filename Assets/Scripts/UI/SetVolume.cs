using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    void Start()
    {
        mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolumeLevel", 1f));
        slider.value = PlayerPrefs.GetFloat("MusicSliderValue");
    }

    public void SetLevel (float sliderValue)
    {
        float value = Mathf.Log10(sliderValue) * 20;
        AudioManager.instance.AdjustMusicVolume(value);
        PlayerPrefs.SetFloat("MusicSliderValue", sliderValue);
        PlayerPrefs.Save();
    }
}
