using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

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
				Debug.Log(FormatMessage(message.ToString()));
			}
		}
	}

	public static void LogWarning(object message, VerboseLevel verboseLevel = VerboseLevel.IMPORTANT){
		if(instance != null){
			if(instance.verboseLevel >= verboseLevel){
				Debug.LogWarning(FormatMessage(message.ToString()));
			}
		}
	}

	public static void LogError(object message, VerboseLevel verboseLevel = VerboseLevel.IMPORTANT){
		if(instance != null){
			if(instance.verboseLevel >= verboseLevel){
				Debug.LogError(FormatMessage(message.ToString()));
			}
		}
	}

	public static string FormatMessage(string message){
		try{
			if((GameManager.instance == null) || (!GameManager.instance.network.isLaunched)){
				return message;
			}

			if(GameManager.instance.network.isServer){
				return "Server: " + message;
			}
			else{
				if(!GameManager.instance.network.client.isConnected){
					return message;
				}
				return "Client " + GameManager.instance.network.client.clientId + ": " + message;
			}
		} catch(Exception e){
			Debug.LogException(e);
			throw e;
		}
	}
}
