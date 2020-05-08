using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sfx;
    public AudioClip gameOverSound;
    public AudioClip affirm;
    public AudioClip negative;
    public AudioClip ambience;
    public Player player;
    public GameObject scores;
    public GameObject options;
    public TextMeshProUGUI score;
    public TextMeshProUGUI bestscore;

    void Start()
    {
        player.LoadPlayer();
        SetScoreVals();
        music.clip = gameOverSound;
        music.Play();
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3);
        scores.SetActive(true);
        yield return new WaitForSeconds(3);
        options.SetActive(true);
        yield return new WaitForSeconds(5);
        music.clip = ambience;
        music.loop = true;
        music.Play();
    }

    public void AffirmSound()
    {
        sfx.clip = affirm;
        sfx.Play();
    }

    public void NegativeSound()
    {
        sfx.clip = negative;
        sfx.Play();
    }
    
    public void YesPressed()
    {
        player.ResetPlayer();
        player.SavePlayer();
        SceneManager.LoadScene("Loading");
    }

    public void NoPressed()
    {
        player.ResetPlayer();
        player.SavePlayer();
        Initiate.Fade("Main Menu", Color.black, 1f);
    }

    private void SetScoreVals()
    {
        score.text = Math.Round(player.totalscore).ToString();
        bestscore.text = Math.Round(player.lifetimebestscore).ToString();
    }
}
