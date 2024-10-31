using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodistHolder : JobSkillHolder
{
    public static BloodistHolder Instance;

    public int BloodAddictEnemy = 0, BloodAddictSelf = 0;

    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {

        }
    }
}
