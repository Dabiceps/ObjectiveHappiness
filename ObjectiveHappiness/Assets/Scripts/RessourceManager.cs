using System;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public AffichageUI UI;
    public GameOverUI gameOverUI;

    public int wood, stone, food, residents;
    public float prosperity;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d�un RessourceManager d�tect� dans la sc�ne ! Un a �t� supprim�.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Update()
    {
        // Set the ressource each tick 
        UI.SetResource(wood.ToString(), stone.ToString(), food.ToString(), residents.ToString());
    }

    // Actions effectu�es en fin de journ�e
    // Modifie : Food / Resident / Prosperity
    public void EndOfDay()
    {
        food -= residents * 20;
        if (food < 0)
        {
            food = 0;
            int nbrkills = residents/3;
            residents -= residents/3;
            foreach (Transform villager in VillagerManager.Instance.villagers.ToList())
            {
                if (nbrkills != 0)
                {
                    VillagerManager.Instance.villagers.Remove(villager);
                    VillagerManager.Instance.KillVillager(villager);
                    nbrkills--;
                }
            }


        }
        else 
        {
            residents += 2;
            VillagerManager.Instance.SpawnRandom(VillagerManager.JobType.Villager, "Villageois");
            VillagerManager.Instance.SpawnRandom(VillagerManager.JobType.Villager, "Villageois");
        }
        ProsperityModifiers();
        gameOverUI.GameOverVerification();
        gameOverUI.WinVerification();
    }

    // M�thode qui calcul le nombre de ressources r�cup�r� en fonction du type (food/wood/stone)
    public void ResourceRecovery(string type)
    {
        //Calcul la nourriture r�cup�r�e en fonction du nombre de ferme
        if (type == "food")
        {
            int multi = 0;
            if(CountBuilding("Ferme") == 0) { multi = 1; }
            else { multi = CountBuilding("Ferme"); }

            food = food + 1 * multi;
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

    // Modification de la prosp�rit� en fonction des �l�ments pr�sents dans le jeu
    public void ProsperityModifiers()
    {
        int nbrlibrairie = CountBuilding("Librairie");
        int nbrmusee = CountBuilding("Musee");
        if (food == 0)
        {
            prosperity -= prosperity*0.05f;
        }
        else
        {
            prosperity += residents * (0.2f + nbrlibrairie*0.1f + nbrmusee*0.2f);
        }
        
        // Baisse la prosp�rit� de 0.3 pour chaques villageois ayant 0 d'�nergie � la fin de la journ�e
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
        // Count the total of building in the game
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
        // Return the energy of the villager to use it in other code
        IJobInterface jobInterface = villager.GetComponent<IJobInterface>();
        return jobInterface.Energy;
    }
}
