using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

namespace Data
{
    [CreateAssetMenu(menuName = ("Data/JobData"), fileName = ("JobData_"))]

    public class JobData : ScriptableObject
    {
        [SerializeField] Sprite jobAvatarImage;

        [SerializeField] string jobName;

        [SerializeField, Range(1, jobMaxLevel)] int jobInitialLevel = 1;

        [SerializeField] int jobIndex;

        public int currentHP;

        public int currentSP;

        [SerializeField] int jobLevel;

        const int jobMaxLevel = 10;

        public Sprite JobAvatarImage => jobAvatarImage;

        public string JobName => jobName;

        public int JobLevel
        {
            get => jobLevel;
            set
            {
                if (jobLevel == value || value is < 1 or > jobMaxLevel) return;

                jobLevel = value;
            }
        }

        public List<int> SkillsID;

        public GameObject JobPrefab;

        [SerializeField] TextAsset dataFile;

        [SerializeField] List<JobStats> jobStatsList;

        public List<JobStats> JobStatsList => jobStatsList;

        void OnValidate()
        {
            if (!dataFile) return;

            jobStatsList.Clear();

            string[] textInLines = dataFile.text.Split('\n');

            for (int LineIndex = 1; LineIndex < textInLines.Length-1; LineIndex++)
            {
                string[] statsValues = textInLines[LineIndex].Split(",");

                JobStats currentLevelStats = new JobStats();

                currentLevelStats.maxHP = int.Parse(statsValues[0]);
                currentLevelStats.maxSP = int.Parse(statsValues[1]);
                currentLevelStats.physicalAttack = int.Parse(statsValues[2]);
                currentLevelStats.soulAttack = int.Parse(statsValues[3]);
                currentLevelStats.physicalDefence = int.Parse(statsValues[4]);
                currentLevelStats.soulDefence = int.Parse(statsValues[5]);
                currentLevelStats.hit = int.Parse(statsValues[6]);
                currentLevelStats.nimbleness = int.Parse(statsValues[7]);
                currentLevelStats.speed = int.Parse(statsValues[8]);
                currentLevelStats.critical = int.Parse(statsValues[9]);
                currentLevelStats.melee = int.Parse(statsValues[10]);
                currentLevelStats.remote = int.Parse(statsValues[11]);
                currentLevelStats.expPerLevel = int.Parse(statsValues[12]);
                currentLevelStats.sxpPerLevel = int.Parse(statsValues[13]);
                //currentLevelStats.PhysicalDizziness = int.Parse(statsValues[14]);
                //currentLevelStats.SoulDizziness = int.Parse(statsValues[15]);
                
                jobStatsList.Add(currentLevelStats);
            }
        }

        private void OnEnable()
        {
            jobLevel = jobInitialLevel;
            currentHP = jobStatsList[jobInitialLevel - 1].maxHP;
            currentSP = jobStatsList[jobInitialLevel - 1].maxSP;
            if (SkillsID.Count <= 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    SkillsID.Add(-1);
                }
            }
        }
    }

}