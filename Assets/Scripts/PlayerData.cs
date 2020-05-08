using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
  public int lives;
  public int currentLevel;
  public float totalscore;
  public float lifetimebestscore;

  public PlayerData(Player player) {
    lives = player.lives;
    currentLevel = player.currentLevel;
    totalscore = player.totalscore;
    lifetimebestscore = player.lifetimebestscore;
  }
}
