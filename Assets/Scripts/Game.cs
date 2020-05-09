using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

  public static HashSet<Vector2> OccupiedGrid = new HashSet<Vector2>();

  /// # of positions you can go to the right
  /// only positions to the right
  public const int gridPositionsX = 2;

  /// # of positions you can go upward
  public const int gridPositionsY = 2;

  public GameObject enemyPrefab;
  public GameObject[] powerupPrefabs;
  public GameObject armorPowerupPrefab;
  public GameObject[] asteroidPrefabs;
  public Transform spawnPosition;
  public Transform movementPlane;
  private float currentTime = 0f;
  private float elapsedTime = 0f;
  private float stepX;
  private float stepY;
  private bool levelComplete = false;
  public AudioSource sfx;
  public AudioSource music;
  public AudioClip finish;
  public GameObject exitcam;
  public GameObject gameCam;
  public AudioClip gameSong;
  public AudioClip outroSong;
  public AudioClip flyout;
  public GameObject playerShip;
  public AudioClip button;
  public AudioClip enemyKilled;
  public AudioClip finishedLoop;
  public GameObject levelCompleteUI;
  private Vector3 center = new Vector3(0f, 0f, 0f);
  public Player player;
  public TextMeshProUGUI scoreDisplay;
  public GameObject flawlessMessage;
  public Button armorButton;
  private bool didBigWall = false;
  private bool doingBigWall = false;

  // interchangable level variables
  public float asteroidSpeed;
  public float endTime;
  public float spawnInterval;
  public float victoryPointBonus;
  public float flawlessPointBonus;
  public int powerupSpawnChance;
  public int enemySpawnChance;
  public uint enemyHP;

  void Start() {
    player.LoadPlayer();
    music.clip = gameSong;
    music.Play();
    stepX = (movementPlane.localScale.x * 5f) / gridPositionsX;
    stepY = (movementPlane.localScale.z * 5f) / gridPositionsY;
    OccupiedGrid = new HashSet<Vector2>();
    currentTime = 0f;
    elapsedTime = 0f;
    levelComplete = false;
    didBigWall = false;
    doingBigWall = false;
    if (ShipCollision.ArmorPowerupStartTime != 0f)
    {
      ShipCollision.ArmorPowerupStartTime = 0f;
    }
  }

  void Update() {
    Debug.Log("Total KC: " + GM.totalEnemiesKilled + ", Total P: " + GM.totalPowerupsCollected + ", Curr NKC: " + GM.enemiesKilledNormally + ", Curr PKC: " + GM.enemiesKilledPowerfully + ", Curr P: " + GM.powerupsCollected);

    currentTime += Time.deltaTime;
    elapsedTime += Time.deltaTime;
    scoreDisplay.text = Math.Round(player.totalscore).ToString();

    if (!didBigWall && elapsedTime > endTime * 0.5) {
      didBigWall = true;
      doingBigWall = true;
      StartCoroutine(DoBigWall());
    }

    if (doingBigWall) {
      return;
    }
    if (player.currentLevel == 1 && player.totalscore > 0f && elapsedTime < 6.0f) {
      player.totalscore = 0f;
    }

    if (ShipCollision.HasArmorPickup && !PauseMenu.GameIsPaused) {
      armorButton.gameObject.SetActive(true);
    } else {
      armorButton.gameObject.SetActive(false);
    }

    // start spawning asteroids after cutscene ends
    if (elapsedTime > 6.0f && elapsedTime < endTime) {
      player.totalscore += Time.deltaTime * 100f;
      if (currentTime > spawnInterval) {
        currentTime = 0f;

        var gridX = UnityEngine.Random.Range(-gridPositionsX + 1, gridPositionsX);
        var gridY = UnityEngine.Random.Range(-gridPositionsY + 1, gridPositionsY);

        if (OccupiedGrid.Contains(new Vector2(gridX, gridY))) {
          return;
        }

        var pos = new Vector3(
          gridX * stepX,
          gridY * stepY,
          spawnPosition.position.z
        );
        if (UnityEngine.Random.Range(0, enemySpawnChance) == 0) {
          // an enemy
          // int enemyIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
          GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.Euler(0, 180, 0));
          Destroy(enemy, 30f);

          var enemyLogic = enemy.GetComponent<EnemyLogic>();
          enemyLogic.gridX = gridX;
          enemyLogic.gridY = gridY;
          enemyLogic.sfx = sfx;
          enemyLogic.enemyDeath = enemyKilled;
          enemyLogic.health = enemyHP;

          Rigidbody body = enemy.GetComponent<Rigidbody>();
          body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
        } else if (UnityEngine.Random.Range(0, powerupSpawnChance) == 0) {
          // powerup
          GameObject powerup = Instantiate(powerupPrefabs[UnityEngine.Random.Range(0, powerupPrefabs.Length)], pos, Quaternion.identity);
          Rigidbody body = powerup.GetComponent<Rigidbody>();
          body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
        } else {
          SpawnAsteroid(pos);
        }
      }
    }

    if (elapsedTime > endTime && !levelComplete) {
      StartCoroutine(FinishLevel());
    }
  } // Update

  void SpawnAsteroid(Vector3 pos) {
    // an asteroid
    int asteroidIndex = UnityEngine.Random.Range(0, asteroidPrefabs.Length);
    GameObject asteroid = Instantiate(asteroidPrefabs[asteroidIndex], pos, Quaternion.identity);
    Destroy(asteroid, 30f);

    Rigidbody body = asteroid.GetComponent<Rigidbody>();
    body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
    body.AddRelativeTorque(Vector3.up * 10);
  }

  IEnumerator DoBigWall() {
    // don't spawn objects for a couple seconds so the powerup is clear to see
    yield return new WaitForSeconds(0.25f);

    // spawn armor powerup
    {
      int gridX;
      int gridY;

      do {
        gridX = UnityEngine.Random.Range(-gridPositionsX + 1, gridPositionsX);
        gridY = UnityEngine.Random.Range(-gridPositionsY + 1, gridPositionsY);
      } while (OccupiedGrid.Contains(new Vector2(gridX, gridY)));

      var pos = new Vector3(
        gridX * stepX,
        gridY * stepY,
        spawnPosition.position.z
      );

      GameObject powerup = Instantiate(armorPowerupPrefab, pos, Quaternion.identity);
      Rigidbody body = powerup.GetComponent<Rigidbody>();
      body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
    }

    yield return new WaitForSeconds(1.0f);

    for (int gridX = -gridPositionsX + 1; gridX < gridPositionsX; gridX++) {
      for (int gridY = -gridPositionsY + 1; gridY < gridPositionsY; gridY++) {
        // if (OccupiedGrid.Contains(new Vector2(gridX, gridY))) {
        //   continue;
        // }

        var pos = new Vector3(
          gridX * stepX,
          gridY * stepY,
          spawnPosition.position.z
        );

        SpawnAsteroid(pos);
      }
    }

    yield return new WaitForSeconds(0.25f);
    // start spawning other objects behind the wall
    doingBigWall = false;
  }

  IEnumerator FinishLevel() {
    levelComplete = true;
    yield return new WaitForSeconds(3);
    GameObject.Find("Player Ship").GetComponent<ShipController>().isInvuln = true;
    sfx.clip = finish;
    sfx.Play();
    GameObject.Find("Player Ship").GetComponent<ShipController>().canPlayerMove = false;
    yield return new WaitForSeconds(2);
    playerShip.transform.position = center;
    ClearBoardOnVictory();
    exitcam.SetActive(true);
    gameCam.SetActive(false);
    GameObject.Find("Player Ship").GetComponent<Animation>().Play();
    yield return new WaitForSeconds(0.5f);
    sfx.clip = flyout;
    sfx.Play();
    yield return new WaitForSeconds(3);
    playerShip.SetActive(false);
    music.Stop();

    player.totalscore += victoryPointBonus;
    if (!ShipCollision.HasTakenDamage) {
      // bonus for no damage taken
      player.totalscore += flawlessPointBonus;
    }

    // add points for how many enemies killed
    player.totalscore += (GM.enemiesKilledNormally * 500f);
    player.totalscore += (GM.enemiesKilledPowerfully * 1000f);

    // store this score. If the player dies in the next level, it will reset to this score
    GM.savedPlayerTotalScore = player.totalscore;

    // reset level killcounts
    GM.enemiesKilledNormally = 0;
    GM.enemiesKilledPowerfully = 0;
    GM.powerupsCollected = 0;

    // check for new highscore
    if (player.totalscore > player.lifetimebestscore) {
      player.lifetimebestscore = player.totalscore;
    }
    // increment player level
    if (player.currentLevel != 4) {
      player.currentLevel += 1;
    }
    player.SavePlayer();

    music.clip = outroSong;
    music.Play();
    yield return new WaitForSeconds(0.5f);
    levelCompleteUI.SetActive(true);
    if (ShipCollision.HasTakenDamage) {
      flawlessMessage.SetActive(false);
    }
    yield return new WaitForSeconds(20f);
    music.clip = finishedLoop;
    music.loop = true;
    music.Play();
  }

  public void ReturnToMainMenu() {
    Initiate.Fade("Main Menu", Color.black, 1f);
  }

  public void NextLevel() {
    Initiate.Fade("Loading", Color.black, 1f);
  }

  // finish game button - go to beaten the game screen
  public void ResultsScreen() { 
    Initiate.Fade("Results Screen", Color.black, 0.5f);  
  }

  public void ButtonSound() {
    sfx.clip = button;
    sfx.Play();
  }

  private void ClearBoardOnVictory() {
    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (GameObject o in enemies) {
      Destroy(o);
    }
  }

}
