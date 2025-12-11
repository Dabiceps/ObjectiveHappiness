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
            Debug.LogWarning("Plus d’un RessourceManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void DisplayPopUp(string text)
    {
        errortext.text = text;
        popup.gameObject.SetActive(true);
        StartCoroutine(HidePopUp());
    }

    private IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(3);
        popup.gameObject.SetActive(false);
    }

}
