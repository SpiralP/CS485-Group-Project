using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectHitShipEvent : UnityEvent<GameObject> { }


[System.Serializable]
public class ShipDeathEvent : UnityEvent<GameObject> { }

public class ShipCollision : MonoBehaviour
{
  public ObjectHitShipEvent objectHitShipEvent;
  public ShipDeathEvent shipDeathEvent;
  public uint health = 3;

  void Start()
  {
    objectHitShipEvent = new ObjectHitShipEvent();
    objectHitShipEvent.AddListener(ObjectHitShip);


    shipDeathEvent = new ShipDeathEvent();
    shipDeathEvent.AddListener(ShipDeath);
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy")
    {
      objectHitShipEvent.Invoke(collision.gameObject);
    }
  }

  void ObjectHitShip(GameObject obj)
  {
    Destroy(obj);

    if (health == 0)
    {
      shipDeathEvent.Invoke(this.gameObject);
    }
    else
    {
      health -= 1;
    }
  }



  void ShipDeath(GameObject ship)
  {
    Destroy(ship);
  }
}
