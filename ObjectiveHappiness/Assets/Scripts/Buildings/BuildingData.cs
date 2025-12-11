using UnityEngine;

[CreateAssetMenu(menuName = "CityBuilder/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;
    public GameObject ghostPrefab;
    public GameObject[] buildingVariants;   // variantes finales possibles
    public float placementRadius;

    [Header("Placement offset")]
    public float placementHeightOffset = 0f; // pour corriger les variantes


    public int costWood;
    public int costStone;
}
