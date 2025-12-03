using UnityEngine;

[CreateAssetMenu(menuName = "CityBuilder/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GameObject prefab;
    public GameObject ghostPrefab;

    public int costWood;
    public int costStone;
}
