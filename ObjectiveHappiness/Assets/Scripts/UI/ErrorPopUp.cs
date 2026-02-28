using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorPopUp : MonoBehaviour
{

    public static ErrorPopUp Instance;
    public GameObject popup;
    public TextMeshProUGUI errortext;


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

    public void DisplayPopUp(string text)
    {
        // To use the DisplayPopUp function, use DisplayPopUp("YourText")
        errortext.text = text;
        popup.gameObject.SetActive(true);
        StartCoroutine(HidePopUp());
    }

    private IEnumerator HidePopUp()
    {
        // Wait 3 sec before deleting the pop up for the player
        yield return new WaitForSeconds(3);
        popup.gameObject.SetActive(false);
    }

}
