using System.Collections;
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

    public bool isWorking = false;

    public virtual void DoJob() { }

    // Reverted to virtual so that subclasses can override properly
    public virtual void EndJob()
    {
        isWorking = false;

        // Stop all active coroutine
        if (JobRoutine != null)
        {
            StopCoroutine(JobRoutine);
            JobRoutine = null;
        }

        // Stop animations & movement (with null-checks)
        var agent = GetComponent<NavMeshAgent>();
        var anim = GetComponent<Animator>();

        if (agent != null)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }
        if (anim != null)
        {
            anim.SetBool("isWalking", false);
        }

        // The villager is no longer working
        isWorking = false;
        if (!Vagabond)
        {
            foreach (Transform transform in GameObject.Find("Buildings").transform)
            {
                if (transform != null && transform.CompareTag("Maison"))
                {
                    Building building1 = transform.GetComponent<Building>();
                    if (building1 != null && !building1.isUsed)
                    {
                        if (agent != null)
                        {
                            Debug.Log("test6");
                            agent.isStopped = false;
                            agent.SetDestination(transform.position);
                            anim?.SetBool("isWalking", true);
                            actionText = "Rentre à la maison";
                            building1.isUsed = true;
                            if (JobRoutine != null) StopCoroutine(JobRoutine);
                            JobRoutine = StartCoroutine(WaitUntilArrived());
                            return;
                        }
                    }
                }
            }
        }
        // Whether you're a vagabond or haven't found a home, you go wandering.
        JobRoutine = StartCoroutine(WanderRoutine());
    }


    public virtual void StartJob()
    {
        // If a routine exists, we stop it (security)
        if (JobRoutine != null) StopCoroutine(JobRoutine);

        var buildingsParent = GameObject.Find("Buildings");
        if (buildingsParent == null)
        {
            // No buildings, we wander.
            isWorking = false;
            JobRoutine = StartCoroutine(WanderRoutine());
            return;
        }

        // If no JobTarget is defined, we also wander.
        if (string.IsNullOrEmpty(JobTarget))
        {
            isWorking = false;
            JobRoutine = StartCoroutine(WanderRoutine());
            return;
        }

        Transform closestBuilding = null;
        float closestDistance = Mathf.Infinity;

        Vector3 agentPos = transform.position;

        // Navigate the buildings to find the nearest free target
        foreach (Transform building in buildingsParent.transform)
        {
            if (building != null && building.CompareTag(JobTarget))
            {
                Building building1 = building.GetComponent<Building>();
                if (building1 != null && !building1.isUsed)
                {
                    float distance = Vector3.Distance(agentPos, building.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestBuilding = building;
                    }
                }
            }
        }

        // If we have found a target
        if (closestBuilding != null)
        {
            Building building1 = closestBuilding.GetComponent<Building>();
            if (building1 != null)
            {
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                Animator animator = GetComponent<Animator>();

                if (agent != null)
                {
                    agent.isStopped = false;
                    agent.SetDestination(closestBuilding.position);
                    animator?.SetBool("isWalking", true);

                    building1.isUsed = true;
                    isWorking = true;

                    if (JobRoutine != null) StopCoroutine(JobRoutine);
                    JobRoutine = StartCoroutine(WaitUntilArrived());
                }
            }
        }

    // If we haven't found anything, we wander.
    isWorking = false;
        JobRoutine = StartCoroutine(WanderRoutine());
    }

    public virtual IEnumerator WaitUntilArrived()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            yield break;
        }

        yield return new WaitUntil(() => !agent.pathPending);

        yield return new WaitUntil(() =>
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        );

        Animator animator = GetComponent<Animator>();
        animator?.SetBool("isWalking", false);

        // IMPORTANT: The working coroutine is stored in JobRoutine.
        if (InGameTime.Instance != null &&
            InGameTime.Instance.intheure >= 480 &&
            InGameTime.Instance.intheure < 1140)
        {
            // JobRoutine is replaced by the WorkLoop coroutine
            JobRoutine = StartCoroutine(WorkLoop());
        }
        else
        {
            Debug.Log("DORS");
            DoSleep();
        }

        yield return null;
    }

    public virtual IEnumerator WorkLoop()
    {
        Animator animator = GetComponent<Animator>();
        animator?.SetBool("isWalking", false);

        while (InGameTime.Instance != null &&
               InGameTime.Instance.intheure >= 480 &&
               InGameTime.Instance.intheure < 1140)
        {
            if (Energy > 0)
            {
                DoJob();
                actionText = "Travail";
                if (Vagabond == false)
                {
                    Energy--;
                    IdentityManager.Instance.UpdateEnergy();
                }
            }
            yield return new WaitForSeconds(InGameTime.Instance.workTime); // pace of work
        }

        // End of the day or break: we put the flag back up and go wandering
        isWorking = false;

        if (JobRoutine != null)
        {
            // We don't explicitly stop the WorkLoop here (we exit naturally),
            // but we replace the JobRoutine so that StopCoroutines elsewhere work correctly.
            JobRoutine = StartCoroutine(WanderRoutine());
        }
        else
        {
            JobRoutine = StartCoroutine(WanderRoutine());
        }
    }

    public virtual void DoSleep()
    {
        Debug.Log($"{Pseudo} dort");
        // By default, a new Wander is not forced if the JobRoutine is already null,
        // but wandering can be explicitly initiated if no coroutine is active.
        if (JobRoutine == null)
            JobRoutine = StartCoroutine(WanderRoutine());
        Energy = 100;
        IdentityManager.Instance.UpdateEnergy();
        actionText = "Dort";
    }

    public void Vagabonder()
    {
        if (JobRoutine != null) StopCoroutine(JobRoutine);
        JobRoutine = StartCoroutine(WanderRoutine());
    }

    public IEnumerator WanderRoutine()
    {
        actionText = "Vagabonde";
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();

        while (true)
        {
            Vector3 randomPoint;
            if (RandomPointOnNavMesh(transform.position, 15f, out randomPoint))
            {
                if (agent != null)
                {
                    agent.isStopped = false;
                    agent.SetDestination(randomPoint);
                    animator?.SetBool("isWalking", true);

                    yield return new WaitUntil(() =>
                        !agent.pathPending &&
                        agent.remainingDistance <= agent.stoppingDistance &&
                        (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    );
                }
            }
            animator?.SetBool("isWalking", false);
            yield return new WaitForSeconds(3f);
        }
    }

    private bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
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

    // Awake: default initialization. Derived classes can override Awake if necessary.
    void Awake()
    {
        JobTarget = "Ecole";
        // We start by wandering
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

    public void GoToSchool(GameObject prevJob, VillagerManager.JobType type)
    {
        Debug.Log($"{Pseudo} va à l'école pour se reconvertir.");

        Transform school = null;

        foreach (Transform building in GameObject.Find("Buildings").transform)
        {
            if (building != null && building.CompareTag("Ecole"))
            {
                school = building;
                break; // We found a school, no need to continue the loop
            }
        }

        if (school == null)
        {
            // No schools found
            ErrorPopUp.Instance.DisplayPopUp("Aucune école n'a été construite dans le village !");
            return;
        }

        // If we've found a school, we continue
        Building buildingComponent = school.GetComponent<Building>();
        if (buildingComponent == null)
            return;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();
        if (agent != null)
        {
            Debug.Log($"{Pseudo} se rend à l'école pour se reconvertir.");
            agent.ResetPath();
            agent.isStopped = true;
            StopAllCoroutines();
            agent.SetDestination(school.position);
            agent.isStopped = false;
            animator?.SetBool("isWalking", true);
            actionText = "Se reconvertit";

            buildingComponent.isUsed = true;
            isWorking = true;

            if (JobRoutine != null) StopCoroutine(JobRoutine);
            JobRoutine = StartCoroutine(WaitUntilArrivedSchool(prevJob, type));
        }
    }


    public IEnumerator WaitUntilArrivedSchool(GameObject prevJob, VillagerManager.JobType type)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            yield break;
        }
        yield return new WaitUntil(() => !agent.pathPending);
        yield return new WaitUntil(() =>
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        );
        Animator animator = GetComponent<Animator>();
        animator?.SetBool("isWalking", false);
        isWorking = false;
        JobRoutine = null;
        VillagerManager.Instance.DoConvert(prevJob, type);
        yield return null;
    }


}
