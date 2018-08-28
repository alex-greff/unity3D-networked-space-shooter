using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Button_NewShip : NetworkBehaviour {
	GameObject newShipButton;
	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			//newShipButton = GameObject.Find("GameManager").GetComponent<GameManager_References>().newShipButton; //Get the saved reference of the repsawn button
			newShipButton.GetComponent<Button>().onClick.AddListener(NewShip); //Add a listener to the button using script. Same as pressing + in the button
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewShip(){
		if (isLocalPlayer) {
			NetworkIdentity ni = GetComponent<NetworkIdentity>();
			Player_ID id = GetComponent<Player_ID> ();

			GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
			int r = Random.Range(0,spawnPoint.Length);

			GameObject newShip = (GameObject) Instantiate (playerPrefab, spawnPoint [r].transform.position, spawnPoint [r].transform.rotation);
			NetworkIdentity newNi = newShip.GetComponent<NetworkIdentity> ();
			newNi = ni;
			Player_ID newId = newShip.GetComponent<Player_ID> ();
			newId = id;

			NetworkServer.Destroy (gameObject);
		}
	}
}
