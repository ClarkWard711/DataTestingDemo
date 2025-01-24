using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSorcererHolder : JobSkillHolder
{
	public ElementSorcererHolder Instance;
	public override void Awake()
	{
		base.Awake();
		if (Instance == null)
		{
			Instance = this;
		}
	}
}
