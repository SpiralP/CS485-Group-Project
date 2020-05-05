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
    if (collision.gameObject.tag == "Health")
    {
      Destroy(this.gameObject);
    }
    if (collision.gameObject.tag == "Power")
    {
        Destroy(this.gameObject);
    }
    if (collision.gameObject.tag == "Armor")
    {
        Destroy(this.gameObject);
    }
  }
}