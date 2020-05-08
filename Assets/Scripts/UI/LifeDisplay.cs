using UnityEngine;

public class LifeDisplay : MonoBehaviour
{
    public Player player;
    public GameObject two;
    public GameObject one;
    public GameObject zero;

    private int lives;

    void Start()
    {
        lives = player.lives;
    }
    void Update()
    {
        switch (lives)
        {
            case 3:
                two.SetActive(true);
                one.SetActive(false);
                zero.SetActive(false);
                break;
            case 2:
                two.SetActive(false);
                one.SetActive(true);
                zero.SetActive(false);
                break;
            case 1:
                two.SetActive(false);
                one.SetActive(false);
                zero.SetActive(true);
                break;
        }
    }
}
