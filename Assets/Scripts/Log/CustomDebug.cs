using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VerboseLevel {
	NONE,
	IMPORTANT,
	INFORMATIONS,
	ALL
}

public class CustomDebug : MonoBehaviour {

	public VerboseLevel verboseLevel;

	public static CustomDebug instance;
	
	void Awake()
	{
		if(instance != null){
			Destroy(gameObject);
			return ;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public static void Log(object message, VerboseLevel verboseLevel = VerboseLevel.IMPORTANT){
		if(instance != null){
			if(instance.verboseLevel >= verboseLevel){
				Debug.Log(message);
			}
		}
	}

	public static void LogWarning(object message, VerboseLevel verboseLevel = VerboseLevel.IMPORTANT){
		if(instance != null){
			if(instance.verboseLevel >= verboseLevel){
				Debug.LogWarning(message);
			}
		}
	}

	public static void LogError(object message, VerboseLevel verboseLevel = VerboseLevel.IMPORTANT){
		if(instance != null){
			if(instance.verboseLevel >= verboseLevel){
				Debug.LogError(message);
			}
		}
	}
}
