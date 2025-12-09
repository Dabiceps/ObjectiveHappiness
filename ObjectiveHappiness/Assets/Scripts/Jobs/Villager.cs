using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, IJobInterface
{
    public string JobName { get; set; }
    public string JobTarget { get; set; }
    public Coroutine JobRoutine { get; set; }
    public string Name { get; set; }

    public virtual void DoJob()
    {

    }

    public virtual void EndJob()
    {
        Debug.Log("Job ended");
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag("Maison"))
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.SetDestination(building.position);

                    building1.isUsed = true;

                    if (JobRoutine != null) StopCoroutine(JobRoutine);
                    JobRoutine = StartCoroutine(WaitUntilArrived());

                    return;
                }
            }
        }
        Vagabonder();
    }

    public virtual void StartJob()
    {
        Debug.Log("Job started");
        if (JobRoutine != null) StopCoroutine(JobRoutine);
        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag(JobTarget))
            {
                Building building1 = building.GetComponent<Building>();
                if (!building1.isUsed)
                {
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    agent.SetDestination(building.position);

                    building1.isUsed = true;

                    if (JobRoutine != null) StopCoroutine(JobRoutine);
                    JobRoutine = StartCoroutine(WaitUntilArrived());

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
        if(InGameTime.Instance.intheure >= 480 && InGameTime.Instance.intheure < 1140)
            StartCoroutine(WorkLoop());
        else
            DoSleep();
        yield return null;
    }

    public virtual IEnumerator WorkLoop()
    {
        while (InGameTime.Instance.intheure >= 480 && InGameTime.Instance.intheure < 1140)
        {
            DoJob();
            yield return new WaitForSeconds(1f); // rythme de travail
        }
    }

    public virtual void DoSleep()
    {
        Debug.Log("Le villageois dort");
    }

    public void Vagabonder()
    {
        // On stoppe toute autre routine de déplacement
        if (JobRoutine != null) StopCoroutine(JobRoutine);

        JobRoutine = StartCoroutine(WanderRoutine());
    }

    public IEnumerator WanderRoutine()
    {
        Debug.Log("Le villageois vagabonde");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        while (true)
        {
            Vector3 randomPoint;

            // On cherche une position aléatoire valide sur la NavMesh
            if (RandomPointOnNavMesh(transform.position, 15f, out randomPoint))
            {
                agent.SetDestination(randomPoint);

                // Attendre d'arriver à la destination
                yield return new WaitUntil(() =>
                    !agent.pathPending &&
                    agent.remainingDistance <= agent.stoppingDistance &&
                    (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                );
            }

            // Petite pause avant de choisir un autre point
            yield return new WaitForSeconds(1f);
        } 
    }

    /// Génère un point aléatoire sur la NavMesh.
    private bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++) // plusieurs essais si nécessaire
        {
            Vector3 randomPos = center + Random.insideUnitSphere * range;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = center;
        return false;
    }




    void Awake()
    {
        JobName = "Villageois";
        JobTarget = "Ecole";
    }
}
