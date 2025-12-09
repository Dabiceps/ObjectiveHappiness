using UnityEngine;

public interface IJobInterface
{
    string JobName { get; }
    string pseudo { get; set; }
    int age { get; set; }
    bool vagabond { get; set; }
    string actionText { get; set; }
    int energy { get; set; }
    Coroutine JobRoutine { get; set; }

    void InitializeIdentity(string pseudo, string jobname, int age, bool vagabon, string action, int energy);

    // Appelé au début de la journée pour assigner le travail
    void StartJob();

    // Appelé chaque frame ou à intervalle régulier
    void DoJob();

    // Appelé en fin de journée si nécessaire
    void EndJob();

    void DoSleep();
}
