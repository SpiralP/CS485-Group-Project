using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public GameObject results;
    public GameObject newRecord;
    public GameObject flawless;
    public AudioSource sfx;
    public AudioClip jingle;
    public AudioClip buttonSound;
    public TextMeshProUGUI score;
    public TextMeshProUGUI bestscore;
    public TextMeshProUGUI enemyCount;
    public TextMeshProUGUI powerUpCount;
    public Player player;
    public Button returnButton;

    void Start()
    {
        player.LoadPlayer();
        SetValues();
        StartCoroutine(Sequence());
    }

    public void ButtonPressed()
    {
        sfx.clip = buttonSound;
        sfx.Play();
        player.ResetPlayer();
        Initiate.Fade("Main Menu", Color.black, 1f);
    }

    private void SetValues()
    {
        if (GM.isAFlawlessRun == true && player.totalscore != 0f)
        {
            player.totalscore += 100000f;
        }
        if (player.totalscore > player.lifetimebestscore)
        {
            player.lifetimebestscore = player.totalscore;
        }
        player.SavePlayer();
        score.text = Math.Round(player.totalscore).ToString();
        bestscore.text = Math.Round(player.lifetimebestscore).ToString();
        enemyCount.text = GM.totalEnemiesKilled.ToString();
        powerUpCount.text = GM.totalPowerupsCollected.ToString();
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(7);
        results.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (player.totalscore == player.lifetimebestscore)
        {
            sfx.clip = jingle;
            sfx.Play();
            newRecord.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        if (GM.isAFlawlessRun == true)
        {
            sfx.clip = jingle;
            sfx.Play();
            flawless.SetActive(true);
        }
        returnButton.gameObject.SetActive(true);
    }
}
