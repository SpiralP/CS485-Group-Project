using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public int lives = 3;
    public int currentLevel = 1;
    public int totalscore = 0;
    public int lifetimebestscore = 0;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        Debug.Log("Saved Game");
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        currentLevel = data.currentLevel;
        health = data.health;
        lives = data.lives;
        totalscore = data.totalscore;
        lifetimebestscore = data.lifetimebestscore;

    }
}