using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberjack : Villager
{

    void Awake()
    {
        // Set the job of the villager and his target
        JobName = "B�cheron";
        JobTarget = "Arbre";
        JobRoutine = StartCoroutine(WanderRoutine());
    }

    public override void StartJob()
    {
        base.StartJob();
    }

    public override void DoJob()
    {
        // Lumberjack recover wood
        ResourceManager.Instance.ResourceRecovery("wood");
    }

}
