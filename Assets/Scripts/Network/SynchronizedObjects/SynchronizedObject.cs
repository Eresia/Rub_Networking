using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SynchronizedObject : MonoBehaviour {

	public int objectPrefabId;

	[HideInInspector]
	public int id;

	[HideInInspector]
	public int owner;

	private Network network;

	[HideInInspector]
	public Dictionary<Type, SynchronizedElement> synchronizedElements;

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
		network.CreateSynchronizedObject(id, this);
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
				ClientData newData = se.SynchronizeFromClient();
				if(newData != null){
					data.Add(newData);
				}
			}

			SynchronizedObjectClientData finalData = new SynchronizedObjectClientData(data.Cast<ClientData>().ToArray(), id);
			network.server.SendDataToAllClients(finalData);
		}
	}

	private void OnDestroy() {
		if(network == null){
			return ;
		}

		network.RemoveSynchronizedObject(id);
		if(network.isServer){
			network.server.SendDataToAllClients(new DestroyObjectData(id));
		}
	}

	public bool IsOwner(){
		if(network.isServer){
			return (owner == -1);
		}
		else{
			return (network.client.clientInformations.clientId == owner);
		}
	}
}
