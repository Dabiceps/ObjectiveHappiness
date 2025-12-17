using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NuckCode : MonoBehaviour
{
    public Button nukebutton;

    private void Start()
    {
        nukebutton.onClick.AddListener(() => ExitGame());
    }

    private void ExitGame()
    {
        Debug.Log("CA CLIQUE ???");
        Application.Quit();
    }
}


