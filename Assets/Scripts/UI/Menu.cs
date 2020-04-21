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


    private void Awake()
    {
        MusicMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        SFXMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
    }

    void Start()
    {
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

        Initiate.Fade("Level1", Color.black, 1.5f);   // change to current level player is on
    }

    public void BackSound ()
    {
        SFXSource.clip = back;
        SFXSource.Play();
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();      
        #endif
    }
}
