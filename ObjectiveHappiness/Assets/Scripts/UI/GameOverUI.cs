using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject buildManager;
    public GameObject buildMenu;
    public GameObject resourcesManager;
    public GameObject identityManager;
    public GameObject gameoverScreen;
    public GameObject victoryScreen;
    // Start is called before the first frame update
    void Start()
    {
        buildManager.SetActive(true);
        buildMenu.SetActive(false);
        resourcesManager.SetActive(true);
        gameoverScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void GameOverVerification()
    {
        if (ResourceManager.Instance.residents <= 2)
        {
            buildManager.SetActive(false);
            resourcesManager.SetActive(false);
            gameoverScreen.SetActive(true);
            Exitgame(33f);
        }
    }

    public void WinVerification()
    {
        if ( ResourceManager.Instance.prosperity >= 100)
        {
            buildManager.SetActive(false);
            resourcesManager.SetActive(false);
            victoryScreen.SetActive(true);
            Exitgame(60f);
        }
    }

    IEnumerator Exitgame(float secondes)
    {
        yield return new WaitForSeconds(secondes);
        Application.Quit();
    }
}
