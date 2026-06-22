using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private int initialChunks = 5;
    [SerializeField] private int maxActiveChunks = 10;
    [SerializeField] private float destroyDistance = 50f;
    [SerializeField] private float spawnAheadDistance = 50f;
    [SerializeField] private float spawnThreshold = 2f;

    private readonly List<GameObject> activeChunks = new List<GameObject>();
    private float nextChunkStartZ;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (chunkPrefabs == null || chunkPrefabs.Length == 0)
        {
            Debug.LogWarning("PlatformSpawner has no chunk prefabs assigned.");
            return;
        }

        if (player != null)
        {
            nextChunkStartZ = player.position.z;
        }
        else
        {
            nextChunkStartZ = transform.position.z;
        }

        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void Update()
    {
        if (player == null || chunkPrefabs == null || chunkPrefabs.Length == 0)
        {
            return;
        }

        if (player.position.z >= nextChunkStartZ - spawnAheadDistance - spawnThreshold)
        {
            SpawnChunk();
        }

        RemoveOldChunks();
    }

    private void SpawnChunk()
    {
        if (activeChunks.Count >= maxActiveChunks)
        {
            return;
        }

        GameObject prefab = SelectChunkPrefab();

        Vector3 spawnPosition = new Vector3(0, 0, nextChunkStartZ);

        GameObject chunk = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
        activeChunks.Add(chunk);

        float chunkSize = GetChunkLength(chunk);
        nextChunkStartZ += chunkSize > 0f ? chunkSize : chunkLength;
    }

    private GameObject SelectChunkPrefab()
    {
        if (activeChunks.Count == 0)
        {
            foreach (GameObject prefab in chunkPrefabs)
            {
                if (prefab == null)
                {
                    continue;
                }

                ChunkData chunkData = prefab.GetComponent<ChunkData>();
                if (chunkData != null && chunkData.HasEntrance(ChunkEntrance.Middle))
                {
                    return prefab;
                }
            }
        }

        ChunkData previousChunkData = null;

        if (activeChunks.Count > 0)
        {
            GameObject previousChunk = activeChunks[activeChunks.Count - 1];
            previousChunkData = previousChunk.GetComponent<ChunkData>();
        }

        ChunkEntrance requiredEntrance = ChunkEntrance.None;

        if (previousChunkData != null)
        {
            requiredEntrance = GetEntranceForExit(previousChunkData.exits);
        }

        List<GameObject> candidates = new List<GameObject>();

        foreach (GameObject prefab in chunkPrefabs)
        {
            if (prefab == null)
            {
                continue;
            }

            ChunkData chunkData = prefab.GetComponent<ChunkData>();

            if (chunkData == null)
            {
                candidates.Add(prefab);
                continue;
            }

            if (requiredEntrance != ChunkEntrance.None && !chunkData.HasEntrance(requiredEntrance))
            {
                continue;
            }

            candidates.Add(prefab);
        }

        if (candidates.Count == 0)
        {
            return chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    private ChunkEntrance GetEntranceForExit(ChunkExit exits)
    {
        List<ChunkEntrance> possibleEntrances = new List<ChunkEntrance>();

        if ((exits & ChunkExit.Left) != 0)
        {
            possibleEntrances.Add(ChunkEntrance.Left);
        }

        if ((exits & ChunkExit.Middle) != 0)
        {
            possibleEntrances.Add(ChunkEntrance.Middle);
        }

        if ((exits & ChunkExit.Right) != 0)
        {
            possibleEntrances.Add(ChunkEntrance.Right);
        }

        if (possibleEntrances.Count == 0)
        {
            return ChunkEntrance.Middle;
        }

        return possibleEntrances[Random.Range(0, possibleEntrances.Count)];
    }

    private float GetChunkLength(GameObject chunk)
    {
        if (chunk == null)
        {
            return chunkLength;
        }

        Renderer[] renderers = chunk.GetComponentsInChildren<Renderer>();
        if (renderers != null && renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds.size.z;
        }

        Collider[] colliders = chunk.GetComponentsInChildren<Collider>();
        if (colliders != null && colliders.Length > 0)
        {
            Bounds bounds = colliders[0].bounds;
            for (int i = 1; i < colliders.Length; i++)
            {
                bounds.Encapsulate(colliders[i].bounds);
            }

            return bounds.size.z;
        }

        return chunkLength;
    }

    private void RemoveOldChunks()
    {
        if (player == null)
        {
            return;
        }

        float cutoff = player.position.z - destroyDistance;

        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = activeChunks[i];

            if (chunk == null)
            {
                activeChunks.RemoveAt(i);
                continue;
            }

            if (chunk.transform.position.z < cutoff)
            {
                Destroy(chunk);
                activeChunks.RemoveAt(i);
            }
        }
    }
}
