using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Array de prefabs de armas que podem ser spawnadas
    public Transform[] spawnPoints; // Array de pontos de spawn

    public float spawnInterval = 5f; // Intervalo entre cada spawn de arma

    private void Start()
    {
        StartCoroutine(SpawnWeapon());
    }

    private IEnumerator SpawnWeapon()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Seleciona um ponto de spawn aleat�rio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Seleciona uma arma aleat�ria
            GameObject weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];

            // Instancia a arma no ponto de spawn
            Instantiate(weaponPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
