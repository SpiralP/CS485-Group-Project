﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource SFXSource;
    public AudioClip affirm;
    public AudioClip back;
    public AudioClip reset;
    public AudioMixer MusicMixer;
    public AudioMixer SFXMixer;
    public Player player;



    void Start()
    {
        //MusicMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        //SFXMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        player.LoadPlayer();
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
        SceneManager.LoadScene("Loading");
    }

    public void BackSound ()
    {
        SFXSource.clip = back;
        SFXSource.Play();
    }

    public void Reset()
    {
        player.ResetPlayer();
        player.SavePlayer();
        SFXSource.clip = reset;
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
