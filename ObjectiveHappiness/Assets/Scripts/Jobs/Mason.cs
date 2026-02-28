using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mason : Villager
{
    private GameObject constructionSite;
    private List<ConstructionSite> assignedSites = new List<ConstructionSite>();

    private void Awake()
    {
        // override of the job target
        JobName = "Mason";
        JobTarget = "construction_site";
    }

    private void OnEnable()
    {
        ConstructionSite.OnSiteCreated += OnNewConstructionSite;
        ConstructionSite.OnSiteFinished += OnConstructionFinished;
    }

    private void OnDisable()
    {
        ConstructionSite.OnSiteCreated -= OnNewConstructionSite;
        ConstructionSite.OnSiteFinished -= OnConstructionFinished;
    }

    private void Start()
    {
        isWorking = false;
        if (JobRoutine == null) JobRoutine = StartCoroutine(WanderRoutine());
    }

    private void OnNewConstructionSite(ConstructionSite site)
    {
        if (site == null) return;

        // If mason is available, he directly go the construction site
        if (!isWorking && constructionSite == null)
        {
            constructionSite = site.gameObject;
            StartJob();
        }
        else
        {
            // Else, he go to the waiting list
            if (!assignedSites.Contains(site))
                assignedSites.Add(site);
        }
    }

    public override void StartJob()
    {
        if (isWorking) return;

        // If the villager doesn't have an assigned construction site, he find the closest
        if (constructionSite == null)
            constructionSite = FindNearestJobTarget();

        if (constructionSite == null) return;

        isWorking = true;

        if (JobRoutine != null) StopCoroutine(JobRoutine);

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(constructionSite.transform.position);
        }

        JobRoutine = StartCoroutine(WaitUntilArrived());
    }

    public override IEnumerator WaitUntilArrived()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent == null) yield break;

        yield return new WaitUntil(() => !agent.pathPending);

        yield return new WaitUntil(() =>
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        );

        agent.isStopped = true;
        agent.ResetPath();

        if (InGameTime.Instance.intheure >= 480 && InGameTime.Instance.intheure < 1140)
            JobRoutine = StartCoroutine(WorkLoop());
        else
            DoSleep();
    }

    public override void DoJob()
    {
        if (constructionSite == null) return;

        ConstructionSite site = constructionSite.GetComponent<ConstructionSite>();
        if (site == null) return;

        site.WorkOnce();
    }

    private GameObject FindNearestJobTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(JobTarget);
        if (targets.Length == 0) return null;

        GameObject nearestTarget = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

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

    public override void DoSleep()
    {
        base.DoSleep();
        isWorking = false;
        constructionSite = null;
    }

    private void OnConstructionFinished(ConstructionSite site)
    {
        if (site == null) return;

        // Delete of the list if present
        if (assignedSites.Contains(site))
            assignedSites.Remove(site);

        // If it was the present construction site, empty it
        if (constructionSite != null)
        {
            var s = constructionSite.GetComponent<ConstructionSite>();
            if (s == site)
                constructionSite = null;
        }

        isWorking = false;

        TryStartNextSite();
    }

    private void TryStartNextSite()
    {
        if (isWorking) return;

        // If there is nothing waiting go as a vagabond
        if (assignedSites.Count == 0)
        {
            if (JobRoutine != null) StopCoroutine(JobRoutine);
            JobRoutine = StartCoroutine(WanderRoutine());
            return;
        }

        // Take the next construction site in waiting file
        constructionSite = assignedSites[0].gameObject;
        assignedSites.RemoveAt(0);

        StartJob();
    }
}
