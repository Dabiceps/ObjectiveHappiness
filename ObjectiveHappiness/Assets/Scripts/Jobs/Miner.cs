using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Villager
{
    void Awake()
    {
        Debug.Log("Le bûcheron est prêt");
        JobName = "Mineur";
        JobTarget = "Rocher";
    }
    public override void StartJob()
    {
        base.StartJob();

    }
    public override void DoJob()
    {
    }
}
