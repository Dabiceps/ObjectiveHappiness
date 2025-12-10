using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.VFX;

public class VillagerManager : MonoBehaviour
{
    [Header("Villager Prefabs")]
    [SerializeField] private GameObject villager;
    [SerializeField] private GameObject lumberjack;
    [SerializeField] private GameObject miner;
    [SerializeField] private GameObject farmer;
    [SerializeField] private GameObject mason;


    public static VillagerManager Instance;
    public List<Transform> villagers = new List<Transform>();
    GameObject parent;

    List<string> pseudoslist = new List<string> {"Gontrand", "Godefroy", "Enguerrand", "Perceval", "Lothaire", "Sigisbert", "Sigebert", "Thiébault", "Théobald", "Rainier",
                                        "Raimbaud", "Reinold", "Arsoude", "Hardouin", "Aubry", "Amaury", "Hildebert", "Clodomir", "Clotaire", "Childéric",
                                        "Dagobert", "Arnaud", "Evrard", "Fulbert", "Wulfric",  "Warin", "Aleran", "Gautier", "Anseau", "Béranger",
                                        "Brévalin", "Tancrède", "Isambart", "Odilon", "Landric", "Rodolphe", "Emmeran", "Isambard", "Eudes", "Broceliand", "Hippolyte "};

    public enum JobType
    {
        Villager,
        Lumberjack,
        Miner,
        Farmer,
        Mason
    }

    private Dictionary<JobType, GameObject> jobPrefabs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un VillagerManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        jobPrefabs = new Dictionary<JobType, GameObject>
    {
        { JobType.Villager, villager },
        { JobType.Lumberjack, lumberjack },
        { JobType.Miner, miner },
        { JobType.Farmer, farmer },
        { JobType.Mason, mason }
    };

    }

    private void Start()
    {
        parent = GameObject.Find("Villagers");
    }
    public void StartGame()
    {


        SpawnRandom(JobType.Villager, "Villageois");
        SpawnRandom(JobType.Miner, "Mineur");
        SpawnRandom(JobType.Lumberjack, "Bûcheron");
        SpawnRandom(JobType.Farmer, "Récolteur");
        SpawnRandom(JobType.Mason, "Maçon");
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

    public GameObject SpawnPNJ(JobType type, string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        var prefab = jobPrefabs[type];

        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instance.transform.SetParent(parent.transform, worldPositionStays: true);

        var jobInterface = instance.GetComponent<IJobInterface>();
        jobInterface.InitializeIdentity(pseudo, jobname, age, vagabon, action, energy);

        return instance.gameObject;
    }

    void SpawnRandom(JobType type, string jobname)
    {
        SpawnPNJ(type, pseudoslist[Random.Range(0, pseudoslist.Count)], jobname, Random.Range(1, 20), false, "idle", 100);
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

    public void ConvertInto(GameObject prevJob, JobType newJob)
    {
        IJobInterface villager = prevJob.GetComponent<IJobInterface>();
        SpawnPNJ(newJob, villager.Pseudo, newJob.ToString(), villager.Age, villager.Vagabond, "idle", villager.Energy);
        Destroy(prevJob);
    }




}
