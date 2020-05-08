using UnityEngine;
using UnityEngine.UI;

public class ArmorTimer : MonoBehaviour
{
    public Image timerDisplay;
    public float cooldown;
    bool isDisplaying = false;

    void Update()
    {
        if (ShipCollision.ArmorPowerupStartTime != 0f)
        {
            isDisplaying = true;
        }
        if (isDisplaying)
        {
            timerDisplay.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }
}
