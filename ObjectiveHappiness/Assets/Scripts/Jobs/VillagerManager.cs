using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VillagerManager : MonoBehaviour
{
    [Header("Villager Prefabs")]
    public GameObject villager;
    public GameObject lumberjack;
    public GameObject miner;
    public GameObject farmer;
    public GameObject mason;


    public static VillagerManager Instance;
    public List<Transform> villagers = new List<Transform>();
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
        SpawnVillager("Villageois", "Villageois", 18, false, "idle", 100);
        SpawnMiner("Villageois", "Villageois", 18, false, "idle", 100);
        SpawnLumberjack("Villageois", "Villageois", 18, false, "idle", 100);
        SpawnFarmer("Villageois", "Villageois", 18, false, "idle", 100);
        SpawnMason("Villageois", "Villageois", 18, false, "idle", 100);
        CheckList();
        
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
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            Building building1 = building.GetComponent<Building>();
            building1.isUsed = false;
        }
    }

    public GameObject SpawnVillager(string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        var instance = Instantiate(villager, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        IJobInterface jobInterface = instance.GetComponent<IJobInterface>();
        jobInterface.InitializeIdentity(pseudo, jobname, age, vagabon, action, energy);
        return instance.gameObject;
    }

    public GameObject SpawnLumberjack(string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        var instance = Instantiate(lumberjack, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        IJobInterface jobInterface = instance.GetComponent<IJobInterface>();
        jobInterface.InitializeIdentity(pseudo, jobname, age, vagabon, action, energy);
        return instance.gameObject;
    }

    public GameObject SpawnMiner(string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        var instance = Instantiate(miner, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        IJobInterface jobInterface = instance.GetComponent<IJobInterface>();
        jobInterface.InitializeIdentity(pseudo, jobname, age, vagabon, action, energy);
        return instance.gameObject;
    }

    public GameObject SpawnFarmer(string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        var instance = Instantiate(farmer, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        IJobInterface jobInterface = instance.GetComponent<IJobInterface>();
        jobInterface.InitializeIdentity(pseudo, jobname, age, vagabon, action, energy);
        return instance.gameObject;
    }

    public GameObject SpawnMason(string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        var instance = Instantiate(mason, new Vector3(0, 0, 0), Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);
        IJobInterface jobInterface = instance.GetComponent<IJobInterface>();
        jobInterface.InitializeIdentity(pseudo, jobname, age, vagabon, action, energy);
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
