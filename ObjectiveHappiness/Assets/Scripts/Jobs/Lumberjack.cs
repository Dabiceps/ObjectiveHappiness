using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberjack : Villager
{

    void Awake()
    {
        JobName = "Bûcheron";
        JobTarget = "Arbre";
        JobRoutine = StartCoroutine(WanderRoutine());
    }
    void Update()
    {
        
    }

    public override void StartJob()
    {
        base.StartJob();
    }

    public override void DoJob()
    {
        ResourceManager.Instance.ResourceRecovery("wood");
    }

}
