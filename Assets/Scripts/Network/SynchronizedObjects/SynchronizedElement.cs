using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SynchronizedObject))]
public abstract class SynchronizedElement : MonoBehaviour{

    public int owner {get ; private set;}

	protected SynchronizedObject synchronizedObject {get ; private set;}

	protected Data actualData {get ; private set;}

	protected virtual void Awake() {
		synchronizedObject = GetComponent<SynchronizedObject>();
	}

	public virtual void Init(int owner, bool isServer){
		this.owner = owner;
	}

	public void SetOwner(int owner){
		this.owner = owner;
	}

	public abstract ServerData SynchronizeFromServer();

	public abstract ClientData SynchronizeFromClient();

	public virtual void ExecuteOnMainThread(){
		if(actualData != null){
			actualData.ExecuteOnMainThread();
		}
	}

	public void SetData(Data newData){
		actualData = newData;
	}
}
