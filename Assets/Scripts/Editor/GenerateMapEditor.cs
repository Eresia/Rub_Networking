using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GenerateMapEditor {

	public readonly static int MAX_HEIGHT = 128;

	[MenuItem("Custom/Generate Normal Map")]
	public static void GenerateNormalMap(){
		GenerateMap(60, 60, GenerateRandomSeed(7f, 9f), GenerateRandomSeed(0.08f, 0.12f));
	}

	[MenuItem("Custom/Remove Map")]
	public static void RemoveMap(){
		Map baseMap = GameObject.FindObjectOfType<Map>();
		Transform baseMapTransform = baseMap.GetComponent<Transform>();
		MapBlock[] childBlocks = baseMapTransform.GetComponentsInChildren<MapBlock>(true);
		foreach(MapBlock child in childBlocks){
			GameObject.DestroyImmediate(child.gameObject);
		}
	}

	private static float GenerateRandomSeed(float min, float max){
		return UnityEngine.Random.Range(min, max);
	}

	private static void GenerateMap(int width, int length, float floatScale, float biomeSeed){
		Map map = GameObject.FindObjectOfType<Map>();
		map.selfTransform = map.GetComponent<Transform>();
		RemoveMap();

		float blockSize = map.blockPrefab.GetComponent<Transform>().localScale.x + 0.05f;
		MapBlock[][][] mapTable = new MapBlock[width][][];
		for(int i = 0; i < mapTable.Length; i++){
			mapTable[i] = new MapBlock[length][];
			for(int j = 0; j < mapTable[i].Length; j++){
				mapTable[i][j] = new MapBlock[Mathf.RoundToInt(floatScale) + 1];
			}
		}

		for(int i = 0; i < width; i++){
			for(int j = 0; j < length; j++){
				int jPos = i + (-width / 2);
				int kPos = j + (-length / 2);

				int flatNoise =  Mathf.RoundToInt (MakePerlinNoise(i, j, floatScale));
				int biomeNoise = (int) (MakePerlinNoise(i*biomeSeed, j*biomeSeed, map.possibleMaterials.Length + biomeSeed));
				try{
					MapBlock prefab;
					if(((jPos != 0) || (kPos != 0))){
						prefab = map.blockPrefab;
					}
					else{
						prefab = map.baseMapPrefab;
					}

					MapBlock newBlock = CreateBlock(new Vector3(jPos, flatNoise, kPos), blockSize, map.selfTransform, prefab);
					newBlock.selfRenderer.material = map.possibleMaterials[biomeNoise];
					mapTable[i][j][flatNoise] = newBlock;
					SetNeighbours(mapTable, newBlock, i, j, flatNoise);

					for(int k = flatNoise-1; k >= 0; k--){
						MapBlock newBlockHeight = CreateBlock(new Vector3(jPos, k, kPos), blockSize, map.selfTransform, prefab);
						newBlockHeight.selfRenderer.material = map.possibleMaterials[biomeNoise];
						mapTable[i][j][k] = newBlockHeight;
						SetNeighbours(mapTable, newBlockHeight, i, j, k);
					}
				} catch(IndexOutOfRangeException e){
					Debug.Log(i + " " + j + " " + flatNoise);
					Debug.Log(MakePerlinNoise(i, j, floatScale));
					// Debug.Log(Mathf.RoundToInt (MakePerlinNoise(i*0.2f, j*0.02f, map.possibleMaterials.Length - 1)));
					Debug.LogError(e.Message);
				}
			}
		}
	}

	private static float MakePerlinNoise(float i, float j, float flatscale){
		return Mathf.PerlinNoise (((float) i) / flatscale, ((float) j) / flatscale) * flatscale;
	}

	private static MapBlock CreateBlock(Vector3 pos, float blockSize, Transform mapTransform, MapBlock mapBlockPrefab){
		GameObject obj = PrefabUtility.InstantiatePrefab(mapBlockPrefab.gameObject) as GameObject;
		Transform mbTransform = obj.GetComponent<Transform>();
		mbTransform.SetParent(mapTransform);
		mbTransform.localRotation = Quaternion.identity;
		mbTransform.localPosition = new Vector3(pos.x * blockSize, pos.y, pos.z * blockSize);
		MapBlock mb = obj.GetComponent<MapBlock>();
		mb.selfRenderer = obj.GetComponent<Renderer>();
		return mb;
		//mb.GetComponent<Renderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1);
	}

	public static void SetNeighbours(MapBlock[][][] mapTable, MapBlock block, int i, int j, int k){
		mapTable[i][j][k] = block;
		MapBlock up = GetBlockAt(mapTable, i, j, k+1);
		MapBlock down = GetBlockAt(mapTable, i, j, k-1);
		MapBlock front = GetBlockAt(mapTable, i, j-1, k);
		MapBlock back = GetBlockAt(mapTable, i, j+1, k);
		MapBlock left = GetBlockAt(mapTable, i-1, j, k);
		MapBlock right = GetBlockAt(mapTable, i+1, j, k);
		block.neighbours = new MapBlock.Neighbours(block, up, down, front, back, left, right);
		block.CalculateVisibility();
	}

	private static MapBlock GetBlockAt(MapBlock[][][] mapTable, int i, int j, int k){
		if((i < 0) || (mapTable.Length <= i)){
			return null;
		}

		if((j < 0) || (mapTable[i].Length <= j)){
			return null;
		}

		if((k < 0) || (mapTable[i][j].Length <= k)){
			return null;
		}

		return mapTable[i][j][k];
	}
}