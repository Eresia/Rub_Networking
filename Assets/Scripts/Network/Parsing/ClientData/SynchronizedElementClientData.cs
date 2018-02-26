using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SynchronizedElementClientData<T> : ClientData where T : SynchronizedElement{

	protected int id;

	[System.NonSerialized]
	private T element;

	protected SynchronizedElementClientData(int id){
		this.id = id;
		element = null;
	}

	private bool ValidateType(){
		SynchronizedObject obj = serverInformations.server.network.synchronizedObjects.Get(id);
		if(obj == null){
			return false;
		}

		if(obj.owner != serverInformations.server.clients[actualClient].id){
			return false;
		}

		return obj.synchronizedElements.ContainsKey(typeof(T));
	}

	protected T GetSynchronizedElement(){
		if(element == null){
			SynchronizedObject obj = serverInformations.server.network.synchronizedObjects.Get(id);
			element = (T) obj.synchronizedElements[typeof(T)];
		}
		return element;
	}

	protected override bool CheckAllValidation(){
		return ValidateType() && base.CheckAllValidation();
	}

	protected override void AddMainThreadAction(){
		GetSynchronizedElement().SetData(this);
	}
}
