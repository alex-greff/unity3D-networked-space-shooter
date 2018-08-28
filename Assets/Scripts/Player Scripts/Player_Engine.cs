using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player_Engine : NetworkBehaviour {
    //[SyncVar(hook = "OnAmmountChanged")]public float engineAmmount;

    //public ParticleSystem[] engineLocal; //An array with all the engines of the ship


	//[SerializeField]
    public ParticleSystem[] engines;

	//[SyncVar]
	private float startSpeedGlobal;


	//float[] engineStartSpeed; //Stores the starting values of each of the engines
	public float speed;
	public float maxSpeed;

    //void OnAmmountChanged(float amt) {
    //    engineAmmount = amt;
    //}

    // Use this for initialization
    void Start () {

		speed = GetComponent<ShipControl> ().speed; //Get the player's current speed
		maxSpeed = GetComponent<ShipControl> ().maxSpeed; //Get the player's max speed
		//engines = engineLocal;

		//engineStartSpeed = new float[engineLocal.Length]; //Initialize the array
		//Get the starting values of all the engines
		//for (int i = 0; i < engineLocal.Length; i++) {
        //    if (engineLocal[i] != null)
		//	engineStartSpeed [i] = engineLocal [i].startSpeed;	
		//}
        
	}

	// Update is called once per frame
	void Update () {
        speed = GetComponent<ShipControl> ().speed; //Get the player's current speed
		maxSpeed = GetComponent<ShipControl> ().maxSpeed; //Get the player's max speed
        for (int i = 0; i < engines.Length; i++) { //For each engine on the ship
			float percentOff = speed / maxSpeed; //Get the percent (in decimals) that the ship is of it's max speed
			float startSpeed = 50 * percentOff;
			engines [i].startSpeed = startSpeed; //Apply the start speed that is the percent of the original startspeed

            //engineLocal[i].startSize = Random.Range(0.01F * startSpeed, 0.2F * startSpeed);
            if (startSpeed == 0) {
                engines[i].startSize = 0;
            }
            else {
                engines[i].startSize = Random.Range(0.05F, 1.5F);
            }
		}

        //UpdateEngine (); //Update the ammount the engine is moving by
	}
	//[ClientCallback]
	void UpdateEngine(){
		//if (isLocalPlayer) {
                /*
			for (int i = 0; i < engineLocal.Length; i++) { //For each engine on the ship
            
				float percentOff = speed / maxSpeed; //Get the percent (in decimals) that the ship is of it's max speed
				float startSpeed = engineStartSpeed [i] * percentOff;
				engineLocal [i].startSpeed = startSpeed; //Apply the start speed that is the percent of the original startspeed

                //engineLocal[i].startSize = Random.Range(0.01F * startSpeed, 0.2F * startSpeed);
                if (startSpeed == 0) {
                    engineLocal[i].startSize = 0;
                }
                else {
                    engineLocal[i].startSize = Random.Range(0.05F, 1.5F);
                }
                

				CmdProvideEnginePercent (startSpeed); //Provide percent off to server
                
			}
            */
		//} 
        
		//else if (!isLocalPlayer){
			//for (int i = 0; i < engines.Length; i++) {
                /*
				float percentOff = percentOffGlobal; //The precent off
				float startSpeed = engineStartSpeed [i] * percentOffGlobal;
				engines [i].startSpeed = startSpeed; //Apply the start speed that is the percent of the original startspeed

                engines[i].startSize = Random.Range(0.5F * percentOffGlobal, 1.5F * percentOffGlobal); //Apply the start size that is the percent of the original startspeed
                */
                //engines [i].startSpeed = startSpeedGlobal; //Apply the start speed that is the percent of the original startspeed
                //if (startSpeedGlobal == 0) {
                //    engines[i].startSize = 0;
                //}
                //else {
                //    engines[i].startSize = Random.Range(0.05F, 1.5F);
                //}
			//}
		//}
        //if (isLocalPlayer) {
            
        //}
	}

    
	[Command]
	void CmdProvideEnginePercent (float x){
		startSpeedGlobal = x;
	}
    
    [ClientRpc]
    void Rpc_GetEngineSpeed(float speed) {
        startSpeedGlobal = speed;
    }

	//[Client]
	//void SyncStartSpeed (float speed){
		
	//}
}
