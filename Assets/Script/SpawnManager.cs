using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabs; // Array de prefabs para spawn
    public GameObject spawnParticle; // Sistema de part�culas a ser gerado antes do prefab
    public Vector2 spawnArea; // �rea onde os prefabs ser�o spawnados
    public float spawnDelay = 1f; // Delay entre spawns
    public float spawnTimer = 0f;
    public float despawnDelay = 10f; // Tempo para despawn dos prefabs
    public float particleDuration = 1f; // Dura��o da part�cula

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
        // Escolha aleat�ria de um prefab
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        // Posi��o de spawn baseada na posi��o do objeto SpawnManager
        Vector2 spawnPosition = transform.position;

        // Spawn da part�cula na posi��o gerada
        yield return StartCoroutine(SpawnPrefabWithParticle(prefabToSpawn, spawnPosition));
    }

    private IEnumerator SpawnPrefabWithParticle(GameObject prefab, Vector2 position)
    {
        // Instanciar a part�cula
        GameObject particle = Instantiate(spawnParticle, position, Quaternion.identity);

        // Esperar a dura��o da part�cula
        yield return new WaitForSeconds(particleDuration);

        // Destruir a part�cula
        Destroy(particle);

        // Instanciar o prefab ap�s a part�cula
        GameObject spawnedPrefab = Instantiate(prefab, position, Quaternion.identity);

        // Destruir o prefab ap�s um certo tempo
        Destroy(spawnedPrefab, despawnDelay);
    }
}
