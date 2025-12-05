using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject buildManager;
    public GameObject resourcesManager;
    public GameObject residentsManager;
    public GameObject identityManager;
    public GameObject gameoverScreen;
    // Start is called before the first frame update
    void Start()
    {
        buildManager.SetActive(true);
        resourcesManager.SetActive(true);
        residentsManager.SetActive(true);
        identityManager.SetActive(true);
        gameoverScreen.SetActive(false);
    }

    public void GameOverVerification()
    {
        if (ResourceManager.Instance.residents <= 2)
        {
            buildManager.SetActive(false);
            resourcesManager.SetActive(false);
            residentsManager.SetActive(false);
            identityManager.SetActive(false);
            gameoverScreen.SetActive(true);
        }
    }
}
