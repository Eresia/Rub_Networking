using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class SynchronizedElementServerData<T> : ServerData where T : SynchronizedElement{

	protected int id;

	[System.NonSerialized]
	private SynchronizedObject obj;

	[System.NonSerialized]
	private T element;

	protected SynchronizedElementServerData(int id) : base(){
		this.id = id;
		element = null;
	}

	private bool ValidateType(){
		SynchronizedObject obj = GetSynchronizedObject();
		if(obj == null){
			return false;
		}

		if(!obj.synchronizedElements.ContainsKey(typeof(T))){
			return false;
		}

		return GetSynchronizedElement().CheckTimeStamp(this);
	}

	protected SynchronizedObject GetSynchronizedObject(){
		if(obj == null){
			obj = clientInformations.client.network.synchronizedObjects.Get(id);
		}
		return obj;
	}

	protected T GetSynchronizedElement(){
		if(element == null){
			SynchronizedObject obj = GetSynchronizedObject();
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
