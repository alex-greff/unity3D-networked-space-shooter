using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_ID : NetworkBehaviour {
	[SyncVar]public string playerUniqueIdentity; //Synced accross the whole server whenever it's changed
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

    public NetworkConnection playerConnection;
    public short playerControllerID;

	public override void OnStartLocalPlayer(){ //When the player starts up
        string playerPrefName = "";
        if (isLocalPlayer) {
            playerPrefName = PlayerPrefs.GetString("Name");
            playerPrefName = playerPrefName.Trim();
            if (playerPrefName == "Player" || playerPrefName == "" || playerPrefName == "Player(Clone)") {
                GetNetIdentity ();
            }
            else {
                OverrideName(playerPrefName);
            }
        }
        SetIdentity();
	}
    
	void SetIdentity(){
        /*
		if (!isLocalPlayer) {
			myTransform.name = playerUniqueIdentity; //Set the name of the player to all clients connected to the server
		} 
		else {
			myTransform.name = MakeUniqueIdentity(); //Create a name for the player
		}
        */
        myTransform.name = playerUniqueIdentity; //Set the name of the player to all clients connected to the server
	}

	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}

	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") { //If the name hasn't been set yet
			SetIdentity();
		}
	}

	[Client]
	void GetNetIdentity(){ //Sets the unique identity of the player to the playerNetID variable
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyName (MakeUniqueIdentity()); 
	}


	string MakeUniqueIdentity (){ 
        if (playerUniqueIdentity == "Player" || playerUniqueIdentity == "" || playerUniqueIdentity == "Player(Clone)") {
            string uniqueName = "Player " + playerNetID.ToString (); //The ID of the player 
		    return uniqueName;
        }
        else {
            return playerUniqueIdentity;
        }
	}

    public void OverrideName(string name) {
        CmdTellServerMyName(name);
    }

	[Command]
	void CmdTellServerMyName(string name){
		playerUniqueIdentity = name;
	}
}
