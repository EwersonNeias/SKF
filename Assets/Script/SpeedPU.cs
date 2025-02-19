using System.Collections;
using UnityEngine;

public class SpeedPU : MonoBehaviour
{
    public float increase = 2f;
    public float duration = 5f;

    void Start()
    {
        // Destroy the game object after 8 seconds
        Destroy(gameObject, 8f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            GameObject player = collision.gameObject;
            PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

            if (playerScript != null)
            {
                playerScript.ApplySpeedBoost(increase, duration);
                Destroy(gameObject);
            }
        }
    }
}
