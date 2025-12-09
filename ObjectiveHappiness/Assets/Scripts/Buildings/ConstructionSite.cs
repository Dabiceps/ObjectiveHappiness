using UnityEngine;
using System;

public class ConstructionSite : MonoBehaviour
{
    public static Action<ConstructionSite> OnSiteCreated;

    public BuildingData buildingData;
    public float buildProgress = 0f;
    public float workPerTick = 5f;

    private void Start()
    {
        Debug.Log($"Construction site created for {buildingData.buildingName}");
        OnSiteCreated?.Invoke(this);
    }

    public void WorkOnce()
    {
        buildProgress += workPerTick;

        if (buildProgress >= 100f)
        {
            FinishConstruction();
        }
    }

    void FinishConstruction()
    {
        Instantiate(buildingData.prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
