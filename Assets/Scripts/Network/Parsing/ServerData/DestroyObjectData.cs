using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DestroyObjectData : ServerData {

	public int id;

	public DestroyObjectData(int id) : base(){
		this.id = id;
	}

	protected override bool Validate(){
		return clientInformations.client.network.synchronizedObjects.Has(id);
	}

	protected override bool Execute(){
		return true;
	}

	public override void ExecuteOnMainThread(){
		GameObject.Destroy(clientInformations.client.network.synchronizedObjects.Get(id).gameObject);
	}
}
