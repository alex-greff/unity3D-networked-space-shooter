using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Buttons : MonoBehaviour   {
    variables variablesScript;


	// Use this for initialization
	void Start () {
        variablesScript = GetComponent<variables>();
	}

    public void Disconnect() {        
        //print("Trying to disconnect");

        NetworkManager.singleton.StopHost();
    }

}
