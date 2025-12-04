using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, IJobInterface
{
    private Coroutine jobRoutine;

    public string JobName => "Villageois";

    public void DoJob()
    {
        //LOGIQUE DE LECOLE
        this.gameObject.SetActive(false);
    }

    public void EndJob()
    {
        this.gameObject.SetActive(true);
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag("Maison"))
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    Debug.Log("Le villageois se dirige vers la maison");
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.SetDestination(building.position);

                    building1.isUsed = true;

                    if (jobRoutine != null) StopCoroutine(jobRoutine);
                    jobRoutine = StartCoroutine(WaitUntilArrived());

                    return;
                }
            }
        }
    }

    public void StartJob()
    {
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag("Ecole"))
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    Debug.Log("Le villageois se dirige vers l'école");
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.SetDestination(building.position);

                    building1.isUsed = true;

                    if (jobRoutine != null) StopCoroutine(jobRoutine);
                    jobRoutine = StartCoroutine(WaitUntilArrived());

                    return;
                }
            }
        }
    }

    private IEnumerator WaitUntilArrived()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        yield return new WaitUntil(() => !agent.pathPending);

        yield return new WaitUntil(() =>
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        );
        if(GameManager.Instance.currentDayState == GameManager.DayState.Work)
            DoJob();
        else
            DoSleep();
    }
    public void DoSleep()
    {
        Debug.Log("Le villageois dort");
    }
}
