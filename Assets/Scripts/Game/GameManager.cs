using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

	public World map;

	[Space]

	public PrefabGestion prefabGestion;
	public Character characterPrefab;

	[Space]

	public Camera menuCamera;
	public GameObject serverCamera;

	[Space]

	public GameObject pointerCanvas;
	public GameObject menuCanvas;
	public Text errorText;

	[Space]

	public Network network;

	[Space]
	public Text serverPort;

	[Space]
	public Text clientIp;
	public Text clientPort;
	

	[Space]

	public int mapWidth = 60;
	public int mapLength = 60;
	public float heightMin = 7f;
	public float heightMax = 9f;

	public static GameManager instance;

	

	void Awake () {
		if(instance != null){
			Destroy(gameObject);
			return ;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void LaunchServer(){
		try{
			int port = int.Parse(serverPort.text);
			map.GenerateMap(mapWidth, mapLength, GenerateRandomSeed(heightMin, heightMax), GenerateRandomSeed(0.08f, 0.12f));
			serverCamera.gameObject.SetActive(true);
			network.LaunchServer(port, map);
			CommonLaunch();
		} catch(FormatException){
			errorText.text = "Port need to be a number";
		}
		
	}

	public void LaunchClient(){
		try{
			int port = int.Parse(clientPort.text);
			pointerCanvas.gameObject.SetActive(true);
			serverCamera.gameObject.SetActive(true);
			network.LaunchClient(clientIp.text, port, map);
			CommonLaunch();
		} catch(FormatException){
			errorText.text = "Port need to be a number";
		}
	}

	private void CommonLaunch(){
		menuCamera.gameObject.SetActive(false);
		menuCanvas.SetActive(false);
	}

	public static float GenerateRandomSeed(float min, float max){
		return UnityEngine.Random.Range(min, max);
	}
}
