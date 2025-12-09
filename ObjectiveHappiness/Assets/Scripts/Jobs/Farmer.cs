using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Villager
{
    private void Awake()
    {
        JobName = "Farmer";
        JobTarget = "Buisson";

    }
    public override void StartJob()
    {
        base.StartJob();

    }

    public override void DoJob()
    {
        while (GameManager.Instance.currentDayState == GameManager.DayState.Work)
        {
            ResourceManager.Instance.ResourceRecovery("food");
        }
    }

}
