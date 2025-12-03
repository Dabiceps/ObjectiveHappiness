using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour, IJobInterface
{
    public string JobName => "Villageois";

    public void DoJob()
    {
        Debug.Log("Le villageois travaille.");
        
    }

    public void EndJob()
    {
        Debug.Log("Le villageois a terminé son travail.");
    }

    public void StartJob()
    {
        Debug.Log("Le villageois commence son travail.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       GameManager.DayState state = GameManager.Instance.currentDayState;
        switch (state)
        {
            case GameManager.DayState.Work:
                DoJob();
                break;
            case GameManager.DayState.Night:
                EndJob();
                break;
        }
    }

}
