using System.Collections;
using System.Collections.Generic;
using Edgar.Unity;
using UnityEngine;

public class GenerateTest : MonoBehaviour
{
	public GameObject generator;
	public int seed;

	public void Awake()
	{
		generator.GetComponent<DungeonGeneratorBaseGrid2D>().RandomGeneratorSeed = seed;
		generator.GetComponent<DungeonGeneratorBaseGrid2D>().Generate();
		//seed = generator.GetComponent<DungeonGeneratorBaseGrid2D>().RandomGeneratorSeed;
		//Debug.Log(seed);
		//Debug.Log(generator.GetComponent<DungeonGeneratorBaseGrid2D>().RandomGeneratorSeed);
	}
}
