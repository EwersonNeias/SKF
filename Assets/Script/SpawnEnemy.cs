using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        public Transform spawnPoint;
        [HideInInspector]
        public GameObject currentInstance;
    }

    public List<SpawnableObject> objectsToSpawn;
    public float respawnDelay = 1f; // Tempo de espera antes de respawnar

    void Start()
    {
        foreach (var obj in objectsToSpawn)
        {
            SpawnObject(obj);
        }
    }

    void Update()
    {
        foreach (var obj in objectsToSpawn)
        {
            if (obj.currentInstance == null)
            {
                StartCoroutine(RespawnObject(obj));
            }
        }
    }

    void SpawnObject(SpawnableObject obj)
    {
        obj.currentInstance = Instantiate(obj.prefab, obj.spawnPoint.position, obj.spawnPoint.rotation);
    }

    IEnumerator RespawnObject(SpawnableObject obj)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnObject(obj);
    }
}
