using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour, IJobInterface
{
    
    public string JobTarget { get; set; }
    public Coroutine JobRoutine { get; set; }

    public string JobName { get; set; }
    public string Pseudo { get; set; }
    public int Age { get; set; }
    public bool Vagabond { get; set; }
    public string actionText { get; set; }
    public int Energy { get; set; }

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
                    Animator animator = GetComponent<Animator>();
                    agent.SetDestination(building.position);
                    animator.SetBool("isWalking", true);

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
        Animator animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        if (InGameTime.Instance.intheure >= 480 && InGameTime.Instance.intheure < 1140)
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
            Energy--;
            yield return new WaitForSeconds(InGameTime.Instance.workTime); // rythme de travail
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
            Animator animator = GetComponent<Animator>();
            // On cherche une position aléatoire valide sur la NavMesh
            if (RandomPointOnNavMesh(transform.position, 15f, out randomPoint))
            {
                agent.SetDestination(randomPoint);
                
                animator.SetBool("isWalking", true);
                // Attendre d'arriver à la destination
                yield return new WaitUntil(() =>
                    !agent.pathPending &&
                    agent.remainingDistance <= agent.stoppingDistance &&
                    (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                );
            }
            animator.SetBool("isWalking", false);
            // Petite pause avant de choisir un autre point
            yield return new WaitForSeconds(3f);
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
        JobTarget = "Ecole";
        JobRoutine = StartCoroutine(WanderRoutine());
    }

    public void InitializeIdentity(string pseudo, string jobname, int age, bool vagabon, string action, int energy)
    {
        Pseudo = pseudo;
        Age = age;
        Vagabond = vagabon;
        actionText = action;
        Energy = energy;
        JobName = jobname;
    }
}
