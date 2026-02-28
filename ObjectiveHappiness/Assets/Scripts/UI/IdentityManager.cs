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
            Debug.LogWarning("Plus d�un IdentityManager d�tect� dans la sc�ne ! Un a �t� supprim�.");
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
        // Open the menu on the left if you click on a villager
        // Display information of each villager
        menu.SetActive(true);
        lastVillager = villager;
        currentVillager = ((MonoBehaviour)villager).gameObject;
        IdentitePerso(villager.Pseudo, villager.JobName, villager.Age, villager.Vagabond, villager.actionText, villager.Energy);
        isOpen = true;
    }

    public void UpdateEnergy()
    {
        // Update the energy of the villager in the UI
        if (!isOpen) return;
        energy.value = lastVillager.Energy;
        energyvalue.text = lastVillager.Energy.ToString();
        action.text = lastVillager.actionText;
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void IdentitePerso(string IDpseudo, string IDjob, int IDage, bool IDvagabon, string IDaction, int IDenergie)
    {
        // Identity display for each villager 
        pseudo.text = IDpseudo;
        job.text = IDjob;
        age.text = IDage.ToString() + " ans";
        vagabon.text = "Vagabond : " + IDvagabon.ToString();
        action.text = IDaction;
        energy.value = IDenergie;
        energyvalue.text = IDenergie.ToString();
    }
}
