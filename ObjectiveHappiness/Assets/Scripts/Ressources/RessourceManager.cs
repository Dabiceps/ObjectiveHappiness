using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public AffichageUI UI;
    public GameOverUI gameOverUI;

    public int wood;
    public int stone;
    public int food;
    public int residents;
    public float prosperity;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un RessourceManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Update()
    {
        UI.SetResource(wood.ToString(), stone.ToString(), food.ToString(), residents.ToString());
    }

    // Actions effectuées en fin de journée
    // Modifie : Food / Resident / Prosperity
    public void EndOfDay()
    {
        food -= residents * 20;
        if (food < 0)
        {
            food = 0;
            residents -= residents/3;
        }
        else { residents += 2; }
        ProsperityModifiers();
        gameOverUI.GameOverVerification();
        gameOverUI.WinVerification();
    }

    // Méthode qui calcul le nombre de ressources récupéré en fonction du type (food/wood/stone)
    public void ResourceRecovery(string type)
    {
        // Cas de  base
        if (type != "food" ||type != "wood" || type != "stone")
        {
            Console.WriteLine("Erreur : mauvais type de ressource");
        }
        //Calcul la nourriture récupérée en fonction du nombre de ferme
        if (type == "food")
        {
            food = food + 1* CountBuilding("Ferme");
        }
        if (type == "wood")
        {
            wood++;
        }
        else
        {
            stone++;
        }
    }

    // Modification de la prospérité en fonction des éléments présents dans le jeu
    public void ProsperityModifiers()
    {
        int nbrlibrairie = CountBuilding("Librairie");
        int nbrmusee = CountBuilding("Musee");
        if (food < 0)
        {
            prosperity -= prosperity*0.05f;
        }
        else
        {
            prosperity += residents * (0.2f + nbrlibrairie*0.1f + nbrmusee*0.2f);
        }
        
        // Baisse la prospérité de 0.3 pour chaques villageois ayant 0 d'énergie à la fin de la journée
        foreach (Transform villager in VillagerManager.Instance.villagers)
        {
            if (GetVillagerEnergy(villager) == 0)
            {
                prosperity -= 0.3f;
            }
        }
        if (prosperity < 0)
        {
            prosperity = 0;
        }
    }

    public int CountBuilding(string tag)
    {
        int count = 0;
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag(tag))
            {
                count++;
            }
        }
        return count;
    }

    public int GetVillagerEnergy(Transform villager)
    {
        IJobInterface jobInterface = villager.GetComponent<IJobInterface>();
        return jobInterface.Energy;
    }
}
