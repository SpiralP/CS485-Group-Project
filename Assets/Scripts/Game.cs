using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

  public static HashSet<Vector2> OccupiedGrid = new HashSet<Vector2>();

  /// # of positions you can go to the right
  /// only positions to the right
  public const int gridPositionsX = 2;

  /// # of positions you can go upward
  public const int gridPositionsY = 2;

  public GameObject enemyPrefab;
  public GameObject[] powerupPrefabs;
  public GameObject[] asteroidPrefabs;
  public Transform spawnPosition;
  public Transform movementPlane;
  public float spawnInterval = 0.3f;
  public float asteroidSpeed = 20f;
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
  public GameObject levelCompleteUI;
  private Vector3 center = new Vector3(0f, 0f, 0f);
  public float endTime;

  private bool didBigWall = false;
  private bool doingBigWall = false;

  void Start() {
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
  }

  void Update() {
    currentTime += Time.deltaTime;
    elapsedTime += Time.deltaTime;

    if (!didBigWall && elapsedTime > endTime * 0.8) {
      didBigWall = true;
      doingBigWall = true;

      StartCoroutine(DoBigWall());
    }

    if (doingBigWall) {
      return;
    }

    // start spawning asteroids after cutscene ends
    if (elapsedTime > 6.0f && elapsedTime < endTime) {
      if (currentTime > spawnInterval) {
        currentTime = 0f;

        var gridX = Random.Range(-gridPositionsX + 1, gridPositionsX);
        var gridY = Random.Range(-gridPositionsY + 1, gridPositionsY);

        if (OccupiedGrid.Contains(new Vector2(gridX, gridY))) {
          return;
        }

        var pos = new Vector3(
          gridX * stepX,
          gridY * stepY,
          spawnPosition.position.z
        );

        if (Random.Range(0, 8) == 0) {
          // an enemy
          // int enemyIndex = Random.Range(0, enemyPrefabs.Length);
          GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.Euler(0, 180, 0));
          Destroy(enemy, 30f);

          var enemyLogic = enemy.GetComponent<EnemyLogic>();
          enemyLogic.gridX = gridX;
          enemyLogic.gridY = gridY;
          enemyLogic.sfx = sfx;
          enemyLogic.enemyDeath = enemyKilled;

          Rigidbody body = enemy.GetComponent<Rigidbody>();
          body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
        } else if (Random.Range(0, 10) == 0) {
          // powerup
          GameObject powerup = Instantiate(powerupPrefabs[Random.Range(0, powerupPrefabs.Length)], pos, Quaternion.identity);
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
    int asteroidIndex = Random.Range(0, asteroidPrefabs.Length);
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
      Debug.Log("spawning armor powerup");
      int gridX;
      int gridY;

      do {
        gridX = Random.Range(-gridPositionsX + 1, gridPositionsX);
        gridY = Random.Range(-gridPositionsY + 1, gridPositionsY);
      } while (OccupiedGrid.Contains(new Vector2(gridX, gridY)));

      var pos = new Vector3(
        gridX * stepX,
        gridY * stepY,
        spawnPosition.position.z
      );

      GameObject powerup = Instantiate(powerupPrefabs[0], pos, Quaternion.identity);
      Rigidbody body = powerup.GetComponent<Rigidbody>();
      body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
    }

    yield return new WaitForSeconds(0.25f);

    Debug.Log("spawning wall");
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
    music.clip = outroSong;
    music.Play();
    yield return new WaitForSeconds(0.5f);
    levelCompleteUI.SetActive(true);
  }

  public void ReturnToMainMenu() {
    // save game here
    Initiate.Fade("Main Menu", Color.black, 1f);
  }

  public void NextLevel() {
    // load current level index + 1
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
