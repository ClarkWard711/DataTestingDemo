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
	public int seed;
	public int stepsToEncounter;
	//位置
	public Vector3 playerPosition;
	//翻转
	public Quaternion playerRotation;
	//武器饰品等数值
	//经验 等级等数据
	//敌人宝箱状态和位置
	//天赋树状态
	public Dictionary<int, JobStatsData> jobStatsState = new Dictionary<int, JobStatsData>();
	//武器？
	public Dictionary<int, int> positionID = new Dictionary<int, int>();
}
public class JobStatsData
{
	public int level;
	public int currentHp;
	public int currentSp;
	public int currentExp;
	public int specialID;
	public List<int> skillsID = new List<int>();
	public List<bool> skillUnlockState;
}
