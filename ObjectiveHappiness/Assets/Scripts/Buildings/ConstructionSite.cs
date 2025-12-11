using UnityEngine;
using System;

public class ConstructionSite : MonoBehaviour
{
    public static Action<ConstructionSite> OnSiteCreated;
    public static Action<ConstructionSite> OnSiteFinished;

    public BuildingData buildingData;
    public GameObject finalPrefab;
    public float buildProgress = 0f;
    public float workPerTick = 5f;

    private void OnEnable()
    {
        // Émettre l'événement quand l'objet devient actif afin que
        // les abonnés (ex: les villageois) aient le temps de s'abonner.
        Debug.Log($"Construction site created for {buildingData?.buildingName ?? "UNKNOWN"}");
        OnSiteCreated?.Invoke(this);
    }

    // Garder Start() si tu as d'autres initialisations, sinon OnEnable suffit
    private void Start()
    {
        // rien d'obligatoire ici pour l'événement
    }

    public void WorkOnce()
    {
        buildProgress += workPerTick;
        // Clamp pour éviter overflow
        buildProgress = Mathf.Min(buildProgress, 100f);

        if (buildProgress >= 100f)
        {
            FinishConstruction();
        }
    }

    void FinishConstruction()
    {
        OnSiteFinished?.Invoke(this);

        if (finalPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * (buildingData?.placementHeightOffset ?? 0f);
            var instance = Instantiate(finalPrefab, spawnPos, transform.rotation);

            var parent = GameObject.Find("Buildings");
            if (parent != null) instance.transform.SetParent(parent.transform);
        }
        else
        {
            Debug.LogWarning("ConstructionSite: finalPrefab is null.");
        }

        Destroy(gameObject);
    }

}
