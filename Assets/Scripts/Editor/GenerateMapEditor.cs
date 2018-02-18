using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GenerateMapEditor {

	public readonly static int MAX_HEIGHT = 128;

	[MenuItem("Custom/Generate Normal Map")]
	public static void GenerateNormalMap(){
		World map = GameObject.FindObjectOfType(typeof(World)) as World;
		map.GenerateMap(60, 60, 2, GameManager.GenerateRandomSeed(7f, 9f), GameManager.GenerateRandomSeed(0.08f, 0.12f));
	}

	[MenuItem("Custom/Remove Map")]
	public static void RemoveMap(){
		World baseMap = GameObject.FindObjectOfType<World>();
		Transform baseMapTransform = baseMap.GetComponent<Transform>();
		MapBlock[] childBlocks = baseMapTransform.GetComponentsInChildren<MapBlock>(true);
		foreach(MapBlock child in childBlocks){
			GameObject.DestroyImmediate(child.gameObject);
		}
	}
}