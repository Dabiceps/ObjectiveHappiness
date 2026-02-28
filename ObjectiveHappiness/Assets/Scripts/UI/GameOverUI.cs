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
        // Not display some UI menus
        buildManager.SetActive(true);
        buildMenu.SetActive(false);
        resourcesManager.SetActive(true);
        gameoverScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    public void GameOverVerification()
    {
        // Verify if the player is in a gameover situation
        // Game Over when there is less than 2 villagers
        // Display the gameover video
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
        // Verify if the player is in a victory situation
        // Victory when prosperity reach 100 or highter
        // Display the victory video
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
        // Button to quit game
        yield return new WaitForSeconds(secondes);
        Application.Quit();
    }
}
