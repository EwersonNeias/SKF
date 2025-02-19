using UnityEngine;

public class CanBraw : MonoBehaviour
{
    public float minZoom = 5f; // Zoom m�nimo ajustado
    public float maxZoom = 10f; // Zoom m�ximo ajustado
    public float zoomSpeed = 5f; // Velocidade de ajuste do zoom

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int totalEntities = players.Length + enemies.Length;

        if (totalEntities > 0)
        {
            // Calcula a posi��o m�dia entre os jogadores e inimigos
            Vector3 targetMidPoint = Vector3.zero;
            foreach (GameObject player in players)
            {
                targetMidPoint += player.transform.position;
            }
            foreach (GameObject enemy in enemies)
            {
                targetMidPoint += enemy.transform.position;
            }
            targetMidPoint /= totalEntities;

            // Calcula a dist�ncia m�xima entre os jogadores e inimigos
            float maxDistance = 0f;
            if (players.Length > 1)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    for (int j = i + 1; j < players.Length; j++)
                    {
                        float distance = Vector3.Distance(players[i].transform.position, players[j].transform.position);
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                        }
                    }
                }
            }

            if (enemies.Length > 1)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    for (int j = i + 1; j < enemies.Length; j++)
                    {
                        float distance = Vector3.Distance(enemies[i].transform.position, enemies[j].transform.position);
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                        }
                    }
                }
            }

            for (int i = 0; i < players.Length; i++)
            {
                for (int j = 0; j < enemies.Length; j++)
                {
                    float distance = Vector3.Distance(players[i].transform.position, enemies[j].transform.position);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                }
            }

            // Calcula o tamanho de zoom com base na dist�ncia m�xima entre os jogadores e inimigos
            float targetZoom = Mathf.Lerp(minZoom, maxZoom, maxDistance / (maxZoom * 2f));

            // Atualiza o tamanho da c�mera com uma interpola��o suave
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

            // Define a posi��o da c�mera como a posi��o m�dia entre os jogadores e inimigos
            transform.position = new Vector3(targetMidPoint.x, targetMidPoint.y, transform.position.z);
        }
    }
}
