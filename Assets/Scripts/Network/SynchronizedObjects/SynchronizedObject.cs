using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SynchronizedObject : MonoBehaviour {

	public int objectPrefabId;

	[HideInInspector]
	public int id;

	private Network network;

	private SynchronizedElement[] synchronizedElements;

	List<Data> data;

	private void Awake() {
		synchronizedElements = GetComponents<SynchronizedElement>();

		if(synchronizedElements == null){
			synchronizedElements = new SynchronizedElement[0];
		}

		network = GameManager.instance.network;
		data = new List<Data>();
		if(network.isServer){
			id = network.RequireNewObjectId();
			Init(id);
		}
	}

	public virtual void Init(int id) {
		network.CreateSynchronizedObject(id, this);
		this.id = id;
	}

	protected void Update() {
		if(network == null){
			return ;
		}

		data.Clear();

		if(network.isServer){
			foreach(SynchronizedElement se in synchronizedElements){
				ServerData newData = se.SynchronizeFromServer();
				if(newData != null){
					data.Add(newData);
				}
			}

			SynchronizedObjectServerData finalData = new SynchronizedObjectServerData((ServerData[]) data.ToArray());
			network.server.SendDataToAllClients(finalData);
		}
		else{
			foreach(SynchronizedElement se in synchronizedElements){
				ClientData newData = se.SynchronizeFromClient();
				if(newData != null){
					data.Add(newData);
				}
			}

			SynchronizedObjectClientData finalData = new SynchronizedObjectClientData((ClientData[]) data.ToArray());
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
}
