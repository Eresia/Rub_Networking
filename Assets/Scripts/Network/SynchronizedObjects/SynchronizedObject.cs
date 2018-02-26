using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SynchronizedObject : MonoBehaviour {

	public int objectPrefabId;

	public float timeout {get; set;}

	public int id {get; private set;}

	public int owner {get; private set;}

	private Network network;

	public Dictionary<Type, SynchronizedElement> synchronizedElements {get ; private set;}

	private List<Data> data;

	private void Awake() {
		synchronizedElements = new Dictionary<Type, SynchronizedElement>();
		SynchronizedElement[] elements = GetComponents<SynchronizedElement>();

		if(elements != null){
			foreach(SynchronizedElement se in elements){
				synchronizedElements.Add(se.GetType(), se);
			}
		}

		network = GameManager.instance.network;
		data = new List<Data>();
		if(network.isServer){
			id = network.RequireNewObjectId();
			Init(id, -1);
		}
	}

	public virtual void Init(int id, int owner) {
		network.synchronizedObjects.Create(id, this);
		this.id = id;
		this.owner = owner;

		foreach(SynchronizedElement se in synchronizedElements.Values){
			se.Init(owner, network.isServer);
		}
	}

	protected void Update() {
		if(network == null){
			return ;
		}

		data.Clear();

		if(network.isServer){
			foreach(SynchronizedElement se in synchronizedElements.Values){
				se.ExecuteOnMainThread();
				ServerData newData = se.SynchronizeFromServer();
				if(newData != null){
					data.Add(newData);
				}
			}

			SynchronizedObjectServerData finalData = new SynchronizedObjectServerData(data.Cast<ServerData>().ToArray(), id, objectPrefabId, owner);
			network.server.SendDataToAllClients(finalData);
		}
		else{
			foreach(SynchronizedElement se in synchronizedElements.Values){
				se.ExecuteOnMainThread();
				ClientData newData = se.SynchronizeFromClient();
				if(newData != null){
					data.Add(newData);
				}

				se.ExecuteOnMainThread();
			}

			SynchronizedObjectClientData finalData = new SynchronizedObjectClientData(data.Cast<ClientData>().ToArray(), id);
			network.client.SendData(finalData);
		}
	}

	private void OnDestroy() {
		if(network == null){
			return ;
		}

		network.synchronizedObjects.Remove(id);
		if(network.isServer){
			network.server.SendDataToAllClients(new DestroyObjectData(id));
		}
	}

	public void SetOwner(int owner){
		this.owner = owner;

		foreach(SynchronizedElement se in synchronizedElements.Values){
			se.SetOwner(owner);
		}
	}

	public bool IsOwner(){
		if(network.isServer){
			return (owner == -1);
		}
		else{
			return (network.client.clientId == owner);
		}
	}
}
