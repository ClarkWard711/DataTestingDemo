using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

namespace Edgar.Unity.Examples.Example1
{
    public class SimpleExample1GameManager : SimpleGameManagerBase<SimpleExample1GameManager>
    {
        public void Update()
        {
            if (SimpleInputHelper.GetKeyDown(KeyCode.G))
            {
                LoadNextLevel();
            }
        }

        public override void LoadNextLevel()
        {
            ShowLoadingScreen("Example 1", "loading..");
            
            var generator = GameObject.Find("Dungeon").GetComponent<DungeonGeneratorGrid2D>();
            
            StartCoroutine(GeneratorCoroutine(generator));
        }

        private IEnumerator GeneratorCoroutine(DungeonGeneratorGrid2D generator)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            yield return null;
            
            generator.Generate();
            yield return null;
            
            stopwatch.Stop();
            SetLevelInfo($"Generated in {stopwatch.ElapsedMilliseconds / 1000d:F}s.");
            HideLoadingScreen();
        }
    }
}

