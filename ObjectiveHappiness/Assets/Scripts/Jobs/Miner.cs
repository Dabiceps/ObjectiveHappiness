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
        Debug.Log("Le mineur commence son travail");
    }
    public override void DoJob()
    {
        Debug.Log("Le mineur extrait du minerai");
    }
}
