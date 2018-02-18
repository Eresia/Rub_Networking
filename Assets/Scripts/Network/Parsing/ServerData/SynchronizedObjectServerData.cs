using System.Collections;
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
		if(GameManager.instance.prefabGestion.synchronizedObjectPrefabs.Length <= prefabId){
			return false;
		}

		if(owner < -1){
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
		newObject.Init(id, owner);

		foreach(ServerData sd in data){
			sd.ValidateAndExecute(clientInformations, sender);
		}
	}
}
