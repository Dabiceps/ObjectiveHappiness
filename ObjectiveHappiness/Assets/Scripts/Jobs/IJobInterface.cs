using UnityEngine;

public interface IJobInterface
{
    string JobName { get; }
    string Pseudo { get; set; }
    int Age { get; set; }
    bool Vagabond { get; set; }
    string actionText { get; set; }
    int Energy { get; set; }
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
