using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    public Image fillImage;

    public void SetMaxHealth(int health)
    {
        fillImage.fillAmount = 1f;
    }

    public void SetHealth(int health, int maxHealth)
    {
        fillImage.fillAmount = (float)health / maxHealth;
    }
}
