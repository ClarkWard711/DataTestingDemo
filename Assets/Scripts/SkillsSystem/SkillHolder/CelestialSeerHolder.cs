using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialSeerHolder : JobSkillHolder
{
    public static CelestialSeerHolder Instance;


    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        var tag = JadeFall.CreateInstance<JadeFall>();
        gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            player.GetComponent<GivingData>().AddTagToCharacter(JadeFall.CreateInstance<JadeFall >());
        }
    }

}
