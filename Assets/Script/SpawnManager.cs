using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs; // Array de prefabs para spawn
    public GameObject spawnParticle; // Sistema de partículas a ser gerado antes do prefab
    public Vector2 spawnArea; // Área onde os prefabs serão spawnados
    public float spawnDelay = 1f; // Delay entre spawns
    public float spawnTimer = 0f;
    public float despawnDelay = 10f; // Tempo para despawn dos prefabs
    public float particleDuration = 1f; // Duração da partícula

    void Update()
    {
        // Controle do tempo entre os spawns
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnDelay)
        {
            StartCoroutine(SpawnRandomPrefabWithParticle());
            spawnTimer = 0f;
        }
    }

    private IEnumerator SpawnRandomPrefabWithParticle()
    {
        // Escolha aleatória de um prefab
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        // Posição de spawn baseada na posição do objeto SpawnManager
        Vector2 spawnPosition = transform.position;

        // Spawn da partícula na posição gerada
        yield return StartCoroutine(SpawnPrefabWithParticle(prefabToSpawn, spawnPosition));
    }

    private IEnumerator SpawnPrefabWithParticle(GameObject prefab, Vector2 position)
    {
        // Instanciar a partícula
        GameObject particle = Instantiate(spawnParticle, position, Quaternion.identity);

        // Esperar a duração da partícula
        yield return new WaitForSeconds(particleDuration);

        // Destruir a partícula
        Destroy(particle);

        // Instanciar o prefab após a partícula
        GameObject spawnedPrefab = Instantiate(prefab, position, Quaternion.identity);

        // Destruir o prefab após um certo tempo
        Destroy(spawnedPrefab, despawnDelay);
    }
}
