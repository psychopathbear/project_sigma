using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { SmallCoin, BigCoin}
    public Tilemap tilemap;
    public GameObject[] objectsPrefabs;
    public float bigCoinProbability = 0.2f; // 20% chance of spawning a big coin
    public float enemyProbability = 0.1f; // 10% chance of spawning an enemy
    public int maxObjects = 5; // Maximum number of objects to spawn
    public float coinLifetime = 10f; // Time in seconds before the coin is destroyed
    public float spawnInterval = 0.5f; // Time in seconds between each spawn

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool isSpawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GatherValidPosition();
        StartCoroutine(SpawnObjectsIfNeeded());
        GameController.OnReset += LevelChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(!tilemap.gameObject.activeInHierarchy)
        {
            LevelChange();
        }
        if(!isSpawning && ActiveObjectsCount() < maxObjects)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }

    private void LevelChange()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPosition();
        DestroyAllSpawnedObjects();
    }

    private int ActiveObjectsCount()
    {
        spawnedObjects.RemoveAll(item => item == null);
        return spawnedObjects.Count;
    }

    private IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;
        while(ActiveObjectsCount() < maxObjects)
        {
            SpawnObject(); // Spawn an object
            yield return new WaitForSeconds(spawnInterval); // Wait for the spawn interval
        }
        isSpawning = false;
    }

    private bool PositionHasObject(Vector3 positionToCheck)
    {
        return spawnedObjects.Any(checkedObj => checkedObj && Vector3.Distance(checkedObj.transform.position, positionToCheck) < 1.0f); // Check if the distance between the checked object and the position to check is less than 1.0f
    }
    
    private ObjectType GetRandomObjectType()
    {
        float randomChoice = Random.value; // Get a random value between 0 and 1

        if(randomChoice <= (enemyProbability + bigCoinProbability))
        {
            return ObjectType.BigCoin;
        }
        else
        {
            return ObjectType.SmallCoin;
        }
    }

    private void SpawnObject()
    {
        if(validSpawnPositions.Count == 0) return; // No valid positions to spawn

        Vector3 spawnPosition = Vector3.zero; // Default spawn position
        bool validPositionFound = false; // Flag to check if a valid position was found

        while(!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count); // Get a random index from the list of valid positions
            Vector3 potentialPosition = validSpawnPositions[randomIndex]; // Get the position at the random index
            Vector3 leftPosition = potentialPosition + Vector3.left; // Get the position to the left of the potential position
            Vector3 rightPosition = potentialPosition + Vector3.right; // Get the position to the right of the potential position 

            if(!PositionHasObject(potentialPosition) && !PositionHasObject(leftPosition) && !PositionHasObject(rightPosition)) // Check if the potential position and the positions to the left and right don't have any objects
            {
                spawnPosition = potentialPosition; // Set the spawn position to the potential position
                validPositionFound = true; // Set the flag to true
            }
            else
            {
                validSpawnPositions.RemoveAt(randomIndex); // Remove the potential position from the list of valid positions
            }
        }

        if(validPositionFound)
        {
            ObjectType objectType = GetRandomObjectType(); // Get a random object type
            GameObject gameObject = Instantiate(objectsPrefabs[(int)objectType], spawnPosition, Quaternion.identity); // Instantiate the object at the spawn position
            spawnedObjects.Add(gameObject); // Add the object to the list of spawned objects

            StartCoroutine(DestroyObjectAfterTime(gameObject, coinLifetime)); // Start the coroutine to destroy the object after a certain time
        }
    }

    private IEnumerator DestroyObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified time

        if(gameObject)
        {
            spawnedObjects.Remove(gameObject); // Remove the object from the list of spawned objects
            validSpawnPositions.Add(gameObject.transform.position); // Add the position of the object to the list of valid positions
            Destroy(gameObject); // Destroy the object
        }
    }

    private void DestroyAllSpawnedObjects()
    {
        foreach(GameObject obj in spawnedObjects)
        {
            if(obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }

    private void GatherValidPosition()
    {
        validSpawnPositions.Clear();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Vector3 start = tilemap.CellToWorld(new Vector3Int(bounds.xMin, bounds.yMin, 0)); // Get the world position of the bottom-left corner of the tilemap

        for(int x = 0; x < bounds.size.x; x++) // Loop through all the tiles in the tilemap
        {
            for(int y = 0; y < bounds.size.y; y++) // Loop through all the tiles in the tilemap
            {
                TileBase tile = allTiles[x + y * bounds.size.x]; // Get the tile at the current position
                if(tile != null)
                {
                    Vector3 place = start + new Vector3(x + 1f, y + 2f, 0); // Add 0.5f to x and 2f to y to place the object in the center of the tile
                    validSpawnPositions.Add(place); // Add the position to the list of valid positions
                }
            }
        }
    }
}
