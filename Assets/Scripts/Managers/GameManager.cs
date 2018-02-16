using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public World map;
	public Character character;

	[Space]

	public Camera menuCamera;
	public GameObject serverCamera;

	[Space]

	public GameObject pointerCanvas;
	public GameObject menuCanvas;

	[Space]
	public InputField serverPort;

	[Space]
	public InputField clientIp;
	public InputField clientPort;
	

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
		map.GenerateMap(mapWidth, mapLength, GenerateRandomSeed(heightMin, heightMax), GenerateRandomSeed(0.08f, 0.12f));
		serverCamera.gameObject.SetActive(true);
		CommonLaunch();
	}

	public void LaunchClient(){
		map.GenerateMap(mapWidth, mapLength, GenerateRandomSeed(heightMin, heightMax), GenerateRandomSeed(0.08f, 0.12f));
		character.gameObject.SetActive(true);
		pointerCanvas.gameObject.SetActive(true);
		CommonLaunch();
	}

	private void CommonLaunch(){
		menuCamera.gameObject.SetActive(false);
		menuCanvas.SetActive(false);
	}

	public static float GenerateRandomSeed(float min, float max){
		return UnityEngine.Random.Range(min, max);
	}
}
