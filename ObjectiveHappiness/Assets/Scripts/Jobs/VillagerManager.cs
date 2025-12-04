using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    [Header("Villager Settings")]
    public GameObject prefab;


    public static VillagerManager Instance;
    private List<Transform> villagers = new List<Transform>();
    GameObject parent;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un VillagerManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        parent = GameObject.Find("Villagers");
        SpawnVillager();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in parent.transform)
        {
            if (child != null && !villagers.Contains(child))
            {
                villagers.Add(child);
            }
        }
    }

    void EndWork()
    {
        foreach (Transform villager in villagers)
        {
            IJobInterface jobInterface = villager.GetComponent<IJobInterface>();
            if (jobInterface != null)
            {
                jobInterface.EndJob();
            }
        }
    }

    void StartWork()
    {
        foreach (Transform villager in villagers)
        {
            IJobInterface jobInterface = villager.GetComponent<IJobInterface>();
            if (jobInterface != null)
            {
                jobInterface.StartJob();
            }
        }
    }

    void SpawnVillager()
    {
        // Implementation for spawning a villager
        var instance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
    }

}
