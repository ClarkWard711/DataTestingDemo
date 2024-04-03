using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;
    void OnEnable()
    {
        //EnemyData.EnemyLevel = Random.Range(1, 10); 
    }
}
