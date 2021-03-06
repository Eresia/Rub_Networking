﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SynchronizedObjectClientData : ClientData {

	public int id;

	public ClientData[] data;

	public SynchronizedObjectClientData(ClientData[] data, int id) : base(){
		this.data = data;
		this.id = id;
	}

	protected override bool Validate(){
		if(data == null){
			return false;
		}

		SynchronizedObject obj = serverInformations.server.network.synchronizedObjects.Get(id);
		if(obj == null){
			return false;
		}

		if(obj.owner != serverInformations.server.clients[actualClient].id){
			return false;
		}

		foreach(ClientData cd in data){
			if((cd == null) || !cd.OnlyValidate(serverInformations, actualClient)){
				return false;
			}
		}
		
		return true;
	}

	protected override bool Execute(){
		foreach(ClientData cd in data){
			cd.ValidateAndExecute(serverInformations, actualClient);
		}
		return false;
	}

	public override void ExecuteOnMainThread(){

	}
}
