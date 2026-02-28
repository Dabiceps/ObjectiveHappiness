using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTime : MonoBehaviour
{
    public static InGameTime Instance;

    public TextMeshProUGUI jour, heure;
    int intjour, globaltime;
    public int intheure = 480;
    float temps;
    public Button pause, resume, x2, x3, start;
    bool isPaused = true;
    public float workTime = 10f;
    public Image day, night;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d�un InGameTime d�tect� dans la sc�ne ! Un a �t� supprim�.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Display informations in the top menu
        jour.text = "Jour : 1";
        heure.text = "Heure : 00:00";
        intjour = 1;
        intheure = 470;
        globaltime = 0;
        temps = 0.5f;
        workTime = temps * 5;

        // Addlistener to time button to use them when we want
        start.onClick.AddListener(() => StartGame());
        pause.onClick.AddListener(() => OnPauseClick());
        resume.onClick.AddListener(() => OnResumeClick());
        x2.onClick.AddListener(() => OnX2Click());
        x3.onClick.AddListener(() => OnX3Click());
    }

    public void StartGame()
    {
        // Initialise the time in game
        isPaused = false;
        StartCoroutine(TimeLoop());
        VillagerManager.Instance.StartGame();
    }

    void OnPauseClick()
    {
        // Pause the game and set villager speed to 0
        isPaused = true;
        SetVillagerSpeed(0f);
    }
    void OnResumeClick()
    {
        // Resume game
        isPaused = false;
        temps = 0.3f;
        workTime = temps * 5;
        SetVillagerSpeed(1f);
    }
    void OnX2Click()
    {
        // Make the game 2 times faster
        isPaused = false;
        temps = 0.15f;
        workTime = temps * 5;
        SetVillagerSpeed(10f);
    }

    void OnX3Click()
    {
        // Make the game 2 times faster
        isPaused = false;
        temps = 0.005f;
        workTime = temps * 5;
        SetVillagerSpeed(20f);
    }

    IEnumerator TimeLoop()
    {
        // Create the ingame time and make actions links to the time
        while (true)
        {
            if (!isPaused)
            {
                intheure++;

                // At 8:00 villagers start their jobs
                if (intheure == 480)
                {
                    VillagerManager.Instance.StartWork();
                    day.gameObject.SetActive(true);
                    night.gameObject.SetActive(false);
                }

                // At 19:00 They go to sleep
                if (intheure == 1140)
                {
                    VillagerManager.Instance.StartNight();
                    day.gameObject.SetActive(false);
                    night.gameObject.SetActive(true); 
                }

                // At 23:59 time go to 00:00 and the ressources are calculated
                if (intheure >= 1440)
                {
                    intjour++;
                    intheure = 0;
                    ResourceManager.Instance.EndOfDay();
                }

                jour.text = "Jour : " + intjour;
                heure.text = "Heure : " + (intheure / 60).ToString("00") + ":" + (intheure % 60).ToString("00");
                globaltime++;
                // Pour utiliser globaltime il faut initialiser un temps de base puis soustraire le global time actuel par ce temps de base 
                // basetime = globaltime
                // if (globaltime - basetime == 60) {action}
                // (Effectue une action au bout de 1 heures ingame)
                // Peut �tre utiliser pour la fatigue, la reconversion, les ressources ou la construction de batiments
            }

            yield return new WaitForSeconds(temps);
        }
    }
    public int GetGlobalTime() { return globaltime; }

    void SetVillagerSpeed(float speed)
    {
        // villager speed change with the time (pause, resume, x2, x3)
        foreach (Transform villager in VillagerManager.Instance.villagers)
        {
            villager.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed;
        }
    }


}
