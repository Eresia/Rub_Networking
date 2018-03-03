using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Linq;

public class Network : MonoBehaviour {

	public int serverMaxAction;
	public int clientMaxAction;

	[Space]

	public float serverTimeout;
	public float clientTimeout;

	public bool isServer {get ; private set;}

	public bool isLaunched {get ; private set;}

	public Server server {get ; private set;}

	public Client client {get ; private set;}

	private readonly static object RequireLock = new object();

	private int objectsId;

	public SynchronizedObjectGestion synchronizedObjects {get; private set;}

	private void Awake() {
		isLaunched = false;
		objectsId = 0;
		synchronizedObjects = new SynchronizedObjectGestion(RequireLock);
	}

	public void LaunchServer(int port, World world){
		if(!isLaunched){
			isServer = true;
			server = new Server(this, port, world, serverMaxAction, serverTimeout);
			server.Launch();
			isLaunched = true;
		}
	}

	public void LaunchClient(string address, int port, World world){
		if(!isLaunched){
			isServer = false;
			client = new Client(this, address, port, world, clientMaxAction, clientTimeout);
			client.Launch();
			isLaunched = true;
		}
	}

	private void Update() {
		if(isLaunched){
			if(isServer){
				int[] removedClient = server.CheckTimeout(Time.deltaTime);
				if(removedClient.Length > 0){
					int[] synchronizedObjectsIds = synchronizedObjects.GetExistantIds();
					foreach(int id in synchronizedObjectsIds){
						if(removedClient.Contains(synchronizedObjects.Get(id).owner)){
							synchronizedObjects.RemoveAndDestroy(id);
						}
					}
				}
				
			}
			else if(client.isConnected){
				client.CheckTimeout(Time.deltaTime);
				int[] actualObjects = synchronizedObjects.GetExistantIds();
				foreach(int obj in actualObjects){
					synchronizedObjects.IncrementTime(obj, Time.deltaTime);
					if(synchronizedObjects.Timeout(obj, client.timeout)){
						synchronizedObjects.RemoveAndDestroy(obj);
					}
				}
			}
		}
	}

	public int RequireNewObjectId(){
		int id = objectsId;
		objectsId++;
		return id;
	}

	public NetworkObject GetNetworkObject(){
		if(isServer){
			return server;
		}
		else{
			return client;
		}
	}

	private void OnDestroy() {
		if(isServer){
			server.Close();
		}
		else{
			client.SendData(new DisconnexionData());
			client.Close();
		}
	}
}
