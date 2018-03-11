using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Concurrent;

[RequireComponent(typeof(SynchronizedObject))]
public abstract class SynchronizedElement : MonoBehaviour{

    public int owner {get ; private set;}

	public bool isServer {get; private set;}

	protected SynchronizedObject synchronizedObject {get ; private set;}

	protected ConcurrentDictionary<Type, Data> actualData {get ; private set;}

	protected virtual void Awake() {
		synchronizedObject = GetComponent<SynchronizedObject>();
		actualData = new ConcurrentDictionary<Type, Data>();
	}

	public virtual void Init(int owner, bool isServer){
		this.owner = owner;
		this.isServer = isServer;
	}

	public void SetOwner(int owner){
		this.owner = owner;
	}

	public abstract ServerData SynchronizeFromServer();

	public abstract ServerData SynchronizeFromServerToOwner();

	public abstract ClientData SynchronizeFromClient();

	public virtual void ExecuteOnMainThread(){
		foreach(Data data in actualData.Values){
			data.ExecuteOnMainThread();
		}
	}

	public void SetData(Data newData){
		if(!actualData.ContainsKey(newData.GetType())){
			actualData.TryAdd(newData.GetType(), newData);
		}
		else{
			actualData[newData.GetType()] = newData;
		}
		
	}

	public bool CheckTimeStamp(Data data){
		if(!actualData.ContainsKey(data.GetType())){
			return true;
		}

		return actualData[data.GetType()].time < data.time;
	}
}
