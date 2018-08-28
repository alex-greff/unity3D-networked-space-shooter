using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Respawn : NetworkBehaviour {
    Player_NetworkSetup playerNetworkSetup;
	variables variablesScript;
    Player_Buttons playerButtonsScript;
	Player_Health healthScript;
	ShipControl shipControlScript;
	public GameObject targetCrosshair;
	public GameObject playerShip;
	public GameObject engineParent;
    public GameObject playerTrail;
	GameObject respawnButton;
    GameObject quitButton;
    GameObject changeShipButton;
    GameObject cancelButton;

    GameObject smallShipButton;
    GameObject mediumShipButton;
    GameObject largeShipButton;

    NetworkConnection playerConnection;

    Player_Parent playerParentScript;

	// Use this for initialization
	void Start () {
        playerNetworkSetup = GetComponent<Player_NetworkSetup>();
        playerParentScript = GetComponent<Player_Parent>();
		//engineParent = transform.Find ("Engines").gameObject;
		variablesScript = GetComponent<variables>();
		healthScript = GetComponent<Player_Health>();
		shipControlScript = GetComponent<ShipControl>();
        playerButtonsScript = GetComponent<Player_Buttons>();
		healthScript.EventRespawn += EnablePlayer; //Subscribe to event
		SetRespawnButton();
	}

	void SetRespawnButton(){
		if (isLocalPlayer){
            respawnButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton; //Get the saved reference of the repsawn button
			respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn); //Add a listener to the button using script. Same as pressing + in the button
            quitButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().quitButton; //Get the saved reference of the repsawn button
            quitButton.GetComponent<Button>().onClick.AddListener(playerButtonsScript.Disconnect);

            
            changeShipButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().changeShipButton;
            changeShipButton.GetComponent<Button>().onClick.AddListener(LoadChangeShipMenu);
            cancelButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().cancelButton;
            cancelButton.GetComponent<Button>().onClick.AddListener(LoadRespawnMenu);

            smallShipButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().SmallShipButton;
            smallShipButton.GetComponent<Button>().onClick.AddListener(SmallShipLoad);
            mediumShipButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().MediumShipButton;
            mediumShipButton.GetComponent<Button>().onClick.AddListener(MediumShipLoad);
            largeShipButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().LargeShipButton;
            largeShipButton.GetComponent<Button>().onClick.AddListener(LargeShipLoad);
            
		}
	}

    public void SmallShipLoad() {
        LoadNewShip("Small");
    }

    public void MediumShipLoad() {
        LoadNewShip("Medium");
    }

    public void LargeShipLoad() {
        LoadNewShip("Large");
    }

    void LoadNewShip(string type) {
        CommenceRespawn();
        GetComponent<Player_Death>().HidePlayer();
        if (type == "Small") {
            playerNetworkSetup.cameraLerpFollow = 5;
            playerParentScript.Cmd_EnableSmallShip();
            playerParentScript.CmdTellServerMyShip("Small");
            playerParentScript.playerType = "Small";
        }
        else if (type == "Medium") {
            playerNetworkSetup.cameraLerpFollow = 2;
            playerParentScript.Cmd_EnableMediumShip();
            playerParentScript.CmdTellServerMyShip("Medium");
            playerParentScript.playerType = "Medium";
        }
        else if (type == "Large") {
            playerNetworkSetup.cameraLerpFollow = 1.3F;
            playerParentScript.Cmd_EnableLargeShip();
            playerParentScript.CmdTellServerMyShip("Large");
            playerParentScript.playerType = "Large";
        }
        
        /*
        NetworkConnection playerConnection = GetComponent<Player_ID>().playerConnection;
        short playerControllerID = GetComponent<Player_ID>().playerControllerId;
        GameObject prefab = playerPrefab;
        int kills = GetComponent<variables>().kills;
        string name = GetComponent<Player_ID>().playerUniqueIdentity;
        DisableRespawnMenu();

        //Destroy the old player cameras
        Destroy(GetComponent<Player_NetworkSetup>().GetComponent<Camera>());
        Destroy(GetComponent<Player_NetworkSetup>().radarCamera);
        */

        //GameObject.Find("NetworkManager").GetComponent<GameManager_PlayerSpawn>().ChangeShip(playerConnection, playerControllerID, gameObject, prefab, kills, name);
    }

    void LoadRespawnMenu() {
        if (isLocalPlayer) {
            respawnButton.SetActive(true);
            quitButton.SetActive(true);

            
            changeShipButton.SetActive(true);
            cancelButton.SetActive(false);

            smallShipButton.SetActive(false);
            mediumShipButton.SetActive(false);
            largeShipButton.SetActive(false);
            
        }
    }

    void LoadChangeShipMenu() {
        if (isLocalPlayer) {
            respawnButton.SetActive(false);
            quitButton.SetActive(false);

            
            changeShipButton.SetActive(false);
            cancelButton.SetActive(true);

            smallShipButton.SetActive(true);
            mediumShipButton.SetActive(true);
            largeShipButton.SetActive(true);
            
        }
    }

    void DisableRespawnMenu() {
        respawnButton.SetActive(false);
        quitButton.SetActive(false);

        
        changeShipButton.SetActive(false);
        cancelButton.SetActive(false);    

        smallShipButton.SetActive(false);
        mediumShipButton.SetActive(false);
        largeShipButton.SetActive(false);
        
    }

	void OnDisable(){
        //if (healthScript != null)
		//healthScript.EventRespawn -= EnablePlayer; //Ubsubscribe
	}

	public void EnablePlayer(){
		if (isLocalPlayer){
            //Spawn the player at one of the spawn points
            Screen.lockCursor = true;
			GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
			int r = Random.Range(0,spawnPoint.Length-1);
			transform.position = spawnPoint[r].transform.position;
			transform.rotation = spawnPoint[r].transform.rotation;
			
			targetCrosshair.SetActive(true); //Show the crosshair
			shipControlScript.speed = GetComponent<ShipControl>().maxSpeed; //Reset the speed
		}

        //GetComponent<Player_Death>().HidePlayer();
		playerShip.SetActive(true); //Show the ship parent to show all the visual/physical components colliders, meshes, etc.
		engineParent.SetActive(true);
        playerTrail.SetActive(true);
		GetComponent<Player_Radar>().EnableRadar(); //Call the player radar script and enable the icons

		variablesScript.isDead = false;
		healthScript.isDead = false; //TODO: might need to get rid of it
	}


	void CommenceRespawn(){
        DisableRespawnMenu();
        GetComponent<Player_Radar>().EnableRadar(); //Call the player radar script and disable the radar icons
		CmdRespawnOnServer();
	}

	[Command]
	void CmdRespawnOnServer(){ //Run a command on the server to reset the player's health globally
		healthScript.ResetHealth();
	}
}
