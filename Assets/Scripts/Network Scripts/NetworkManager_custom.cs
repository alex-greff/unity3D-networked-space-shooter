using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_custom : NetworkManager {
	//Doesn't recognize singleton
	public void StartUpHost(){
		SetPort();
        //NetworkManager.singleton.StartHost();
	}


	public void JoinGame(){
		SetIPAddress();
		SetPort();
		//NetworkManager.singleton.StartClient();
	}

	void SetIPAddress(){
		//string ipAddress = GameObject.Find();
	}

	void SetPort(){
		//NetworkManager.singleton.networkPort = 7777;
	}
}
