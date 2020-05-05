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
  public static uint ShipHealth;

  public GameObject parentShipObject;
  public ObjectHitShipEvent objectHitShipEvent;
  public ShipDeathEvent shipDeathEvent;
  public PowerupHitShipEvent powerupHitShipEvent;
  public AudioSource sfx;
  public AudioClip powerupGet;
  public AudioClip damageTaken;
  public AudioClip playerDeath;
  public AudioClip activateShield;
  public AudioClip asteroidHitOnArmor;
  private bool invuln;
  public Player player;

  void Start()
  {
    ShipHealth = 3;
    player.LoadPlayer();
    Debug.Log("Lives: " + player.lives + ", Health: " + ShipHealth);
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
      Debug.Log("Hit. Health: " + ShipHealth);
      objectHitShipEvent.Invoke(collision.gameObject);
    }
    if (collision.gameObject.tag == "Armor" || collision.gameObject.tag == "Health" || collision.gameObject.tag == "Power")
    {
      //Debug.Log("powerup");
      sfx.clip = powerupGet;
      sfx.Play();
      powerupHitShipEvent.Invoke(collision.gameObject);
    }
  }

  void ObjectHitShip(GameObject obj)
  {
    Destroy(obj);
    // has armor powerup
    if (ArmorPowerupStartTime != 0f)
    {
      // hitting an asteroid while armor is active - thud sound. Let's player know they've tanked hits and survived.
      if (obj.tag == "Asteroid")
      {
        sfx.clip = asteroidHitOnArmor;
        sfx.Play();
      }
    }

    // asteroid collision with no armor - instant death
    else if (obj.tag == "Asteroid")
    {
        sfx.clip = playerDeath;
        sfx.Play();
        shipDeathEvent.Invoke(parentShipObject);

        // decrement life
        player.lives -= 1;

        // game over if lives run out. Save score, go to game over scene
        if (player.lives == 0)
        {
          // if player reaches a new high score in this run, save that to their lifetime best score
          if (player.totalscore > player.lifetimebestscore)
          {
            player.lifetimebestscore = player.totalscore;
            player.SavePlayer();
          }
          // load game over scene
          Initiate.Fade("Game Over", Color.black, 0.2f);
        }

        // otherwise, save player data and restart this level
        player.SavePlayer();
        Initiate.Fade("Loading", Color.black, 1f);

    }

    // defeated by enemy ship
    else if (ShipHealth == 1)
    {
      // invuln checks are for if the player clears the level, but a stray enemy bullet may still hit them 
      // during the flyout cutscene when they have 1 health left
      invuln = parentShipObject.GetComponent<ShipController>().isInvuln;
      if (!invuln)
      {
        sfx.clip = playerDeath;
        sfx.Play();
        shipDeathEvent.Invoke(parentShipObject);

        // decrement life
        player.lives -= 1;

        // game over if lives run out. Save score, go to game over scene
        if (player.lives == 0)
        {
            // if player reaches a new high score in this run, save that to their lifetime best score
            if (player.totalscore > player.lifetimebestscore)
            {
                player.lifetimebestscore = player.totalscore;
                player.SavePlayer();
            }
            // load game over scene
            Initiate.Fade("Game Over", Color.black, 0.2f);
        }

        // otherwise, save player data and restart this level
        player.SavePlayer();
        Initiate.Fade("Loading", Color.black, 1f);
      }
    }

    // player is hit by an enemy, but still alive
    else
    {
      invuln = parentShipObject.GetComponent<ShipController>().isInvuln;
      if (!invuln)
      {
        sfx.clip = damageTaken;
        sfx.Play();
        ShipHealth -= 1;
      }
    }
  }

  void ShipDeath(GameObject ship)
  {
    Destroy(ship);
    ShipHealth = MaxShipHealth;
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
    sfx.clip = activateShield;
    sfx.Play();
    ArmorPowerupStartTime = Time.time;

    yield return new WaitForSeconds(3.0f);

    ArmorPowerupStartTime = 0f;
  }
}
