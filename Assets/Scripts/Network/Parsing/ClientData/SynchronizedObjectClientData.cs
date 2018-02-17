using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SynchronizedObjectClientData : ClientData {

	public int id;

	public ClientData[] data;

	public SynchronizedObjectClientData(ClientData[] data){
		this.data = data;
	}

	protected override bool Validate(){
		if(!serverInformations.server.network.HasSynchronizedObject(id)){
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
