using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
public class Server : MonoBehaviour {

	public int maxActionByFrame;

	[HideInInspector]
	public bool serverClose;

	private IPEndPoint serverInformations;
	private List<UdpClient> clients;
	private UdpClient server;

	private ConcurrentQueue<NetworkAction> actionQueue;

	

	void Awake()
	{
		serverClose = true;
	}

	public void LaunchServer(int port){
		serverClose = false;
		serverInformations = new IPEndPoint(IPAddress.Any, port);
		server = new UdpClient();
		clients = new List<UdpClient>();
		actionQueue = new ConcurrentQueue<NetworkAction>();
	}

	private IEnumerator MakeActionsCoroutine(){
		while(server.Client != null){
			int nbAction = 0;
			while((!actionQueue.IsEmpty) && (nbAction < maxActionByFrame)){
				NetworkAction newAction;
				if(actionQueue.TryDequeue(out newAction)){
					newAction.Execute();
					nbAction++;
				}
			}
			yield return null;
		}
	}

	public void ReceiveCallback(IAsyncResult asyncResult){
		try{
			IPEndPoint sender = (IPEndPoint) asyncResult.AsyncState;
			byte[] buffer = server.EndReceive(asyncResult, ref sender);
			
			/* Parse buffer */

			server.BeginReceive(new AsyncCallback(ReceiveCallback), serverInformations);
		}
		catch (ObjectDisposedException){
			Debug.Log("Connexion closed");
			server.Close();
		}
		catch (Exception err)
		{
			Debug.Log(err);
			server.Close();
		}
	}

	public void SendCallback(IAsyncResult asyncResult){
		try{
			server.EndSend(asyncResult);
		}
		catch (ObjectDisposedException){
			Debug.Log("Connexion closed ");
			server.Close();
		}
		catch (Exception err)
		{
			Debug.Log(err);
			server.Close();
		}
	}
}
