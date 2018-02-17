using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SynchronizedObjectServerData : ServerData {

	public int id;

	public int prefabId;

	public ServerData[] data;

	public SynchronizedObjectServerData(ServerData[] data){
		this.data = data;
	}

	protected override bool Validate(){
		if(GameManager.instance.prefabGestion.synchronizedObjectPrefabs.Length <= prefabId){
			return false;
		}

		foreach(ServerData sd in data){
			if((sd == null) || !sd.OnlyValidate(clientInformations, sender)){
				return false;
			}
		}
		
		return true;
	}

	protected override bool Execute(){
		if(!clientInformations.client.network.HasSynchronizedObject(id)){
			return true;
		}

		foreach(ServerData sd in data){
			sd.ValidateAndExecute(clientInformations, sender);
		}
		return false;
	}

	public override void ExecuteOnMainThread(){
		SynchronizedObject newObject = GameObject.Instantiate<SynchronizedObject>(GameManager.instance.prefabGestion.synchronizedObjectPrefabs[prefabId]);
		newObject.Init(id);

		foreach(ServerData sd in data){
			sd.ValidateAndExecute(clientInformations, sender);
		}
	}
}
