using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IdentityManager : MonoBehaviour
{
    public GameObject menu;
    public TextMeshProUGUI pseudo;
    public TextMeshProUGUI age;
    public TextMeshProUGUI job;
    public TextMeshProUGUI vagabon;
    public TextMeshProUGUI action;
    public Slider energy;
    public TextMeshProUGUI energyvalue;

    void Start()
    {
        pseudo.text = "Pascal Ducnam";
        age.text = "16 ans";
        job.text = "Bûcheron";
        vagabon.text = "Vagabon : Non";
        action.text = "Action : Travail";
        energy.value = 50;
        energyvalue.text = energy.value.ToString();
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }
}
