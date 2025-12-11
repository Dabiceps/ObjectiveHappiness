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
    public TextMeshProUGUI pseudo;
    public TextMeshProUGUI job;
    public TextMeshProUGUI age;
    public TextMeshProUGUI vagabon;
    public TextMeshProUGUI action;
    public Slider energy;
    public TextMeshProUGUI energyvalue;
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
        action.text = "Action : " + IDaction;
        energy.value = IDenergie;
        energyvalue.text = IDenergie.ToString();
    }

    public void Reconversion()
    {
        // TODO
    }

    // VillagerManager.Instance.ConvertInto(currentVillager, VillagerManager.JobType.Mason);

}
