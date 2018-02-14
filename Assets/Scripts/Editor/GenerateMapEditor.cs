using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateMapEditor {


	[MenuItem("Custom/Generate Normal Map")]
	public static void GenerateNormalMap(){
		GenerateMap(50, 50, 6);
	}

	[MenuItem("Custom/Remove Map")]
	public static void RemoveMap(){
		BaseMap baseMap = GameObject.FindObjectOfType<BaseMap>();
		Transform baseMapTransform = baseMap.GetComponent<Transform>();
		MapBlock[] childBlocks = baseMapTransform.GetComponentsInChildren<MapBlock>();
		foreach(MapBlock child in childBlocks){
			GameObject.DestroyImmediate(child.gameObject);
		}
	}

	private static void GenerateMap(int width, int length, int height){
		BaseMap baseMap = GameObject.FindObjectOfType<BaseMap>();
		baseMap.selfTransform = baseMap.GetComponent<Transform>();
		RemoveMap();

		MapBlock mapBlockPrefab = baseMap.blockPrefab;
		float blockSize = mapBlockPrefab.GetComponent<Transform>().localScale.x + 0.09f;
		MapBlock[][][] mapTable = new MapBlock[width][][];
		for(int i = 0; i < mapTable.Length; i++){
			mapTable[i] = new MapBlock[length][];
			for(int j = 0; j < mapTable[i].Length; j++){
				mapTable[i][j] = new MapBlock[height];
			}
		}

		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				for(int k = 0; k < length; k++){
					int jPos = j + (-width / 2);
					int kPos = k + (-length / 2);
					if(((i != 0) || (jPos != 0) || (kPos != 0))){
						float flatscale = 10f;  
						float flatNoise =  Mathf.Round (Mathf.PerlinNoise (j * flatscale, k * flatscale));
						// Debug.Log(flatNoise);
						MapBlock newBlock = CreateBlock(new Vector3(jPos, flatNoise - i, kPos), blockSize, baseMap, mapBlockPrefab);
						mapTable[j][k][i] = newBlock;
						SetNeighbours(mapTable, newBlock, i, j, k);
					}
				}
			}
		}
	}

	private static MapBlock CreateBlock(Vector3 pos, float blockSize, BaseMap baseMap, MapBlock mapBlockPrefab){
		GameObject obj = PrefabUtility.InstantiatePrefab(mapBlockPrefab.gameObject) as GameObject;
		Transform mbTransform = obj.GetComponent<Transform>();
		mbTransform.SetParent(baseMap.selfTransform);
		mbTransform.localRotation = Quaternion.identity;
		mbTransform.localPosition = pos * blockSize;
		MapBlock mb = obj.GetComponent<MapBlock>();
		mb.selfRenderer = obj.GetComponent<Renderer>();
		return mb;
		//mb.GetComponent<Renderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
	}

	public static void SetNeighbours(MapBlock[][][] mapTable, MapBlock block, int i, int j, int k){
		mapTable[j][k][i] = block;
		MapBlock up = GetBlockAt(mapTable, i+1, j, k);
		MapBlock down = GetBlockAt(mapTable, i-1, j, k);
		MapBlock front = GetBlockAt(mapTable, i, j, k-1);
		MapBlock back = GetBlockAt(mapTable, i, j, k+1);
		MapBlock left = GetBlockAt(mapTable, i, j-1, k);
		MapBlock right = GetBlockAt(mapTable, i, j+1, k);
		block.neighbours = new MapBlock.Neighbours(block, up, down, front, back, left, right);
		block.CalculateVisibility();
	}

	private static MapBlock GetBlockAt(MapBlock[][][] mapTable, int i, int j, int k){
		if((j < 0) || (mapTable.Length <= j)){
			return null;
		}

		if((k < 0) || (mapTable[j].Length <= k)){
			return null;
		}

		if((i < 0) || (mapTable[j][k].Length <= i)){
			return null;
		}

		return mapTable[j][k][i];
	}
}