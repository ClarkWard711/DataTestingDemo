using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = ("Data/PartyData"), fileName = ("PartyData_"))]

    public class PartyData : ScriptableObject
    {
        [SerializeField] List<JobData> jobDataList;

        public List<JobData> JobDataList => jobDataList;
    }
}
