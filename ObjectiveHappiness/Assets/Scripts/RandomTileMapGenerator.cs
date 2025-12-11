using Unity.AI.Navigation;
using UnityEngine;

public class RandomTileMapGenerator : MonoBehaviour
{
    [Header("Tiles (4 prefabs)")]
    public GameObject[] tiles; // 4 tiles (5x5)

    [Header("Map Settings")]
    public int mapSizeInTiles = 4; // ex: 4 => map finale = 4 × 4 tiles

    public NavMeshSurface navMeshSurface;

    void Start()
    {
        GenerateMap();
        navMeshSurface.BuildNavMesh();
        NatureSpawner.Instance.SpawnAll();
    }

    void GenerateMap()
    {
        if (tiles.Length != 4)
        {
            Debug.LogError("Tu dois mettre exactement 4 tiles dans le tableau !");
            return;
        }

        int totalTiles = mapSizeInTiles * mapSizeInTiles;
        int countTileA = totalTiles / 2; // 50%
        int countOthers = totalTiles - countTileA;

        // Construire une liste contenant les tiles dans les bonnes proportions
        System.Collections.Generic.List<GameObject> tilePool = new System.Collections.Generic.List<GameObject>();

        // Tile 0 = la tile qui représente les 50%
        for (int i = 0; i < countTileA; i++)
            tilePool.Add(tiles[0]);

        // Les 3 autres tiles
        for (int i = 0; i < countOthers; i++)
        {
            int randomTile = Random.Range(1, 4); // index 1,2,3
            tilePool.Add(tiles[randomTile]);
        }

        // Mélange aléatoire
        for (int i = 0; i < tilePool.Count; i++)
        {
            var tmp = tilePool[i];
            int r = Random.Range(i, tilePool.Count);
            tilePool[i] = tilePool[r];
            tilePool[r] = tmp;
        }

        // Placement en grille carrée
        int index = 0;
        float tileSize = 10f; // Chaque tile fait 5x5

        for (int x = 0; x < mapSizeInTiles; x++)
        {
            for (int y = 0; y < mapSizeInTiles; y++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, y * tileSize);
                Instantiate(tilePool[index], pos, Quaternion.identity, transform);
                index++;
            }
        }
    }
}
