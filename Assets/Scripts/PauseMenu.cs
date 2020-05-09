using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public AudioSource sfx;
    public AudioSource music;
    public AudioClip pause;
    public AudioClip resume;

    // UI stuff to hide while game is paused
    public GameObject pauseIcon;
    public Slider healthbar;
    public GameObject livesIcon;
    public GameObject livesCounter;
    public GameObject fireButton;
    public GameObject upButton;
    public GameObject upGraphic;
    public GameObject leftButton;
    public GameObject leftGraphic;
    public GameObject rightButton;
    public GameObject rightGraphic;
    public GameObject downButton;
    public GameObject downGraphic;
    public GameObject armorButton;
    public GameObject powerTimer;
    public GameObject armorTimer;


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        sfx.clip = resume;
        sfx.Play();
        music.UnPause();
        GameIsPaused = false;

        // show UI elements again
        pauseIcon.SetActive(true);
        healthbar.gameObject.SetActive(true);
        livesIcon.SetActive(true);
        livesCounter.SetActive(true);
        fireButton.SetActive(true);
        upButton.SetActive(true);
        upGraphic.SetActive(true);
        rightButton.SetActive(true);
        rightGraphic.SetActive(true);
        leftButton.SetActive(true);
        leftGraphic.SetActive(true);
        downButton.SetActive(true);
        downGraphic.SetActive(true);
        if (ShipCollision.HasArmorPickup)
        {
            armorButton.SetActive(true);
        }
        if (ShipCollision.ArmorPowerupStartTime != 0f)
        {
            armorTimer.SetActive(true);
        }
        if (ShipCollision.PowerPowerupStartTime != 0f)
        {
            powerTimer.SetActive(true);
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        music.Pause();
        sfx.clip = pause;
        sfx.Play();
        GameIsPaused = true;

        // hide UI elements
        armorButton.SetActive(false);
        pauseIcon.SetActive(false);
        healthbar.gameObject.SetActive(false);
        livesIcon.SetActive(false);
        livesCounter.SetActive(false);
        fireButton.SetActive(false);
        upButton.SetActive(false);
        upGraphic.SetActive(false);
        rightButton.SetActive(false);
        rightGraphic.SetActive(false);
        leftButton.SetActive(false);
        leftGraphic.SetActive(false);
        downButton.SetActive(false);
        downGraphic.SetActive(false);
        armorTimer.SetActive(false);
        powerTimer.SetActive(false);
    }

    public void Return()
    {
        sfx.clip = resume;
        sfx.Play();
        Time.timeScale = 1f;
        // reset killcounts
        GM.totalEnemiesKilled -= (GM.enemiesKilledNormally + GM.enemiesKilledPowerfully);
        GM.totalPowerupsCollected -= GM.powerupsCollected;
        GM.enemiesKilledNormally = 0;
        GM.enemiesKilledPowerfully = 0;
        GM.powerupsCollected = 0;
        Initiate.Fade("Main Menu", Color.black, 1f);
    }
}
