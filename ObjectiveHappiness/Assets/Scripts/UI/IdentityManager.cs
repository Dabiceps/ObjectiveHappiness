using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IdentityManager : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI pseudo;
    public TextMeshProUGUI job;
    public TextMeshProUGUI age;
    public TextMeshProUGUI vagabon;
    public TextMeshProUGUI action;
    public Slider energy;
    public TextMeshProUGUI energyvalue;

    void Start()
    {

    }

    public void OpenMenu()
    {
        menu.SetActive(true);
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
}
