using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour {

	public int serverMaxAction;
	public int clientMaxAction;

	[HideInInspector]
	public bool isServer;

	[HideInInspector]
	public bool isLaunched;

	[HideInInspector]
	public Server server;

	[HideInInspector]
	public Client client;

	private int objectsId;

	private Dictionary<int, SynchronizedObject> synchronizedObjects;

	private void Awake() {
		isLaunched = false;
		objectsId = 0;
		synchronizedObjects = new Dictionary<int, SynchronizedObject>();
	}

	public void LaunchServer(int port, World world){
		isServer = true;
		server = new Server(this, port, world, serverMaxAction);
		server.Launch();
		isLaunched = true;
	}

	public void LaunchClient(string address, int port, World world){
		isServer = false;
		client = new Client(this, address, port, world, clientMaxAction);
		client.Launch();
		isLaunched = true;
	}

	public int RequireNewObjectId(){
		int id = objectsId;
		objectsId++;
		return id;
	}

	public bool CreateSynchronizedObject(int i, SynchronizedObject obj){
		if(HasSynchronizedObject(i)){
			return false;
		}

		synchronizedObjects.Add(i, obj);
		return true;
	}

	public bool HasSynchronizedObject(int i){
		return synchronizedObjects.ContainsKey(i);
	}

	public SynchronizedObject GetSynchronizedObject(int i){
		if(HasSynchronizedObject(i)){
			return synchronizedObjects[i];
		}

		return null;
	}

	public void RemoveSynchronizedObject(int i){
		if(HasSynchronizedObject(i)){
			synchronizedObjects.Remove(i);
		}
	}

	public NetworkObject GetNetworkObject(){
		if(isServer){
			return server;
		}
		else{
			return client;
		}
	}
}
