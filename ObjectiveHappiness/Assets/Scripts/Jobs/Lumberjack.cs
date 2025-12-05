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
        Debug.Log("Le bûcheron commence son travail");
    }

    public override void DoJob()
    {
        Debug.Log("Le bûcheron coupe du bois");
    }

}
