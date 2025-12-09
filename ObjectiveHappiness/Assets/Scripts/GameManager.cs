using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int DayDurationInSeconds = 0;
    public int NightDurationInSeconds = 0;

    private int currentDay = 1;
    public DayState currentDayState = DayState.Work;
    void Start()
    {
        StartGame();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un GameManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum DayState
    {
        Work,
        Night,
    }
    void StartGame()
    {
        Debug.Log("Debut du jeu");
        //StartCoroutine(DayCoroutine());
        
    }

    public IEnumerator DayCoroutine()
    {
        Debug.Log("Début du jour " + currentDay);
        currentDayState = DayState.Work;
        VillagerManager.Instance.StartWork();
        int FixTime = DayDurationInSeconds;
        while (FixTime > 0)
        {
            yield return new WaitForSeconds(1f);
            FixTime--;
            
        }
        currentDayState = DayState.Night;
        Debug.Log("Début de la nuit " + currentDay);
        VillagerManager.Instance.StartNight();
        int FixNightTime = NightDurationInSeconds;
        while (FixNightTime > 0)
        {
            yield return new WaitForSeconds(1f);
            FixNightTime--;
        }
        
        currentDay++;
        StartCoroutine(DayCoroutine());
        yield return null;
    }


}
