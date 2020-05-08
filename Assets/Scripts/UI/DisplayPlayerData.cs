using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayPlayerData : MonoBehaviour
{
    public TextMeshProUGUI highestScore;
    public TextMeshProUGUI currentScore;
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI livesLeft;
    public Player player;

    void Start()
    {
        player.LoadPlayer();
        highestScore.text = Math.Round(player.lifetimebestscore).ToString();
        currentScore.text = Math.Round(player.totalscore).ToString();
        currentLevel.text = player.currentLevel.ToString();
        livesLeft.text = player.lives.ToString();
    }

    void Update()
    {
        highestScore.text = Math.Round(player.lifetimebestscore).ToString();
        currentScore.text = Math.Round(player.totalscore).ToString();
        currentLevel.text = player.currentLevel.ToString();
        livesLeft.text = player.lives.ToString();
    }
}
