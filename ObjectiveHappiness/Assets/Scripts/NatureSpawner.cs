using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObject
{
    public string name;                  // juste pour repérer dans l'Inspector
    public GameObject[] variants;        // variantes possibles
    public int countPerTile = 3;         // combien de fois on veut spawn par tile
}

public enum TileType
{
    Buisson,
    Rocher,
    Foret,
    Autre
}

public class NatureSpawner : MonoBehaviour
{

    public static NatureSpawner Instance;

    [Header("Tiles parent")]
    public Transform tilesParent; // parent contenant toutes les tiles générées

    [Header("Parent pour les objets générés")]
    public Transform natureParent; // parent sous lequel les buissons/arbres/rochers seront instanciés

    [Header("Spawnable Objects")]
    public SpawnableObject bushes;
    public SpawnableObject rocks;
    public SpawnableObject trees;

    [Header("Placement settings")]
    public float offsetMin = 1f; // distance minimale du bord
    public float offsetMax = 4f; // distance maximale depuis le bord

    void Start()
    {
        if (natureParent == null)
        {
            GameObject go = new GameObject("Nature");
            natureParent = go.transform;
        }

    }

    private void Awake()
    { 
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un NatureSpawner détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SpawnAll()
    {
        if (tilesParent == null)
        {
            Debug.LogError("NatureSpawner: tilesParent est vide !");
            return;
        }

        foreach (Transform tile in tilesParent)
        {
            TileType type = TileTypeFromName(tile.name);

            switch (type)
            {
                case TileType.Buisson:
                    SpawnObjectsOnTile(tile, bushes);
                    break;
                case TileType.Rocher:
                    SpawnObjectsOnTile(tile, rocks);
                    break;
                case TileType.Foret:
                    SpawnObjectsOnTile(tile, trees);
                    break;
            }
        }
    }

    TileType TileTypeFromName(string name)
    {
        name = name.ToLower();
        if (name.Contains("buisson")) return TileType.Buisson;
        if (name.Contains("rocher")) return TileType.Rocher;
        if (name.Contains("foret")) return TileType.Foret;
        return TileType.Autre;
    }

    void SpawnObjectsOnTile(Transform tile, SpawnableObject spawnable)
    {
        if (spawnable.variants == null || spawnable.variants.Length == 0) return;

        float tileSize = 10f; // doit correspondre à la taille des tiles
        float yOffset = 1.5f; // valeur par défaut

        // si ce sont les arbres, offset = 7
        if (spawnable == trees) yOffset = 7f;

        for (int i = 0; i < spawnable.countPerTile; i++)
        {
            // position aléatoire à l'intérieur de la tile
            float x = Random.Range(offsetMin, tileSize - offsetMax);
            float z = Random.Range(offsetMin, tileSize - offsetMax);

            Vector3 spawnPos = tile.position + new Vector3(x, yOffset, z);

            // choisir variante aléatoire
            int r = Random.Range(0, spawnable.variants.Length);
            GameObject obj = spawnable.variants[r];

            // instanciation sous le parent global
            Instantiate(obj, spawnPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), natureParent);
        }
    }


}
