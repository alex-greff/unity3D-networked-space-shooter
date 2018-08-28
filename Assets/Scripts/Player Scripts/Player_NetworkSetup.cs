using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {
	//[SerializeField] Camera PlayerCamera;
	//public GameObject PlayerCamera;
	public GameObject cameraPrefab;
    public float cameraLerpFollow; //The lerp speed the camera follows at

	public GameObject radarCameraPrefab;
	public GameObject targetUI;

    //public Vector3 cameraPosOffest;
    //public Quaternion cameraRotOffset;

    //[SerializeField] AudioListener audioListener; //FOR WHEN I ADD THE SOUNDS

    public GameObject playerCamera;
    public GameObject radarCamera;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			//Create the camera for the player
			playerCamera = (GameObject)Instantiate(cameraPrefab, transform.position, transform.rotation); //Create the camera only on the local client
			playerCamera.GetComponent<Camera_sync>().targetGameObject = gameObject; //Set the camera's target to this player's gameobject
            playerCamera.GetComponent<Camera_sync>().lerpRatePos = cameraLerpFollow; //Apply the lerp rate
			playerCamera.transform.name = GetComponent<Player_ID>().playerUniqueIdentity + " camera"; //Rename the camera to the player's name + camera (for ease of recognizing it)

			//Create the Radar Camera
			radarCamera = (GameObject)Instantiate(radarCameraPrefab, transform.position, transform.rotation); //Create the radar camera only on the local client
			radarCamera.GetComponent<RadarCamera_sync>().targetGameObject = gameObject; //Set the target to this player's gameobject
			radarCamera.transform.name = GetComponent<Player_ID>().playerUniqueIdentity + " radar camera"; //Rename the it to the player's name + radar camera (for ease of recognizing it)

			targetUI.SetActive(true); //Enable the targetting crosshairs

			GameObject.Find ("Main Camera").SetActive(false); //Disable the default main camera in the scene

			GetComponent<ShipControl>().enabled  = true; //Enable the script that allows the player to control their ship (may be redundant)
		}
	}

    void Update() {
        if (playerCamera != null) {
            playerCamera.GetComponent<Camera_sync>().lerpRatePos = cameraLerpFollow; //Apply the lerp rate
        }
    }
}
