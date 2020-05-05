using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sfx;
    public AudioClip gameOverSound;
    public AudioClip affirm;
    public AudioClip negative;
    public AudioClip ambience;
    public Player player;

    void Start()
    {
        SaveSystem.LoadPlayer();
        music.clip = gameOverSound;
        music.Play();
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(10);
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
        SaveSystem.SavePlayer(player);
        SceneManager.LoadScene("Loading");
    }

    public void NoPressed()
    {
        player.ResetPlayer();
        SaveSystem.SavePlayer(player);
        Initiate.Fade("Main Menu", Color.black, 1f);
    }
}
