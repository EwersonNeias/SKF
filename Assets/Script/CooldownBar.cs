using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Image cooldownBarFill; // Referência à imagem de preenchimento da barra de cooldown
    private float cooldownDuration; // Duração total do cooldown
    private float cooldownTimeRemaining; // Tempo restante do cooldown

    void Update()
    {
        if (cooldownTimeRemaining > 0)
        {
            cooldownTimeRemaining -= Time.deltaTime;
            cooldownBarFill.fillAmount = cooldownTimeRemaining / cooldownDuration;
        }
    }

    // Inicia o cooldown com a duração especificada
    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        cooldownTimeRemaining = duration;
        cooldownBarFill.fillAmount = 1f;
    }

    // Verifica se o cooldown terminou
    public bool IsCooldownComplete()
    {
        return cooldownTimeRemaining <= 0;
    }
}
