using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTime : MonoBehaviour
{
    public static InGameTime Instance;

    public TextMeshProUGUI jour;
    public TextMeshProUGUI heure;
    int intjour;
    public int intheure = 480;
    int globaltime;
    float temps;
    public Button pause;
    public Button resume;
    public Button x2;
    public Button x3;
    public Button start;
    bool isPaused = true;
    public int workTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un InGameTime détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        jour.text = "Jour : 1";
        heure.text = "Heure : 00:00";
        intjour = 1;
        intheure = 400;
        globaltime = 0;
        temps = 0.5f;

        start.onClick.AddListener(() => isPaused = false);
        pause.onClick.AddListener(() => isPaused = true);
        resume.onClick.AddListener(() => isPaused = false);
        resume.onClick.AddListener(() => temps = 0.3f);
        x2.onClick.AddListener(() => isPaused = false);
        x2.onClick.AddListener(() => temps = 0.15f);
        x3.onClick.AddListener(() => isPaused = false);
        x3.onClick.AddListener(() => temps = 0.005f);
        
        StartCoroutine(TimeLoop());
    }

    IEnumerator TimeLoop()
    {
        while (true)
        {
            if (!isPaused)
            {
                intheure++;

                if (intheure == 480)
                {
                    VillagerManager.Instance.StartWork();
                }

                if (intheure == 1140)
                {
                    VillagerManager.Instance.StartNight();
                }

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
                // Peut être utiliser pour la fatigue, la reconversion, les ressources ou la construction de batiments
            }

            yield return new WaitForSeconds(temps);
        }
    }
    public int GetGlobalTime() { return globaltime; }
}
