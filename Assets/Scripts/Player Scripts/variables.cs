using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class variables : NetworkBehaviour {
    
	[SyncVar] public bool cameraFreeLook = false;
	[SyncVar] public bool isDead = false;
    [SyncVar] public bool spotlightOn = false;
    [SyncVar] public bool isBoosting = false;
    [SyncVar] public bool isCameraLooking = false;
    [SyncVar] public bool isTyping = false;
    [SyncVar] public bool isPaused = false;
    [SyncVar] public int kills = 0;
	//public float maxSpeed;
	//public float maneuverability;
	//public float fireDelay; //In seconds

	// Use this for initialization
	void Start () {
		//SyncVariables();
	}
	
	// Update is called once per frame
	void Update () {
		
		

		//SyncVariables();
	}
	/*
	void SyncVariables(){
		GetComponent<ShipControl>().maxSpeed = maxSpeed;
		GetComponent<ShipControl>().maneuverability = maneuverability;

		GetComponent<Player_Fire>().fireDelay = fireDelay;
	}
	*/
}
