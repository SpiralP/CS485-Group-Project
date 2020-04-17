using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionDeleteEvent : UnityEvent<GameObject>
{
}

public class AsteroidCulling : MonoBehaviour
{
  public CollisionDeleteEvent asteroidCollide;
  // Start is called before the first frame update
  void Start()
  {
    asteroidCollide = new CollisionDeleteEvent();
    asteroidCollide.AddListener(Collide);
  }


  // Update is called once per frame
  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Asteroid")
    {
      asteroidCollide.Invoke(collision.gameObject);
    }
  }

  void Collide(GameObject asteroid)
  {
    Destroy(asteroid);
  }
}
