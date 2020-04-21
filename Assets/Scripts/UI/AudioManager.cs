using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public static AudioManager instance;

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            DontDestroyOnLoad(gameObject);
            AudioManager.instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolumeLevel", 1f);
        AdjustMusicVolume(music);

        float sfx = PlayerPrefs.GetFloat("SFXVolumeLevel", 1f);
        AdjustSFXVolume(sfx);
    }

    public void AdjustMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVol", volume);
        PlayerPrefs.SetFloat("MusicVolumeLevel", volume);
        PlayerPrefs.Save();
    }

    public void AdjustSFXVolume(float volume)
    {
        sfxMixer.SetFloat("SFXVol", volume);
        PlayerPrefs.SetFloat("SFXVolumeLevel", volume);
        PlayerPrefs.Save();
    }


}
