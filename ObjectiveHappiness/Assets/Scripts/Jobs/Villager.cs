using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, IJobInterface
{
    private Coroutine jobRoutine;
    Lumberjack lumberjack;
    public string JobName { get; set; }
    public string JobTarget { get; set; }

    public virtual void DoJob()
    {
        //LOGIQUE DE LECOLE
        this.gameObject.SetActive(false);
    }

    public virtual void EndJob()
    {
        this.gameObject.SetActive(true);
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag("Maison"))
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    Debug.Log("Le " + JobName + " se dirige vers la maison");
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

    public virtual void StartJob()
    {
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag(JobTarget))
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    Debug.Log("Le " + JobName +" se dirige vers le " + JobTarget);
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

    public virtual IEnumerator WaitUntilArrived()
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
    public virtual void DoSleep()
    {
        Debug.Log("Le villageois dort");
    }

    public virtual void TransformIntoLumberjack()
    {
        this.gameObject.AddComponent<Lumberjack>();
        this.gameObject.GetComponent<Villager>().enabled = false;
    }
    void Awake()
    {
        JobName = "Villageois";
        JobTarget = "Ecole";
    }
}
