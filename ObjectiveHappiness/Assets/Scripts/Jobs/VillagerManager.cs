using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VillagerManager : MonoBehaviour
{
    [Header("Villager Settings")]
    public GameObject villager;
    public GameObject lumberjack;
    public GameObject miner;


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
        SpawnMiner();
        SpawnLumberjack();
        CheckList();
        
    }

    public IEnumerator JobCoroutine()
    {
        yield return new WaitForSeconds(10f);
        StartWork();
    }

    // Update is called once per frame
    void Update()
    {
        CheckList();
    }

    public void StartNight()
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

    public void StartWork()
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

    public GameObject SpawnVillager()
    {
        var instance = Instantiate(villager, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        return instance.gameObject;
    }

    public GameObject SpawnLumberjack()
    {
        var instance = Instantiate(lumberjack, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        return instance.gameObject;
    }

    public GameObject SpawnMiner()
    {
        var instance = Instantiate(miner, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        return instance.gameObject;
    }

    void CheckList()
    {
        foreach (Transform child in parent.transform)
        {
            if (child != null && !villagers.Contains(child))
            {
                villagers.Add(child);
            }
        }
    }

}
