using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTime : MonoBehaviour
{
    public TextMeshProUGUI jour;
    public TextMeshProUGUI heure;
    int intjour;
    int intheure;
    int globaltime;
    float temps;
    public Button pause;
    public Button resume;
    public Button x2;
    public Button x3;
    bool isPaused = false;

    void Start()
    {
        jour.text = "Jour : 0";
        heure.text = "Heure : 00:00";
        intjour = 0;
        intheure = 0;
        globaltime = 0;
        temps = 0.5f;

        pause.onClick.AddListener(() => isPaused = true);
        resume.onClick.AddListener(() => isPaused = false);
        resume.onClick.AddListener(() => temps = 0.3f);
        x2.onClick.AddListener(() => isPaused = false);
        x2.onClick.AddListener(() => temps = 0.15f);
        x3.onClick.AddListener(() => isPaused = false);
        x3.onClick.AddListener(() => temps = 0.10f);
        
        StartCoroutine(TimeLoop());
    }

    IEnumerator TimeLoop()
    {
        while (true)
        {
            if (!isPaused)
            {
                intheure++;

                if (intheure >= 1440)
                {
                    intjour++;
                    intheure = 0;
                }

                jour.text = "Jour : " + intjour;
                heure.text = "Heure : " + (intheure / 60).ToString("00") + ":" + (intheure % 60).ToString("00");
                globaltime++;
            }

            yield return new WaitForSeconds(temps);
        }
    }
}
