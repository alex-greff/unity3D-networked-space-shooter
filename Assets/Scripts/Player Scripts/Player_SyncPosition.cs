using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings (channel = 0, sendInterval = 0.1f)]
public class Player_SyncPosition : NetworkBehaviour {
	//Syncs the variable to all the clients
	[SyncVar(hook="SyncPositionValues")] //Send Vector3 to all the SyncPositionValues() functions in all the clients
	private Vector3 syncPos;

	[SerializeField] Transform myTransform;
	private float lerpRate;
	private float normalLerpRate = 18; //Might need to tie lerp rate values to compensate for the different shipspeeds
	private float fasterLerpRate = 27;

	private Vector3 lastPos;
	private float threshold = 0F; //0.5F was original

	private List<Vector3>syncPosList = new List<Vector3>();
	[SerializeField] private bool useHistoricalLerping = false;
	private float closeEnough = 0.1f;

	void Start(){
		lerpRate = normalLerpRate;
	}

	// Update is called once per frame
	void FixedUpdate () {
		TransmitPosition ();
	}

	void Update(){
		//Time.deltaTime is a fixed rate in FixedUpdate so it will vary on different machines
		LerpPosition ();
	}

	void LerpPosition(){
		//Smooths the position of the other players
		if (!isLocalPlayer) {
			if(useHistoricalLerping){
				HistoricalLerping(); 
			}
			else{
				OrdinaryLerping();
			}
		}
	}

	//Only the server runs this
	[Command]
	void CmdProvidePositionToServer (Vector3 pos){
		syncPos = pos;
	}

	//Only the clients runs this
	[ClientCallback]
	void TransmitPosition(){
		if (isLocalPlayer && Vector3.Distance(myTransform.position,lastPos)>threshold) {
			CmdProvidePositionToServer (myTransform.position);
			lastPos = myTransform.position;
		}
	}

	[Client]
	void SyncPositionValues(Vector3 latestPos){
		syncPos = latestPos;
		syncPosList.Add (syncPos);
	}

	void OrdinaryLerping(){
		myTransform.position = Vector3.Lerp(myTransform.position,syncPos, Time.deltaTime*lerpRate);
	}

	void HistoricalLerping(){
		if(syncPosList.Count > 0){ //If list isn't empty
			myTransform.position = Vector3.Lerp(myTransform.position,syncPosList[0], Time.deltaTime*lerpRate);

			if (Vector3.Distance(myTransform.position,syncPosList[0]) < closeEnough){ //When [0] is removed the next one over aka [1] takes it's place
				syncPosList.RemoveAt(0);//Remove item in list
			}

			if (syncPosList.Count > 10){ //If the list is getting too big then speed the lerp rate up to catch up
				lerpRate = fasterLerpRate;
			}
			else {
				lerpRate = normalLerpRate;
			}
			print(syncPosList.Count);
		}
	}
}
