using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player_SyncRotation : NetworkBehaviour {
	//Use Quaternion for angles
	
	[SyncVar(hook = "SyncRotationValues")]private Quaternion syncPlayerRotation;
//	[SyncVar]private Quaternion syncCamRotation;

	[SerializeField]private Transform playerTransform;
//	[SerializeField]private Transform camTransform;
	private float lerpRate;
	private float normalLerpRate = 16; //Might need to tie lerp rate values to compensate for the different shipspeeds
	private float fasterLerpRate = 27;

	private Quaternion lastPlayerRot;
	//private Quaternion lastCamRot;
	private float threshold = 0F; //5F was original

	private List<Quaternion>syncRotList = new List<Quaternion>();
	[SerializeField] private bool useHistoricalLerping = false;
	private float closeEnough = 0.11f;

	// Use this for initialization
	void Start () {
		lerpRate = normalLerpRate;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		TransmitRotations ();
	}

	void Update(){
		//Time.deltaTime is a fixed rate in FixedUpdate so it will vary on different machines
		LerpRotations ();
	}

	void LerpRotations(){
		if (!isLocalPlayer) {
			if (useHistoricalLerping){
				HistoricalLerping();
			}
			else{
				OrdinaryLerping();
			}
//			camTransform.rotation = Quaternion.Lerp (camTransform.rotation, syncCamRotation, Time.deltaTime * lerpRate);
		}
	}

	//[Command]
	//void CmdProvideRotationsToServer(Quaternion playerRot, Quaternion camRot){
	//	syncPlayerRotation = playerRot;
//		syncCamRotation = camRot;
	//}
	[Command]
	void CmdProvideRotationsToServer(Quaternion playerRot){
		syncPlayerRotation = playerRot;
		//		syncCamRotation = camRot;
	}

	[ClientCallback]
	void TransmitRotations(){
		if (isLocalPlayer && Quaternion.Angle (playerTransform.rotation, lastPlayerRot) > threshold) {
			//if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot)>threshold||Quaternion.Angle(camTransform.rotation, lastCamRot)>threshold)
				//CmdProvideRotationsToServer(playerTransform.rotation, camTransform.rotation);
			CmdProvideRotationsToServer (playerTransform.rotation);
			lastPlayerRot = playerTransform.rotation;
			//}
		}
	}

	[Client]
	void SyncRotationValues(Quaternion latestRot){
		syncPlayerRotation = latestRot;
		syncRotList.Add (syncPlayerRotation);
	}

	void OrdinaryLerping(){
        if (syncPlayerRotation != new Quaternion(0,0,0,0) && syncPlayerRotation != new Quaternion(0,0,0,0)) {
		    playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
        }
       
        //playerTransform.rotation = syncPlayerRotation;
	}

	void HistoricalLerping(){
		if(syncRotList.Count > 0){ //If list isn't empty
			playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation,syncRotList[0], Time.deltaTime*lerpRate);
			
			if (Quaternion.Angle(playerTransform.rotation,syncRotList[0]) < closeEnough){ //When [0] is removed the next one over aka [1] takes it's place
				syncRotList.RemoveAt(0);//Remove item in list
			}
			
			if (syncRotList.Count > 10){ //If the list is getting too big then speed the lerp rate up to catch up
				lerpRate = fasterLerpRate;
			}
			else {
				lerpRate = normalLerpRate;
			}
			print(syncRotList.Count);
		}
	}
}
