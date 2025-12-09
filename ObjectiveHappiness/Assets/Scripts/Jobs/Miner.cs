using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Villager
{

    void Awake()
    {
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
        ResourceManager.Instance.ResourceRecovery("stone");
    }
}
