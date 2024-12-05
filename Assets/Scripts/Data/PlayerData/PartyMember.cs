using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

[CreateAssetMenu(menuName = ("Data/CharacterList"), fileName = ("CharacterList_"))]

public class PartyMember : ScriptableObject
{
    [SerializeField] List<JobData> characterList;

    public List<JobData> CharacterList => characterList;
}
