using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player_Parent : NetworkBehaviour {
    public GameObject smallShipPackage;
    public GameObject mediumShipPackage;
    public GameObject largeShipPackage;

    public GameObject smallShipPrimaryWeapon;
    public GameObject mediumShipPrimaryWeapon;
    public GameObject largeShipPrimaryWeapon;

    public GameObject smallShipSpecialWeapon;
    public GameObject mediumShipSpecialWeapon;
    public GameObject largeShipSpecialWeapon;

    public AudioClip smallPrimarySfx;
    public AudioClip mediumPrimarySfx;
    public AudioClip largePrimarySfx;
    public AudioClip missileLaunchSfx;
    public AudioClip mineLaunchSfx;
    public AudioClip nukeLaunchSfx;


    variables variablesScript;
    Player_Boost playerBoostScript;
    Player_Death playerDeathScript;
    Player_Respawn playerRespawnScript;
    Player_Fire playerFireScript;
    Player_Health playerHealthScript;
    Player_ID playerIDScript;
    ShipControl shipControlScript;
    Player_Engine playerEngineScript;
    Player_Spotlight playerSpotlightScript;
    Player_NetworkSetup playerNetworkSetup;

    [SyncVar]
    public string playerTypeGlobal;

    public string playerType;

	// Use this for initialization
	void Start () {
        //if (isLocalPlayer) {

        variablesScript = GetComponent<variables>();
        playerBoostScript = GetComponent<Player_Boost>();
        playerDeathScript = GetComponent<Player_Death>();
        playerRespawnScript = GetComponent<Player_Respawn>();
        playerFireScript = GetComponent<Player_Fire>();
        playerHealthScript = GetComponent<Player_Health>();
        playerIDScript = GetComponent<Player_ID>();
        shipControlScript = GetComponent<ShipControl>();
        playerEngineScript = GetComponent<Player_Engine>();
        playerSpotlightScript = GetComponent<Player_Spotlight>();
        playerNetworkSetup = GetComponent<Player_NetworkSetup>();

        if (isLocalPlayer) {
            playerType = PlayerPrefs.GetString("ShipType");
            //print("Setting player type to: " + playerType);
            if (playerType != "Large" && playerType != "Medium" && playerType != "Small") {
                playerType = "Medium";
            }
            if (playerType == "Small") {
                Cmd_EnableSmallShip();
            }
            else if (playerType == "Medium") {
                Cmd_EnableMediumShip();
            }
            else if (playerType == "Large") {
                Cmd_EnableLargeShip();
            }
            CmdTellServerMyShip(playerType);
        }   
        
        /*
        if (!isLocalPlayer) {
            if (playerTypeGlobal == "Small") {
                Cmd_EnableSmallShip();
            }
            else if (playerTypeGlobal == "Medium") {
                Cmd_EnableMediumShip();
            }
            else if (playerTypeGlobal == "Large") {
                Cmd_EnableLargeShip();
            }
        }
        */
	}
	
    void Update() {
        /*
        if (isLocalPlayer) {
            if (playerType == "Small") {
                smallShipPackage.SetActive(true);
                mediumShipPackage.SetActive(false);
                largeShipPackage.SetActive(false);
            }
            else if (playerType == "Medium") {
                smallShipPackage.SetActive(false);
                mediumShipPackage.SetActive(true);
                largeShipPackage.SetActive(false);
            }
            else if (playerType == "Large") {
                smallShipPackage.SetActive(false);
                mediumShipPackage.SetActive(false);
                largeShipPackage.SetActive(true);
            }
        }
        */
        //if (!isLocalPlayer) {
            if (playerTypeGlobal == "Small") {
                playerNetworkSetup.cameraLerpFollow = 5;
                smallShipPackage.SetActive(true);
                mediumShipPackage.SetActive(false);
                largeShipPackage.SetActive(false);
                foreach (Transform child in smallShipPackage.transform){
                    if (child.name == "SmallShip") {
                        playerDeathScript.playerShip = child.gameObject;
                        playerRespawnScript.playerShip = child.gameObject;
                    }
                    else if (child.name == "Engines") {
                        playerDeathScript.engineParent = child.gameObject;
                        playerRespawnScript.engineParent = child.gameObject;
                        int ammount = 0;
                        foreach(Transform engine in child.transform) {
                            ammount++;
                        }
                        playerEngineScript.engines = new ParticleSystem[ammount];

                        int i = 0;
                        foreach (Transform engine in child.transform) {
                            playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                            i++;
                        }
                    }
                }
            }
            else if (playerTypeGlobal == "Medium") {
                playerNetworkSetup.cameraLerpFollow = 2;
                smallShipPackage.SetActive(false);
                mediumShipPackage.SetActive(true);
                largeShipPackage.SetActive(false);
                foreach (Transform child in mediumShipPackage.transform){
                    if (child.name == "MediumShip") {
                        playerDeathScript.playerShip = child.gameObject;
                        playerRespawnScript.playerShip = child.gameObject;
                    }
                    else if (child.name == "Engines") {
                        playerDeathScript.engineParent = child.gameObject;
                        playerRespawnScript.engineParent = child.gameObject;
                        int ammount = 0;
                        foreach(Transform engine in child.transform) {
                            ammount++;
                        }
                        playerEngineScript.engines = new ParticleSystem[ammount];

                        int i = 0;
                        foreach (Transform engine in child.transform) {
                            playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                            i++;
                        }
                    }
                }
            }
            else if (playerTypeGlobal == "Large") {
                playerNetworkSetup.cameraLerpFollow = 1.3F;
                smallShipPackage.SetActive(false);
                mediumShipPackage.SetActive(false);
                largeShipPackage.SetActive(true);
                foreach (Transform child in largeShipPackage .transform){
                    if (child.name == "LargeShip") {
                        playerDeathScript.playerShip = child.gameObject;
                        playerRespawnScript.playerShip = child.gameObject;
                    }
                    else if (child.name == "Engines") {
                        playerDeathScript.engineParent = child.gameObject;
                        playerRespawnScript.engineParent = child.gameObject;
                        int ammount = 0;
                        foreach(Transform engine in child.transform) {
                            ammount++;
                        }
                        playerEngineScript.engines = new ParticleSystem[ammount];

                        int i = 0;
                        foreach (Transform engine in child.transform) {
                            playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                            i++;
                        }
                    }
                }
            }
        //}
    }

    [Command]
    public void CmdTellServerMyShip(string type) {
        playerTypeGlobal = type;
    }

    [Command]
	public void Cmd_EnableSmallShip() {
            playerNetworkSetup.cameraLerpFollow = 5;
            playerHealthScript.maxHealth = 50;
            playerHealthScript.health = 50;
            playerFireScript.fireDelayPrimary = 0.2F;
            playerFireScript.fireDelaySpecial = 20;
            playerFireScript.projectilePrefab = smallShipPrimaryWeapon;
            playerFireScript.specialPrefab = smallShipSpecialWeapon;
            shipControlScript.maxSpeed = 15;
            shipControlScript.speed = 15;
            shipControlScript.maneuverability = 75;
            playerBoostScript.useDelay = 20;
            playerBoostScript.timeAtMax = 2;
            playerBoostScript.maxSpeed = 25;    

            smallShipPackage.SetActive(true);
            mediumShipPackage.SetActive(false);
            largeShipPackage.SetActive(false);
            
            foreach (Transform child in smallShipPackage.transform){
                if (child.name == "MuzzleLocations"){
                    int ammount = 0; 
                    foreach (Transform muzzles in child.transform) {
                        ammount++;
                    }
                    playerFireScript.muzzleLocations = new GameObject[ammount];

                    int i = 0;
                    foreach (Transform muzzles in child.transform) {
                        playerFireScript.muzzleLocations[i] = muzzles.gameObject;
                        i++;
                    }
                }
                else if (child.name == "SpecialLaunchLocation") {
                    playerFireScript.specialLaunchLocation = child.gameObject;
                }
                else if (child.name == "SmallShip") {
                    playerDeathScript.playerShip = child.gameObject;
                    playerRespawnScript.playerShip = child.gameObject;
                }
                else if (child.name == "Engines") {
                    playerDeathScript.engineParent = child.gameObject;
                    playerRespawnScript.engineParent = child.gameObject;
                    int ammount = 0;
                    foreach(Transform engine in child.transform) {
                        ammount++;
                    }
                    playerEngineScript.engines = new ParticleSystem[ammount];

                    int i = 0;
                    foreach (Transform engine in child.transform) {
                        playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
                else if (child.name == "Spotlights") {
                    playerSpotlightScript.spotlight = child.gameObject;
                }
             }
        Rpc_ClientSyncSmall();
    }

    [ClientRpc]
    void Rpc_ClientSyncSmall() {
            playerNetworkSetup.cameraLerpFollow = 5;
            playerHealthScript.maxHealth = 50;
            playerHealthScript.health = 50;
            playerFireScript.fireDelayPrimary = 0.2F;
            playerFireScript.fireDelaySpecial = 20;
            playerFireScript.projectilePrefab = smallShipPrimaryWeapon;
            playerFireScript.specialPrefab = smallShipSpecialWeapon;
            shipControlScript.maxSpeed = 15;
            shipControlScript.speed = 15;
            shipControlScript.maneuverability = 75;
            playerBoostScript.useDelay = 20;
            playerBoostScript.timeAtMax = 2;
            playerBoostScript.maxSpeed = 25;    

            smallShipPackage.SetActive(true);
            mediumShipPackage.SetActive(false);
            largeShipPackage.SetActive(false);

            foreach (Transform child in smallShipPackage.transform){
                if (child.name == "MuzzleLocations"){
                    int ammount = 0; 
                    foreach (Transform muzzles in child.transform) {
                        ammount++;
                    }
                    playerFireScript.muzzleLocations = new GameObject[ammount];

                    int i = 0;
                    foreach (Transform muzzles in child.transform) {
                        playerFireScript.muzzleLocations[i] = muzzles.gameObject;
                        i++;
                    }
                }
                else if (child.name == "SpecialLaunchLocation") {
                    playerFireScript.specialLaunchLocation = child.gameObject;
                }
                else if (child.name == "SmallShip") {
                    playerDeathScript.playerShip = child.gameObject;
                    playerRespawnScript.playerShip = child.gameObject;
                }
                else if (child.name == "Engines") {
                    playerDeathScript.engineParent = child.gameObject;
                    playerRespawnScript.engineParent = child.gameObject;
                    int ammount = 0;
                    foreach(Transform engine in child.transform) {
                        ammount++;
                    }
                    playerEngineScript.engines = new ParticleSystem[ammount];

                    int i = 0;
                    foreach (Transform engine in child.transform) {
                        playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
                else if (child.name == "Spotlights") {
                    playerSpotlightScript.spotlight = child.gameObject;
                }
             }
    }

    [Command]
    public void Cmd_EnableMediumShip() {
            playerNetworkSetup.cameraLerpFollow = 2;
            playerHealthScript.maxHealth = 100;
            playerHealthScript.health = 100;
            playerFireScript.fireDelayPrimary = 0.5F;
            playerFireScript.fireDelaySpecial = 10;
            playerFireScript.projectilePrefab = mediumShipPrimaryWeapon;
            playerFireScript.specialPrefab = mediumShipSpecialWeapon;
            shipControlScript.maxSpeed = 10;
            shipControlScript.speed = 10;
            shipControlScript.maneuverability = 55;
            playerBoostScript.useDelay = 20;
            playerBoostScript.timeAtMax = 3;
            playerBoostScript.maxSpeed = 20;
                
            smallShipPackage.SetActive(false);
            mediumShipPackage.SetActive(true);
            largeShipPackage.SetActive(false);

            foreach (Transform child in mediumShipPackage.transform){
                if (child.name == "MuzzleLocations"){
                    int ammount = 0; 
                    foreach (Transform muzzles in child.transform) {
                        ammount++;
                    }
                    playerFireScript.muzzleLocations = new GameObject[ammount];

                    int i = 0;
                    foreach (Transform muzzles in child.transform) {
                        playerFireScript.muzzleLocations[i] = muzzles.gameObject;
                        i++;
                    }
                }
                else if (child.name == "SpecialLaunchLocation") {
                    playerFireScript.specialLaunchLocation = child.gameObject;
                }
                else if (child.name == "MediumShip") {
                    playerDeathScript.playerShip = child.gameObject;
                    playerRespawnScript.playerShip = child.gameObject;
                }
                else if (child.name == "Engines") {
                    playerDeathScript.engineParent = child.gameObject;
                    playerRespawnScript.engineParent = child.gameObject;
                    int ammount = 0;
                    foreach(Transform engine in child.transform) {
                        ammount++;
                    }
                    playerEngineScript.engines = new ParticleSystem[ammount];

                    int i = 0;
                    foreach (Transform engine in child.transform) {
                        playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
                else if (child.name == "Spotlights") {
                    playerSpotlightScript.spotlight = child.gameObject;
                }
            }
        Rpc_ClientSyncMedium();
    }

    [ClientRpc]
    void Rpc_ClientSyncMedium() {
            playerNetworkSetup.cameraLerpFollow = 2;
            playerHealthScript.maxHealth = 100;
            playerHealthScript.health = 100;
            playerFireScript.fireDelayPrimary = 0.5F;
            playerFireScript.fireDelaySpecial = 10;
            playerFireScript.projectilePrefab = mediumShipPrimaryWeapon;
            playerFireScript.specialPrefab = mediumShipSpecialWeapon;
            shipControlScript.maxSpeed = 10;
            shipControlScript.speed = 10;
            shipControlScript.maneuverability = 55;
            playerBoostScript.useDelay = 20;
            playerBoostScript.timeAtMax = 3;
            playerBoostScript.maxSpeed = 20;
                
            smallShipPackage.SetActive(false);
            mediumShipPackage.SetActive(true);
            largeShipPackage.SetActive(false);

            foreach (Transform child in mediumShipPackage.transform){
                if (child.name == "MuzzleLocations"){
                    int ammount = 0; 
                    foreach (Transform muzzles in child.transform) {
                        ammount++;
                    }
                    playerFireScript.muzzleLocations = new GameObject[ammount];

                    int i = 0;
                    foreach (Transform muzzles in child.transform) {
                        playerFireScript.muzzleLocations[i] = muzzles.gameObject;
                        i++;
                    }
                }
                else if (child.name == "SpecialLaunchLocation") {
                    playerFireScript.specialLaunchLocation = child.gameObject;
                }
                else if (child.name == "MediumShip") {
                    playerDeathScript.playerShip = child.gameObject;
                    playerRespawnScript.playerShip = child.gameObject;
                }
                else if (child.name == "Engines") {
                    playerDeathScript.engineParent = child.gameObject;
                    playerRespawnScript.engineParent = child.gameObject;
                    int ammount = 0;
                    foreach(Transform engine in child.transform) {
                        ammount++;
                    }
                    playerEngineScript.engines = new ParticleSystem[ammount];

                    int i = 0;
                    foreach (Transform engine in child.transform) {
                        playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
                else if (child.name == "Spotlights") {
                    playerSpotlightScript.spotlight = child.gameObject;
                }
            }
    }

    [Command]
    public void Cmd_EnableLargeShip() {
            playerNetworkSetup.cameraLerpFollow = 1.3F;
            playerHealthScript.maxHealth = 250;
            playerHealthScript.health = 250;
            playerFireScript.fireDelayPrimary = 0.7F;
            playerFireScript.fireDelaySpecial = 40;
            playerFireScript.projectilePrefab = largeShipPrimaryWeapon;
            playerFireScript.specialPrefab = largeShipSpecialWeapon;
            shipControlScript.maxSpeed = 7;
            shipControlScript.speed = 7;
            shipControlScript.maneuverability = 45;
            playerBoostScript.useDelay = 20;
            playerBoostScript.timeAtMax = 4;
            playerBoostScript.maxSpeed = 17;
                
            smallShipPackage.SetActive(false);
            mediumShipPackage.SetActive(false);
            largeShipPackage.SetActive(true);

            foreach (Transform child in largeShipPackage.transform){
                if (child.name == "MuzzleLocations"){
                    int ammount = 0; 
                    foreach (Transform muzzles in child.transform) {
                        ammount++;
                    }
                    playerFireScript.muzzleLocations = new GameObject[ammount];

                    int i = 0;
                    foreach (Transform muzzles in child.transform) {
                        playerFireScript.muzzleLocations[i] = muzzles.gameObject;
                        i++;
                    }
                }
                else if (child.name == "SpecialLaunchLocation") {
                    playerFireScript.specialLaunchLocation = child.gameObject;
                }
                else if (child.name == "LargeShip") {
                    playerDeathScript.playerShip = child.gameObject;
                    playerRespawnScript.playerShip = child.gameObject;
                }
                else if (child.name == "Engines") {
                    playerDeathScript.engineParent = child.gameObject;
                    playerRespawnScript.engineParent = child.gameObject;
                    int ammount = 0;
                    foreach(Transform engine in child.transform) {
                        ammount++;
                    }
                    playerEngineScript.engines = new ParticleSystem[ammount];

                    int i = 0;
                    foreach (Transform engine in child.transform) {
                        playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
                else if (child.name == "Spotlights") {
                    playerSpotlightScript.spotlight = child.gameObject;
                }
            }
        Rpc_ClientSyncLarge();
    }
    
    [ClientRpc]
    void Rpc_ClientSyncLarge() {
            playerNetworkSetup.cameraLerpFollow = 1.3F;
            playerHealthScript.maxHealth = 250;
            playerHealthScript.health = 250;
            playerFireScript.fireDelayPrimary = 0.7F;
            playerFireScript.fireDelaySpecial = 40;
            playerFireScript.projectilePrefab = largeShipPrimaryWeapon;
            playerFireScript.specialPrefab = largeShipSpecialWeapon;
            shipControlScript.maxSpeed = 7;
            shipControlScript.speed = 7;
            shipControlScript.maneuverability = 45;
            playerBoostScript.useDelay = 20;
            playerBoostScript.timeAtMax = 4;
            playerBoostScript.maxSpeed = 17;
                
            smallShipPackage.SetActive(false);
            mediumShipPackage.SetActive(false);
            largeShipPackage.SetActive(true);
            
            //playerFireScript.firePrimarySfx = largePrimarySfx;
            //playerFireScript.fireSpecialSfx = nukeLaunchSfx;

            foreach (Transform child in largeShipPackage.transform){
                if (child.name == "MuzzleLocations"){
                    int ammount = 0; 
                    foreach (Transform muzzles in child.transform) {
                        ammount++;
                    }
                    playerFireScript.muzzleLocations = new GameObject[ammount];

                    int i = 0;
                    foreach (Transform muzzles in child.transform) {
                        playerFireScript.muzzleLocations[i] = muzzles.gameObject;
                        i++;
                    }
                }
                else if (child.name == "SpecialLaunchLocation") {
                    playerFireScript.specialLaunchLocation = child.gameObject;
                }
                else if (child.name == "LargeShip") {
                    playerDeathScript.playerShip = child.gameObject;
                    playerRespawnScript.playerShip = child.gameObject;
                }
                else if (child.name == "Engines") {
                    playerDeathScript.engineParent = child.gameObject;
                    playerRespawnScript.engineParent = child.gameObject;
                    int ammount = 0;
                    foreach(Transform engine in child.transform) {
                        ammount++;
                    }
                    playerEngineScript.engines = new ParticleSystem[ammount];

                    int i = 0;
                    foreach (Transform engine in child.transform) {
                        playerEngineScript.engines[i] = engine.gameObject.GetComponent<ParticleSystem>();
                        i++;
                    }
                }
                else if (child.name == "Spotlights") {
                    playerSpotlightScript.spotlight = child.gameObject;
                }
            }
    }

}
