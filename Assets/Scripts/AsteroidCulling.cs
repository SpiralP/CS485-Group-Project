using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectHitAsteroidEvent : UnityEvent<GameObject> { }

public class AsteroidCulling : MonoBehaviour
{
  public ObjectHitAsteroidEvent objectHitAsteroidEvent;
  void Start()
  {
    objectHitAsteroidEvent = new ObjectHitAsteroidEvent();
    objectHitAsteroidEvent.AddListener(ObjectHitAsteroid);
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy")
    {
      objectHitAsteroidEvent.Invoke(collision.gameObject);
    }
  }

  void ObjectHitAsteroid(GameObject obj)
  {
    Destroy(obj);
  }
}
