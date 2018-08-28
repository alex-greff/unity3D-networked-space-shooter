using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent (typeof(Audio_Sync))]
public class Player_Death : NetworkBehaviour {
    Audio_Sync audioSync;
    Player_Pause playerPauseScirpt;

	Player_Health healthScript;
	public GameObject targetCrosshair;
	public GameObject playerShip;
    public GameObject explosionPrefab;

	public GameObject engineParent;
    public GameObject playerTrail;
	variables variablesScript;
    Player_Chat playerChatScript;
    Player_ID playerIDScript;


	// Use this for initialization
	void Start () {
        audioSync = GetComponent<Audio_Sync>();
        playerPauseScirpt = GetComponent<Player_Pause>();
		//engineParent = transform.Find("Engines").gameObject;
		healthScript = GetComponent<Player_Health>();
		variablesScript = GetComponent<variables>();
        playerChatScript = GetComponent<Player_Chat>();
        playerIDScript = GetComponent<Player_ID>();

		healthScript.EventDie += DisablePlayer; //Subscribes it to the EventDie
	}

	void OnDisable(){
        //if (healthScript != null)
		//healthScript.EventDie -= DisablePlayer; //Ubsubcribe the event to prevent memory leaks (not sure if needed)
	}

	void OnEnable(){
        //if (healthScript != null)
		//healthScript.EventDie += DisablePlayer;
	}

    public void HidePlayer() {
        playerShip.SetActive(false); //Hide the ship parent to hide all the visual/physical components colliders, meshes, etc.
		engineParent.SetActive(false); //Disable the engines
        playerTrail.SetActive(false);
    }

	public void DisablePlayer(){
        print("Disabling " + transform.name);
        playerShip.SetActive(false); //Hide the ship parent to hide all the visual/physical components colliders, meshes, etc.
		engineParent.SetActive(false); //Disable the engines
        playerTrail.SetActive(false);

		GetComponent<Player_Radar>().DisableRadar(); //Call the player radar script and disable the radar icons

        GameObject lastHitter = healthScript.lastHitBy; //Get the person who last hit the player
        if (lastHitter != null) { //If the last hitter exist
            //playerChatScript.Cmd_UpdateChat(playerIDScript.playerUniqueIdentity + " was killed by " + lastHitter.transform.name);
            lastHitter.GetComponent<variables>().kills++; //Add one to the killer's kills

        }
        else if (lastHitter == null) {
            //playerChatScript.Cmd_UpdateChat(playerIDScript.playerUniqueIdentity + " died");
        }

		variablesScript.isDead = true;
		healthScript.isDead = true;

        

		if (isLocalPlayer){
            print("Showing lock cursor");
            Screen.lockCursor = false;
			targetCrosshair.SetActive(false); //Hide the crosshair
			GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton.SetActive(true); //Enable the repsawn button
            GameObject.Find("GameManager").GetComponent<GameManager_References>().quitButton.SetActive(true); //Enable the repsawn button
            GameObject.Find("GameManager").GetComponent<GameManager_References>().changeShipButton.SetActive(true); //Enable the change ship button    
            audioSync.PlaySound(11, transform.position); //Play ship explosion sound
            playerPauseScirpt.Resume();
		}


        if (isServer) {
            print("Print Spawning Explosion");
            Cmd_SpawnExplosion(); //Spawn the graphical explosion
        }
	}

    
    [Command]
    void Cmd_SpawnExplosion() {
        if (explosionPrefab != null) {
            GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation); //Instantiate the explosion at the player's position
            NetworkServer.Spawn(explosion); //Spawn explosion on server
            Destroy(explosion, 5F); //Destroy the explosion gameobject when it's done
        }
    }
}
