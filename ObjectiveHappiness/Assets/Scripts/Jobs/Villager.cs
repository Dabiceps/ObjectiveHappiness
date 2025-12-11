using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
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

    public bool isWorking = false;

    public virtual void DoJob() { }

    // Remis en virtual pour que les sous-classes puissent override proprement
    public virtual void EndJob()
    {
        Debug.Log($"{Pseudo} finit sa journée.");

        // Stopper toute coroutine active
        if (JobRoutine != null)
        {
            StopCoroutine(JobRoutine);
            JobRoutine = null;
        }

        // Stopper les animations & déplacements (avec null-checks)
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

        // Le villageois N’EST PLUS en train de travailler
        isWorking = false;

        if (!Vagabond)
        {
            foreach (Transform transform in GameObject.Find("Buildings").transform)
            {
                if (transform != null && transform.CompareTag("Maison"))
                {
                    Building building1 = transform.GetComponent<Building>();
                    if (building1 != null)
                    {
                        if (agent != null)
                        {
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
        // Si vagabond ou pas de maison trouvée, on part vagabonder
        JobRoutine = StartCoroutine(WanderRoutine());
    }


    public virtual void StartJob()
    {
        // Si une routine existe, on l'arrête (sécurité)
        if (JobRoutine != null) StopCoroutine(JobRoutine);

        var buildingsParent = GameObject.Find("Buildings");
        if (buildingsParent == null)
        {
            // pas de bâtiments, on vagabonde
            isWorking = false;
            JobRoutine = StartCoroutine(WanderRoutine());
            return;
        }

        // Si pas de JobTarget défini, on vagabonde aussi
        if (string.IsNullOrEmpty(JobTarget))
        {
            isWorking = false;
            JobRoutine = StartCoroutine(WanderRoutine());
            return;
        }

        // Parcours des bâtiments pour trouver une cible libre
        foreach (Transform building in buildingsParent.transform)
        {
            if (building != null && building.CompareTag(JobTarget))
            {
                Building building1 = building.GetComponent<Building>();
                if (building1 != null && !building1.isUsed)
                {
                    NavMeshAgent agent = GetComponent<NavMeshAgent>();
                    Animator animator = GetComponent<Animator>();
                    if (agent != null)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(building.position);
                        animator?.SetBool("isWalking", true);

                        building1.isUsed = true;

                        // Indiquer qu'on est en train de partir travailler
                        isWorking = true;

                        if (JobRoutine != null) StopCoroutine(JobRoutine);
                        JobRoutine = StartCoroutine(WaitUntilArrived());
                        return;
                    }
                }
            }
        }

        // si on n'a rien trouvé, on vagabonde
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

        // IMPORTANT : on stocke la coroutine de travail dans JobRoutine
        if (InGameTime.Instance != null &&
            InGameTime.Instance.intheure >= 480 &&
            InGameTime.Instance.intheure < 1140)
        {
            // JobRoutine est remplacée par la WorkLoop coroutine
            JobRoutine = StartCoroutine(WorkLoop());
        }
        else
        {
            // fin de journée / nuit
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
                IdentityManager.Instance.UpdateAction("Travail");
                if (Vagabond == false)
                {
                    Energy--;
                    IdentityManager.Instance.UpdateEnergy();
                }
            }
            yield return new WaitForSeconds(InGameTime.Instance.workTime); // rythme de travail
        }

        // Fin de journée ou pause : on remet le flag et on part vagabonder
        isWorking = false;

        if (JobRoutine != null)
        {
            // On ne stoppe pas explicitement la WorkLoop ici (on sort naturellement),
            // mais on remplace JobRoutine pour que les StopCoroutine ailleurs fonctionne correctement.
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
        // Par défaut on ne force pas un new Wander si on est déjà en JobRoutine null,
        // mais on peut explicitement lancer le vagabondage si aucune coroutine n'est active.
        if (JobRoutine == null)
            JobRoutine = StartCoroutine(WanderRoutine());
        Energy = 100;
        IdentityManager.Instance.UpdateEnergy();
        actionText = "Dort";
        IdentityManager.Instance.UpdateAction("Dort");
    }

    public void Vagabonder()
    {
        if (JobRoutine != null) StopCoroutine(JobRoutine);
        JobRoutine = StartCoroutine(WanderRoutine());
    }

    public IEnumerator WanderRoutine()
    {
        Debug.Log($"{Pseudo} vagabonde");
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();

        while (true)
        {
            Vector3 randomPoint;
            if (RandomPointOnNavMesh(transform.position, 15f, out randomPoint))
            {
                if (agent != null)
                {
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

    // Awake : initialisation par défaut. Les classes dérivées peuvent redéfinir Awake si nécessaire.
    void Awake()
    {
        JobTarget = "Ecole";
        // On démarre en vagabondage
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
                break; // On a trouvé une école, pas besoin de continuer la boucle
            }
        }

        if (school == null)
        {
            // Aucune école trouvée
            ErrorPopUp.Instance.DisplayPopUp("Aucune école n'a été construite dans le village !");
            return;
        }

        // Si on a trouvé une école, on continue
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
