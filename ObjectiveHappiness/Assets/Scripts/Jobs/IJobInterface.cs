using UnityEngine;

public interface IJobInterface
{
    string JobName { get; }
    string Name { get; set; }
    Coroutine JobRoutine { get; set; }

    // Appelé au début de la journée pour assigner le travail
    void StartJob();

    // Appelé chaque frame ou à intervalle régulier
    void DoJob();

    // Appelé en fin de journée si nécessaire
    void EndJob();

    void DoSleep();
}
