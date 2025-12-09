using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mason : Villager
{
    private bool isWorking = false;
    private GameObject constructionSite;
    private void Awake()
    {
        JobName = "Mason";
        JobTarget = "construction_site";
    }

    private void OnEnable()
    {
        ConstructionSite.OnSiteCreated += OnNewConstructionSite;
    }

    private void OnDisable()
    {
        ConstructionSite.OnSiteCreated -= OnNewConstructionSite;
    }

    private void OnNewConstructionSite(ConstructionSite site)
    {
        if (!isWorking)
        {
            StartJob();
        }
    }

    public override void StartJob()
    {
        if (isWorking) return;

        constructionSite = FindNearestJobTarget();
        if (constructionSite == null) return;

        isWorking = true;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(constructionSite.transform.position);

        StartCoroutine(WaitUntilArrived());
    }


    public override IEnumerator WaitUntilArrived()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            yield return new WaitUntil(() => !agent.pathPending);

            yield return new WaitUntil(() =>
                agent.remainingDistance <= agent.stoppingDistance &&
                (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            );
            if (GameManager.Instance.currentDayState == GameManager.DayState.Work)
                StartCoroutine(WorkLoop());
            else
                DoSleep();
            yield return null;
        }

    public override IEnumerator WorkLoop()
    {
        while (GameManager.Instance.currentDayState == GameManager.DayState.Work)
        {
            DoJob();
            yield return new WaitForSeconds(1f); // rythme de travail
        }

        isWorking = false;
    }

    public override void DoJob()
    {
        if (constructionSite == null) return;

        ConstructionSite site = constructionSite.GetComponent<ConstructionSite>();
        if (site == null) return;

        site.WorkOnce();

        // Animations éventuelles…
        // Réduire l’énergie du villageois…
    }

    private GameObject FindNearestJobTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(JobTarget);
        GameObject nearestTarget = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        if (targets.Length == 0)
        {
            return null;
        }
        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(currentPosition, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = target;
            }
        }
        return nearestTarget;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
