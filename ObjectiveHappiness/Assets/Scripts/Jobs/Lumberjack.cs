using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberjack : Villager
{

    void Awake()
    {
        Debug.Log("Le bûcheron est prêt");
        JobName = "Bûcheron";
        JobTarget = "Arbre";
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
    }

}
