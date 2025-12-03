public interface IJobInterface
{
    string JobName { get; }

    // Appelé au début de la journée pour assigner le travail
    void StartJob();

    // Appelé chaque frame ou à intervalle régulier
    void DoJob();

    // Appelé en fin de journée si nécessaire
    void EndJob();
}
