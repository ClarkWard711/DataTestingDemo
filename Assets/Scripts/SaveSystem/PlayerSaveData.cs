using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
	public class jobStatsData
	{
		int level;
		int currentHp;
		int currentSp;
		int currentExp;
	}
	public PartyMember playerParty;
	//解锁职业
	public Dictionary<int, bool> jobUnlockState = new Dictionary<int, bool>();
	//职业解锁的技能
	public Dictionary<int, List<bool>> skillUnlockState = new Dictionary<int, List<bool>>();
	//是否是新的一局
	public bool isNewGame = true;
	//地图数据
	//经验 等级等数据
	public Dictionary<int, jobStatsData> jobStatsState = new Dictionary<int, jobStatsData>();
	//武器？
}
