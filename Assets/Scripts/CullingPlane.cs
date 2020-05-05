using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// this is for the plane/cuboid behind the ship that will destroy
// objects that go past the player

public class CullingPlane : MonoBehaviour
{
  void OnCollisionEnter(Collision collision)
  {
    if (
      collision.gameObject.tag == "Asteroid"
      || collision.gameObject.tag == "Bullet"
      || collision.gameObject.tag == "Enemy"
      || collision.gameObject.tag == "Armor"
      || collision.gameObject.tag == "Health"
      || collision.gameObject.tag == "Power"
    )
    {
      Destroy(collision.gameObject);
    }
  }
}
