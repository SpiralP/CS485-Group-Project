using UnityEngine;
using UnityEngine.Events;


public class LevelCompleteCollision : MonoBehaviour
{
    public CollisionEvent orbCollide;

    void Start()
    {
        orbCollide = new CollisionEvent();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            orbCollide.Invoke(collision.gameObject);
        }
    }

    void Collide(GameObject orb)
    {
        Destroy(orb);
    }
}
