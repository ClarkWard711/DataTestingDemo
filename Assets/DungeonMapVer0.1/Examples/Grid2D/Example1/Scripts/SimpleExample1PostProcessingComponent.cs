using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples.Example1
{
    public class SimpleExample1PostProcessingComponent : DungeonGeneratorPostProcessingComponentGrid2D
    {
        [Range(0, 1)] 
        public float EnemySpawnChance = 0.5f;
        public override void Run(DungeonGeneratorLevelGrid2D level)
        {
            HandleEnemies(level);
        }

        private void HandleEnemies(DungeonGeneratorLevelGrid2D level)
        {
            foreach (var roomInstance in level.RoomInstances)
            {
                var enemiesHolder = roomInstance.RoomTemplateInstance.transform.Find("Enemies");

                if (enemiesHolder == null)
                {
                    continue;
                }

                foreach (Transform enemyTransform in enemiesHolder)
                {
                    var enemy = enemyTransform.gameObject;

                    if (Random.NextDouble() < EnemySpawnChance)
                    {
                        enemy.SetActive(true);
                    }
                    else
                    {
                        enemy.SetActive(false);
                    }
                    
                }
            }
            
        }
        
    }
}

