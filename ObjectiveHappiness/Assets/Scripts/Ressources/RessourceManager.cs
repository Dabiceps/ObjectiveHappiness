using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
        food -= residents * 10;
        if (food < 0)
        {
            food = 0;
            residents /= 3;
        }
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
            food = food + 1;  //+ (1* nbrferme);
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
        float bonus = 0.2f; // bonus = 0.2f + (NbrLibraire*0.1) + (NbrMusee*0.2)
        if (food < 0)
        {
            prosperity -= prosperity*0.05f;
        }
        else
        {
            prosperity += (residents * bonus);
        }

    }
}
