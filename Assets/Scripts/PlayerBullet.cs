using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Asteroid")
    {
      Destroy(this.gameObject);
    }
  }
}