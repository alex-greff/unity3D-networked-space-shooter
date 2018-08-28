using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_FogCreate : NetworkBehaviour {
	public GameObject fogPrefab;
	public float intervalTime; 
	float nextTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) { //If its the local player (show only on the client side
			if (Time.time > nextTime) { //Spawn a new fog item once every time peroid
				GameObject fog = (GameObject)Instantiate (fogPrefab, transform.position, Quaternion.identity); //Create the fog at the player's pos
				Destroy (fog, fog.GetComponent<ParticleSystem> ().duration * 2F); //Destroy the fog after it has finished its cycle
				nextTime = Time.time + intervalTime; //Set timer to next interval
			}
		}
	}
}
