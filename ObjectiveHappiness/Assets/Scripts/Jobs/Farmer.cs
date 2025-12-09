using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Villager
{
    private void Awake()
    {
        JobName = "Farmer";
        JobTarget = "Buisson";
        JobRoutine = StartCoroutine(WanderRoutine());

    }
    public override void StartJob()
    {
        base.StartJob();

    }

    public override void DoJob()
    {
        ResourceManager.Instance.ResourceRecovery("food");
    }

}
