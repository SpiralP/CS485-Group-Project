using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
  public int lives = 3;
  public int currentLevel = 1;
  public float totalscore = 0f;
  public float lifetimebestscore = 0f;

  public void SavePlayer() {
    SaveSystem.SavePlayer(this);
    Debug.Log("Saved Game");
  }

  public void LoadPlayer() {
    PlayerData data = SaveSystem.LoadPlayer();

    if (data != null) {
      currentLevel = data.currentLevel;
      lives = data.lives;
      totalscore = data.totalscore;
      lifetimebestscore = data.lifetimebestscore;
    }
  }

  public void ResetPlayer() {
    PlayerData data = SaveSystem.LoadPlayer();
    lives = 3;
    currentLevel = 1;
    totalscore = 0f;
    lifetimebestscore = data.lifetimebestscore;
    SavePlayer();
  }
}
