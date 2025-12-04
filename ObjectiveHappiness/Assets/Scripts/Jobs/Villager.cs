using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, IJobInterface
{
    private GameObject buildings;

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
        foreach (Transform building in buildings.transform)
        {
            if (building != null && building.tag == "Ecole")
            {

                Debug.Log("Le villageois commence à aller à la ferme");
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                agent.SetDestination(building.position);
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildings = BuildingManager.Instance.parent;
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
