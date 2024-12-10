using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public EnemyData EnemyData;
	public GivingData givingData;
	public void Awake()
	{
		givingData = gameObject.GetComponent<GivingData>();
	}
	public virtual void EnemyAction()
	{

	}
}
