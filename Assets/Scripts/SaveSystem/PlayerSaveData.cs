using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{

	public PartyMember playerParty;
	//解锁职业
	public Dictionary<int, bool> jobUnlockState = new Dictionary<int, bool>();
	//职业解锁的技能
	public Dictionary<int, List<bool>> skillUnlockState = new Dictionary<int, List<bool>>();
	//是否是新的一局
	public bool isNewGame = true;
	//地图数据
	//武器饰品等数值
	//经验 等级等数据
	//敌人宝箱状态和位置
	//天赋树状态
	public Dictionary<int, JobStatsData> jobStatsState = new Dictionary<int, JobStatsData>();
	//武器？
}
public class JobStatsData
{
	int level;
	int currentHp;
	int currentSp;
	int currentExp;
	int specialID;
	List<int> skillsID;
	List<bool> skillUnlockState;
}
