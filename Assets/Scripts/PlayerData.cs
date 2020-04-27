using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int lives;
    public int currentLevel;
    public int totalscore;
    public int lifetimebestscore;

    public PlayerData(Player player)
    {
        health = player.health;
        lives = player.lives;
        currentLevel = player.currentLevel;
        totalscore = player.totalscore;
        lifetimebestscore = player.lifetimebestscore;
    }

}
