using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IdentityManager : MonoBehaviour
{
    public static IdentityManager Instance;

    public GameObject menu;
    public TextMeshProUGUI pseudo, job, age, vagabon, action, energyvalue;
    public Slider energy;
    public Button RecoMason, RecoLumberjack, RecoMiner, RecoFarmer;

    private bool isOpen = false;
    private IJobInterface lastVillager;
    private GameObject currentVillager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un IdentityManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Convert villager into a new job
        RecoMason.onClick.AddListener(() => VillagerManager.Instance.ConvertInto(currentVillager, VillagerManager.JobType.Mason));
        RecoLumberjack.onClick.AddListener(() => VillagerManager.Instance.ConvertInto(currentVillager, VillagerManager.JobType.Lumberjack));
        RecoMiner.onClick.AddListener(() => VillagerManager.Instance.ConvertInto(currentVillager, VillagerManager.JobType.Miner));
        RecoFarmer.onClick.AddListener(() => VillagerManager.Instance.ConvertInto(currentVillager, VillagerManager.JobType.Farmer));
    }

    public void OpenMenu(IJobInterface villager)
    {
        menu.SetActive(true);
        lastVillager = villager;
        currentVillager = ((MonoBehaviour)villager).gameObject;
        IdentitePerso(villager.Pseudo, villager.JobName, villager.Age, villager.Vagabond, villager.actionText, villager.Energy);
        isOpen = true;
    }

    public void UpdateEnergy()
    {
        if (!isOpen) return;
        energy.value = lastVillager.Energy;
        energyvalue.text = lastVillager.Energy.ToString();
        action.text = "Action : " + lastVillager.actionText;
    }

    public void UpdateAction(string actualaction)
    {
        if (!isOpen) return;
        action.text = actualaction;
    }


    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void IdentitePerso(string IDpseudo, string IDjob, int IDage, bool IDvagabon, string IDaction, int IDenergie)
    {
        pseudo.text = IDpseudo;
        job.text = IDjob;
        age.text = IDage.ToString() + " ans";
        vagabon.text = "Vagabond : " + IDvagabon.ToString();
        action.text = IDaction;
        energy.value = IDenergie;
        energyvalue.text = IDenergie.ToString();
    }
}
