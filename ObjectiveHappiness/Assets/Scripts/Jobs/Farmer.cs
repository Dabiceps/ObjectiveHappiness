using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Villager
{
    private void Awake()
    {
        // Set the job of the villager and his target
        JobName = "Harvester";
        JobTarget = "Buisson";
        JobRoutine = StartCoroutine(WanderRoutine());

    }
    public override void StartJob()
    {
        base.StartJob();

    }

    public override void DoJob()
    {
        // Farmer recover food
        ResourceManager.Instance.ResourceRecovery("food");
    }

}
