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
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.tag == "Ecole")
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    Debug.Log("Le villageois se dirige vers l'école");
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.SetDestination(building.position);
                    building1.isUsed = true;
                    return;
                }
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

        
    }

}
