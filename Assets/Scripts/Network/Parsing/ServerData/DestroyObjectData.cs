using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectData : ServerData {

	public int id;

	public DestroyObjectData(int id){
		this.id = id;
	}

	protected override bool Validate(){
		return IsConnected() && clientInformations.client.network.HasSynchronizedObject(id);
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		GameObject.Destroy(clientInformations.client.network.GetSynchronizedObject(id).gameObject);
	}
}
