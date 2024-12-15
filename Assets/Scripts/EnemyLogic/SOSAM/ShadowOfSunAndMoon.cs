using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowOfSunAndMoon : Enemy
{
	public enum SOSAMState { Tsuki, Hi, Final }
	public Sprite TsukiMode;
	public Sprite HiMode;
	public SOSAMState BossState = SOSAMState.Tsuki;

	public override void Awake()
	{
		base.Awake();
		SOSAMTurnStartCheck tag = SOSAMTurnStartCheck.CreateInstance<SOSAMTurnStartCheck>();
		tag.unit = this.gameObject;
	}
	public override void EnemyAction()
	{
		if (givingData.currentHP <= givingData.maxHP * 0.8f && givingData.currentHP > givingData.maxHP * 0.6f)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = HiMode;
			BossState = SOSAMState.Hi;
		}
		else if (givingData.currentHP <= givingData.maxHP * 0.6f && givingData.currentHP > givingData.maxHP * 0.4f)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = TsukiMode;
			BossState = SOSAMState.Tsuki;
		}
		else if (givingData.currentHP <= givingData.maxHP * 0.4f && givingData.currentHP > givingData.maxHP * 0.2f)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = HiMode;
			BossState = SOSAMState.Hi;
		}
	}
}
