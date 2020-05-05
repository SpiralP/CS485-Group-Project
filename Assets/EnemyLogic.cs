using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyKilledEvent : UnityEvent<GameObject> { }

/// shoots at you???
public class EnemyLogic : MonoBehaviour
{
  public EnemyKilledEvent enemyKilledEvent;
  public Transform bulletSpawnPos;
  public GameObject bulletPrefab;
  public float bulletSpeed = 10.0f;

  private Rigidbody myRigidBody;
  public int gridX, gridY;
  private uint health = 3;

  void Start()
  {
    myRigidBody = GetComponent<Rigidbody>();
    Game.OccupiedGrid.Add(new Vector2(gridX, gridY));

    enemyKilledEvent = new EnemyKilledEvent();
    enemyKilledEvent.AddListener(EnemyKilled);
  }

  void ShootBullet()
  {
    StartCoroutine(Burst());
  }

  private bool running = false;
  void Update()
  {
    if (!running && transform.position.z < 10)
    {
      running = true;
      StartCoroutine(StopMoving());
    }
  }

  IEnumerator StopMoving()
  {
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

  IEnumerator Burst()
  {
    var r = Random.Range(1, 2);

    for (int i = 0; i < r; i++)
    {
      var bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity);
      Destroy(bullet, 10f);

      var rigidbody = bullet.GetComponent<Rigidbody>();
      rigidbody.velocity = myRigidBody.velocity + new Vector3(0f, 0f, -1f * bulletSpeed);

      yield return new WaitForSeconds(0.2f);
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "PlayerBullet")
    {
      Debug.Log("hit");

      Destroy(collision.gameObject);

      if (ShipCollision.PowerPowerupStartTime != 0f || health == 1)
      {
        enemyKilledEvent.Invoke(this.gameObject);
      }
      else
      {
        health -= 1;
      }
    }
  }

  void EnemyKilled(GameObject enemy)
  {
    // TODO explosion
    Destroy(enemy);
  }
}