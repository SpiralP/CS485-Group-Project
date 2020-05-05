using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public Slider slider;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        // start async operation
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        string currentLevel = "Level";
        player.LoadPlayer();
        currentLevel += player.currentLevel.ToString();
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(currentLevel);

        while (gameLevel.progress < 1)
        {
            float progress = Mathf.Clamp01(gameLevel.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
