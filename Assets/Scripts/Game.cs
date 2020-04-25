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
    public GameObject[] asteroidPrefabs;
    public Transform spawnPosition;
    public Transform movementPlane;
    public float spawnInterval = 0.25f;
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
    public GameObject player;
    public AudioClip button;
    public GameObject levelCompleteUI;
    private Vector3 center = new Vector3(0f, 0f, 0f);
    public float endTime;

    void Start() {
        music.clip = gameSong;
        music.Play();
        stepX = (movementPlane.localScale.x * 5f) / gridPositionsX;
        stepY = (movementPlane.localScale.z * 5f) / gridPositionsY;
    }

    void Update() {
        currentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;
        // start spawning asteroids after cutscene ends
        if (elapsedTime > 6.0f && elapsedTime < endTime) {
            if (currentTime > spawnInterval) {
                currentTime = 0f;

                var gridX = Random.Range(-gridPositionsX + 1, gridPositionsX);
                var gridY = Random.Range(-gridPositionsY + 1, gridPositionsY);

                if (OccupiedGrid.Contains(new Vector2(gridX, gridY))) { return; }

                var pos = new Vector3(
                    gridX * stepX,
                    gridY * stepY,
                    spawnPosition.position.z
                );

                if (Random.Range(0, 5) == 0) {
                    // an enemy
                    // int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                    GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.Euler(0, 180, 0));
                    Destroy(enemy, 30f);

                    var enemyLogic = enemy.GetComponent<EnemyLogic>();
                    enemyLogic.gridX = gridX;
                    enemyLogic.gridY = gridY;

                    Rigidbody body = enemy.GetComponent<Rigidbody>();
                    body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
                } else {
                    // an asteroid
                    int asteroidIndex = Random.Range(0, asteroidPrefabs.Length);
                    GameObject asteroid = Instantiate(asteroidPrefabs[asteroidIndex], pos, Quaternion.identity);
                    Destroy(asteroid, 30f);

                    Rigidbody body = asteroid.GetComponent<Rigidbody>();
                    body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
                    body.AddRelativeTorque(Vector3.up * 10);
                }
            }
        }
        if (elapsedTime > endTime && !levelComplete) {
            StartCoroutine(FinishLevel());
        }
    }

    IEnumerator FinishLevel() {
        levelComplete = true;
        yield return new WaitForSeconds(3);
        sfx.clip = finish;
        sfx.Play();
        GameObject.Find("Player Ship").GetComponent<ShipController>().canPlayerMove = false;
        yield return new WaitForSeconds(2);
        player.transform.position = center;
        exitcam.SetActive(true);
        gameCam.SetActive(false);
        GameObject.Find("Player Ship").GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.5f);
        sfx.clip = flyout;
        sfx.Play();
        yield return new WaitForSeconds(3);
        player.SetActive(false);
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
}
