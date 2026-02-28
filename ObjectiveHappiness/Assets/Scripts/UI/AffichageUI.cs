using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class AffichageUI : MonoBehaviour
{
    public TextMeshProUGUI woodDisplay, stoneDisplay, foodDisplay, residentDisplay, prosperityDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ressources to 0
        woodDisplay.text = "Bois : 0";
        stoneDisplay.text = "Pierre : 0";
        foodDisplay.text = "Nourriture : 0";
        residentDisplay.text = "Habitants : 0";
    }

    public void SetResource(string wood, string stone, string food, string residents)
    {
        // Set UI ressources to the calculated ressources in game
        woodDisplay.text = "Bois : " + wood;
        stoneDisplay.text = "Pierre : " + stone;
        foodDisplay.text = "Nourriture : " + food;
        residentDisplay.text = "Habitants : " + residents;

    }
}
