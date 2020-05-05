using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectHitShipEvent : UnityEvent<GameObject> { }

[System.Serializable]
public class ShipDeathEvent : UnityEvent<GameObject> { }


[System.Serializable]
public class PowerupHitShipEvent : UnityEvent<GameObject> { }


public class ShipCollision : MonoBehaviour
{

  public static float PowerPowerupStartTime = 0f;

  public static bool HasArmorPickup = false;
  public static float ArmorPowerupStartTime = 0f;

  public const uint MaxShipHealth = 3;
  public static uint ShipHealth = MaxShipHealth;

  public GameObject parentShipObject;
  public ObjectHitShipEvent objectHitShipEvent;
  public ShipDeathEvent shipDeathEvent;
  public PowerupHitShipEvent powerupHitShipEvent;


  void Start()
  {
    objectHitShipEvent = new ObjectHitShipEvent();
    objectHitShipEvent.AddListener(ObjectHitShip);

    shipDeathEvent = new ShipDeathEvent();
    shipDeathEvent.AddListener(ShipDeath);

    powerupHitShipEvent = new PowerupHitShipEvent();
    powerupHitShipEvent.AddListener(PowerupHitShip);
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Enemy")
    {
      Debug.Log("hurt");
      objectHitShipEvent.Invoke(collision.gameObject);
    }
    if (collision.gameObject.tag == "Armor" || collision.gameObject.tag == "Health" || collision.gameObject.tag == "Power")
    {
      Debug.Log("powerup");
      powerupHitShipEvent.Invoke(collision.gameObject);
    }
  }

  void ObjectHitShip(GameObject obj)
  {
    Destroy(obj);
    if (ArmorPowerupStartTime != 0f)
    {
      // do nothing we invincible baby
    }
    else if (ShipHealth == 1)
    {
      shipDeathEvent.Invoke(parentShipObject);
    }
    else
    {
      ShipHealth -= 1;
    }
  }

  void ShipDeath(GameObject ship)
  {
    Destroy(ship);
  }

  void PowerupHitShip(GameObject powerup)
  {
    Destroy(powerup);

    if (powerup.tag == "Armor")
    {
      // user will store this powerup up and activate it
      // in order to get by a "wall of asteroids"

      // disables collision, make it a bool
      HasArmorPickup = true;
    }
    else if (powerup.tag == "Health")
    {
      // increase HP
      ShipHealth = Math.Min(MaxShipHealth, ShipHealth + 1);
      Debug.Log("Got health pickup, new health " + ShipHealth);
    }
    else if (powerup.tag == "Power")
    {
      // increase bullet damage
      // one hit kill enemies

      ActivatePowerPowerup();
    }
  }

  IEnumerator powerPowerupLogic;
  void ActivatePowerPowerup()
  {
    if (powerPowerupLogic != null)
    {
      StopCoroutine(powerPowerupLogic);
    }

    powerPowerupLogic = PowerPowerupLogic();
    StartCoroutine(powerPowerupLogic);
  }

  IEnumerator PowerPowerupLogic()
  {
    PowerPowerupStartTime = Time.time;

    yield return new WaitForSeconds(4.0f);

    PowerPowerupStartTime = 0f;
  }



  IEnumerator armorPowerupLogic = null;
  public void ActivateArmorPowerup()
  {
    if (armorPowerupLogic != null)
    {
      StopCoroutine(armorPowerupLogic);
    }

    HasArmorPickup = false;

    armorPowerupLogic = ArmorPowerupLogic();
    StartCoroutine(armorPowerupLogic);
  }

  IEnumerator ArmorPowerupLogic()
  {
    ArmorPowerupStartTime = Time.time;

    yield return new WaitForSeconds(4.0f);

    ArmorPowerupStartTime = 0f;
  }
}
