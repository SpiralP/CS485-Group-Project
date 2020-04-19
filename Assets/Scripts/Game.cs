using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    /// # of positions you can go to the right
    /// only positions to the right
    public int gridPositionsX = 2;

    /// # of positions you can go upward
    public int gridPositionsY = 2;

    public GameObject[] asteroidPrefabs;
    public Transform spawnPlane;
    public float spawnInterval = 1f;
    public float asteroidSpeed = 20f;
    private float currentTime = 0f;
    private float elapsedTime = 0f;
    private float stepX;
    private float stepY;

    void Start()
    {
        stepX = (spawnPlane.localScale.x * 5f) / gridPositionsX;
        stepY = (spawnPlane.localScale.z * 5f) / gridPositionsY;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;
        Debug.Log(elapsedTime);
        // start spawning asteroids after cutscene ends
        if (elapsedTime > 6.0f)
        {
            if (currentTime > spawnInterval)
            {
                currentTime = 0f;
                float posX = Random.Range(-gridPositionsX + 1, gridPositionsX) * stepX;
                float posY = Random.Range(-gridPositionsY + 1, gridPositionsY) * stepY;
                float posZ = spawnPlane.position.z;

                int asteroidIndex = Random.Range(0, asteroidPrefabs.Length);
                GameObject asteroid = Instantiate(asteroidPrefabs[asteroidIndex], new Vector3(posX, posY, posZ), Quaternion.identity);
                Destroy(asteroid, 30f);

                Rigidbody body = asteroid.GetComponent<Rigidbody>();
                body.velocity = new Vector3(0f, 0f, -1f * asteroidSpeed);
                body.AddRelativeTorque(Vector3.up * 10);
            }
        }
    }
}
