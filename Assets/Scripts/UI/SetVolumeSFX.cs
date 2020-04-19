using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolumeSFX : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    
    void Start()
    {
        mixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol", 1f));
        slider.value = PlayerPrefs.GetFloat("SFXVol");
    }

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat ("SFXVol", Mathf.Log10(sliderValue) * 20 );
        PlayerPrefs.SetFloat("SFXVol", sliderValue);
    }
}
