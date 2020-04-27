using UnityEngine;

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
        pauseIcon.SetActive(false);
    }

    public void Return()
    {
        sfx.clip = resume;
        sfx.Play();
        Time.timeScale = 1f;
        Initiate.Fade("Main Menu", Color.black, 1f);
    }
}
