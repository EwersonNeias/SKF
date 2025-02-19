using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float increase = 2f;
    public float duration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

            if (playerScript != null)
            {
                StartCoroutine(ApplySpeedBoost(playerScript));
            }
        }
    }

    private IEnumerator ApplySpeedBoost(PlayerMovement player)
    {
        player.speed += increase;  // Aumenta a velocidade
        yield return new WaitForSeconds(duration);  // Aguarda pelo tempo de duração
        player.speed -= increase;  // Retorna a velocidade ao normal
    }
}