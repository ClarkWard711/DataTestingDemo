using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public EnemyData EnemyData;
	public GivingData givingData;
	public virtual void Awake()
	{
		givingData = gameObject.GetComponent<GivingData>();
	}
	public virtual void EnemyAction()
	{

	}
}
