using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public AffichageUI UI;

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
            prosperity -= 3;
            GenocideVerification();
        }
        else
        {
            prosperity += residents*0.2f;
            residents *= 2;
        }
    }

    // Méthode qui calcul le nombre de ressources récupéré en fonction du type (food/wood/stone)
    public void ResourceRecovery(string type)
    {
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

    // Méthode qui verifie si la partie est finie par un game over
    void GenocideVerification()
    {
        if (residents <= 2)
        {
            Console.WriteLine("Génocide -> Game Over");
        }
    }
}
