using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public abstract class NetworkObject {

	public bool isClosed;

	public Network network {get ; private set;}

	protected UdpClient socket;

	protected ConcurrentQueue<Data> actionQueue;

	private DataParser parser;

	private int maxActionPerFrame;

	public NetworkObject(){
		isClosed = false;
		actionQueue = new ConcurrentQueue<Data>();
	}

	protected void Init(UdpClient socket, Network network, int maxActionPerFrame){
		this.socket = socket;
		this.network = network;
		this.maxActionPerFrame = maxActionPerFrame;
		parser = GetParser();
	}

	public virtual void Launch()
	{
		network.StartCoroutine(MakeActionsCoroutine());
		socket.BeginReceive(new AsyncCallback(ReceiveCallback), null);
	}

	private IEnumerator MakeActionsCoroutine(){
		while(!isClosed){
			int nbAction = 0;
			while((!actionQueue.IsEmpty) && (nbAction < maxActionPerFrame)){
				Data newAction;
				if(actionQueue.TryDequeue(out newAction)){
					newAction.ExecuteOnMainThread();
					nbAction++;
				}
			}
			yield return null;
		}
	}

	public void AddMainThreadAction(Data action){
		actionQueue.Enqueue(action);
	}

	protected virtual void SendData(IPEndPoint client, Data message){
		if(message != null){
			byte[] buffer = parser.ToBytes(message);
			CustomDebug.Log("Send : " + message.GetType(), VerboseLevel.ALL);
			socket.BeginSend(buffer, buffer.Length, client, SendCallback, null);
		}
	}

	protected abstract DataParser GetParser();

	public void ReceiveCallback(IAsyncResult asyncResult){
		try{
			IPEndPoint sender = new IPEndPoint(0, 0);
			byte[] buffer = socket.EndReceive(asyncResult, ref sender);

			parser.Parse(sender, buffer);

			socket.BeginReceive(new AsyncCallback(ReceiveCallback), null);
		}
		catch (ObjectDisposedException){
			CustomDebug.LogWarning("Connexion closed", VerboseLevel.IMPORTANT);
			Close();
		}
		catch (SocketException){
			CustomDebug.LogWarning("Connexion closed", VerboseLevel.IMPORTANT);
			Close();
		}
		catch (Exception e)
		{
			Close();
			Debug.LogException(e);
		}
	}

	public void SendCallback(IAsyncResult asyncResult){
		try{
			if(socket.EndSend(asyncResult) == 0){
				CustomDebug.LogWarning("Send empty message", VerboseLevel.IMPORTANT);
			}
		}
		catch (Exception e)
		{
			Close();
			Debug.LogException(e);
		}
	}

	public void Close(){
		socket.Close();
		isClosed = true;
	}

	void OnApplicationQuit()
	{
		if(socket != null){
			Close();
		}
	}
}
