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
        Debug.Log("Le fermier commence son travail");
    }

    public override void DoJob()
    {
        Debug.Log("Le fermier récolte des cultures");
    }

}
