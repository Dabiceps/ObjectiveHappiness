using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Villager
{

    void Awake()
    {
        // Set the job of the villager and his target
        JobName = "Mineur";
        JobTarget = "Rocher";
        JobRoutine = StartCoroutine(WanderRoutine());
    }
    public override void StartJob()
    {
        base.StartJob();

    }
    public override void DoJob()
    {
        // Miner recover stone
        ResourceManager.Instance.ResourceRecovery("stone");
    }
}
