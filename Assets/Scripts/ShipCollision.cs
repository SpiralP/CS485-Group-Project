using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionEvent : UnityEvent<GameObject> { }

public class ShipCollision : MonoBehaviour {
  public CollisionEvent asteroidCollide;

  void Start() {
    asteroidCollide = new CollisionEvent();
    asteroidCollide.AddListener(Collide);
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy") {

      asteroidCollide.Invoke(collision.gameObject);
    }
  }

  void Collide(GameObject asteroid) {
    Destroy(asteroid);
  }
}
