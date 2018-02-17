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
	public NetworkObject networkObject;

	private void Awake() {
		isLaunched = false;
	}

	public void LaunchServer(int port, World world){
		isServer = true;
		networkObject = new Server(this, port, world, serverMaxAction);
		CommonLaunch();
	}

	public void LaunchClient(string address, int port, World world){
		isServer = false;
		networkObject = new Client(this, address, port, world, clientMaxAction);
		CommonLaunch();
	}

	private void CommonLaunch(){
		networkObject.Launch();
		isLaunched = true;
	}
}
