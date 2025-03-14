using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/EnemyData"), fileName = ("EnemyData_"))]

public class EnemyData : ScriptableObject
{
    

    [SerializeField] string enemyName;
    
    [SerializeField, Range(1, enemyMaxLevel)] int jobInitialLevel = 1;

    public int currentHP;

    [SerializeField] int enemyLevel;

    const int enemyMaxLevel = 15;
    
    public string EnemyName => enemyName;

    public int EnemyLevel
    {
        get => enemyLevel;
        set
        {
            if (enemyLevel == value || value is < 1 or > enemyMaxLevel) return;

            enemyLevel = value;
        }
    }

    public GameObject JobPrefab;


    [SerializeField] TextAsset dataFile;

    [SerializeField] List<EnemyStats> enemyStatsList;

    public List<EnemyStats> EnemyStatsList => enemyStatsList;

    void OnValidate()
    {
        if (!dataFile) return;

        enemyStatsList.Clear();

        string[] textInLines = dataFile.text.Split('\n');

        for (int LineIndex = 1; LineIndex < textInLines.Length - 1; LineIndex++) 
        {
            string[] statsValues = textInLines[LineIndex].Split(",");

            EnemyStats currentLevelStats = new EnemyStats();

            currentLevelStats.maxHP = int.Parse(statsValues[0]);
            currentLevelStats.physicalAtk = int.Parse(statsValues[2]);
            currentLevelStats.soulAtk = int.Parse(statsValues[3]);
            currentLevelStats.physicalDfs = int.Parse(statsValues[4]);
            currentLevelStats.soulDfs = int.Parse(statsValues[5]);
            currentLevelStats.hit = int.Parse(statsValues[6]);
            currentLevelStats.nim = int.Parse(statsValues[7]);
            currentLevelStats.speed = int.Parse(statsValues[8]);
            currentLevelStats.AntiCri = int.Parse(statsValues[9]);
            currentLevelStats.melee = int.Parse(statsValues[10]);
            currentLevelStats.remote = int.Parse(statsValues[11]);
            currentLevelStats.exp = int.Parse(statsValues[12]);
            currentLevelStats.sxp = int.Parse(statsValues[13]);
            currentLevelStats.dizzinessBar = int.Parse(statsValues[14]);
            currentLevelStats.PhysicalAntiDizziness = int.Parse(statsValues[15]);
            currentLevelStats.SoulAntiDizziness = int.Parse(statsValues[16]);

            enemyStatsList.Add(currentLevelStats);
        }

    }

    private void OnEnable()
    {
        enemyLevel = jobInitialLevel;
        currentHP = enemyStatsList[jobInitialLevel - 1].maxHP;
    }
}
