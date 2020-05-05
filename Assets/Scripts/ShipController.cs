using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
  public Transform movementPlane;
  public GameObject bulletPrefab;
  public Transform bulletSpawnPos;
  public float bulletSpeed = 10.0f;

  private float stepX;
  private float stepY;
  //movement speed in units per second

  // start in the middle: 0, 0
  private int posX = 0;
  private int posY = 0;

  public bool canPlayerMove;

  private IEnumerator coroutine;

  void Start()
  {
    canPlayerMove = true;
    stepX = (movementPlane.localScale.x * 5f) / Game.gridPositionsX;
    stepY = (movementPlane.localScale.z * 5f) / Game.gridPositionsY;
  }

  void Update()
  {
    if (canPlayerMove)
    {
      Vector3 wantedPos = new Vector3(posX * stepX, posY * stepY, 0);

      if (Input.GetKeyDown("w"))
      {
        // move up
        posY = Mathf.Min(posY + 1, Game.gridPositionsY - 1);
      }
      else if (Input.GetKeyDown("s"))
      {
        posY = Mathf.Max(posY - 1, -1 * Game.gridPositionsY + 1);
      }
      else if (Input.GetKeyDown("a"))
      {
        posX = Mathf.Max(posX - 1, -1 * Game.gridPositionsX + 1);
      }
      else if (Input.GetKeyDown("d"))
      {
        posX = Mathf.Min(posX + 1, Game.gridPositionsX - 1);
      }

      if (Input.GetKeyDown("space"))
      {
        StartShooting();
      }

      // if we have an armor pickup, we haven't enabled it yet, and key is pressed
      if (
        ShipCollision.HasArmorPickup &&
        ShipCollision.ArmorPowerupStartTime == 0f &&
        Input.GetKeyDown("e")
      )
      {
        GameObject.Find("Player Ship").GetComponentInChildren<ShipCollision>().ActivateArmorPowerup();
      }

      transform.position = Vector3.MoveTowards(transform.position, wantedPos, 10f * Time.deltaTime);

    }
  }

  void StartShooting()
  {
    if (coroutine != null)
    {
      StopShooting();
    }

    coroutine = FireBullets();
    StartCoroutine(coroutine);
  }

  void StopShooting()
  {
    StopCoroutine(coroutine);
    coroutine = null;
  }

  IEnumerator FireBullets()
  {
    while (canPlayerMove && Input.GetKey("space"))
    {
      var bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity);
      Destroy(bullet, 10f);

      var rigidbody = bullet.GetComponent<Rigidbody>();
      rigidbody.velocity = new Vector3(0f, 0f, 1f * bulletSpeed);

      yield return new WaitForSeconds(0.3f);
    }

    coroutine = null;
  }
}
