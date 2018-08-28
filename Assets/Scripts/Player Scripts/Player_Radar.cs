using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Radar : NetworkBehaviour {
	public GameObject radarDotRed;
	public GameObject radarDotGreen;


//Enable the local player's radar items + camera
	void Start () {
		if (isLocalPlayer) {
			radarDotRed.SetActive (false);
			radarDotGreen.SetActive (true);
		} 
		else {
			radarDotRed.SetActive (true);
			radarDotGreen.SetActive (false);
		}
	}

	public void EnableRadar(){
		if (isLocalPlayer) {
			radarDotRed.SetActive (false);
			radarDotGreen.SetActive (true);
		} 
		else {
			radarDotRed.SetActive (true);
			radarDotGreen.SetActive (false);
		}
	}

	public void DisableRadar(){
		radarDotRed.SetActive(false);
		radarDotGreen.SetActive(false);
	}
}
