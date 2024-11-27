using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinNoNameHolder : JobSkillHolder
{
	public AssassinNoNameHolder Instance;
	public override void Awake()
	{
		base.Awake();
		if (Instance == null)
		{
			Instance = this;
		}
	}
	public override void AddSkillToButton()
	{
		//base.AddSkillToButton();
	}
}
