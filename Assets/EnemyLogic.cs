using System.Collections;
using UnityEngine;

/// shoots at you???
public class EnemyLogic : MonoBehaviour {
    public Transform bulletSpawnPos;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;

    private Rigidbody myRigidBody;
    public int gridX, gridY;

    void Start() {
        myRigidBody = GetComponent<Rigidbody>();
        Game.OccupiedGrid.Add(new Vector2(gridX, gridY));
    }

    void ShootBullet() {
        StartCoroutine(Burst());
    }

    private bool running = false;
    void Update() {
        if (!running && transform.position.z < 10) {
            running = true;
            StartCoroutine(StopMoving());
        }
    }

    IEnumerator StopMoving() {
        InvokeRepeating("ShootBullet", 0.0f, 3.0f);

        var oldVelocity = myRigidBody.velocity;
        myRigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(6.0f);
        // after a couple seconds, flies away
        Game.OccupiedGrid.Remove(new Vector2(gridX, gridY));

        CancelInvoke("ShootBullet");
        yield return new WaitForSeconds(1.0f);

        myRigidBody.velocity = oldVelocity;

    }

    IEnumerator Burst() {
        var r = Random.Range(1, 4);

        for (int i = 0; i < r; i++) {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity);
            Destroy(bullet, 10f);

            var rigidbody = bullet.GetComponent<Rigidbody>();
            rigidbody.velocity = myRigidBody.velocity + new Vector3(0f, 0f, -1f * bulletSpeed);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
