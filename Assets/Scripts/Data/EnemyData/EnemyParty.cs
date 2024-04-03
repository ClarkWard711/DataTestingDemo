using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = ("Data/EnemyPartyData"), fileName = ("EnemyPartyData_"))]

    public class EnemyParty : ScriptableObject
    {
        [SerializeField] List<EnemyData> enemyDataList;

        public List<EnemyData> EnemyDataList => enemyDataList;
    }
}

