using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource SFXSource;
    public AudioClip affirm;
    public AudioClip back;
    public AudioMixer MusicMixer;
    public AudioMixer SFXMixer;

    void Start()
    {
        MusicMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVolume"));
        SFXMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));

        // load player data here, maybe move the audio volumes in there too
        // LoadPlayerData() ?
    }

    public void ForwardSound()
    {
        SFXSource.clip = affirm;
        SFXSource.Play();
    }

    public void PlayGame ()
    {
        SFXSource.clip = affirm;
        SFXSource.Play();
       
        SceneManager.LoadScene("Level1");   // will change to current level player is on
    }

    public void BackSound ()
    {
        SFXSource.clip = back;
        SFXSource.Play();
    }
}
