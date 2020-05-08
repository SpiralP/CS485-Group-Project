using UnityEngine;
using UnityEngine.UI;

public class AbilityTimer : MonoBehaviour
{
    public Image timerDisplay;
    public float cooldown;
    bool isDisplaying = false;

    void Update()
    {
        if (ShipCollision.PowerPowerupStartTime != 0f)
        {
            isDisplaying = true;
        }
        if (isDisplaying)
        {
            timerDisplay.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }

    public void ResetAbilityTimer()
    {
        timerDisplay.fillAmount = 1f;
    }
}
