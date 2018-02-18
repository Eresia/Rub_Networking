﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SynchronizedObjectServerData : ServerData {

	public int id;

	public int owner;

	public int prefabId;

	public ServerData[] data;

	public SynchronizedObjectServerData(ServerData[] data, int id, int prefabId, int owner){
		this.id = id;
		this.data = data;
		this.prefabId = prefabId;
		this.owner = owner;
	}

	protected override bool Validate(){
		if(id < 0){
			return false;
		}

		if(data == null){
			return false;
		}

		if(GameManager.instance.prefabGestion.synchronizedObjectPrefabs.Length <= prefabId){
			return false;
		}

		if(owner < -1){
			return false;
		}

		if(clientInformations.client.network.HasSynchronizedObject(id)){
			foreach(ServerData sd in data){
				if((sd == null) || !sd.OnlyValidate(clientInformations, sender)){
					return false;
				}
			}
		}
		else if(clientInformations.client.network.HasRequiredObject(id)){
			return false;
		}
		
		return true;
	}

	protected override bool Execute(){
		if(!clientInformations.client.network.HasSynchronizedObject(id)){
			clientInformations.client.network.RequireObject(id);
			return true;
		}

		foreach(ServerData sd in data){
			sd.ValidateAndExecute(clientInformations, sender);
		}

		return false;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedObject newObject = GameObject.Instantiate<SynchronizedObject>(GameManager.instance.prefabGestion.synchronizedObjectPrefabs[prefabId]);
		newObject.Init(id, owner);
		clientInformations.client.network.EndRequireObject(id);
	}
}
