using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomManager : MonoBehaviour {
    public InputField address;
    public GameObject helpParent;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void HostandJoinNewGame () {
        NetworkManager.singleton.StartHost(); //Start the game as host
        
    }

    public void HostNewGame() {
        NetworkManager.singleton.StartServer(); //Start the game as server
    }

    public void JoinGame () {
        address = GameObject.Find("Address").GetComponent<InputField>(); //Get the inputbox's text for the ip
		NetworkManager.singleton.networkAddress = address.text; //Set the network address
		NetworkManager.singleton.StartClient(); //Start
    }

    public void ToggleHelp() {
        if (helpParent.activeInHierarchy) {
            helpParent.SetActive(false);
        }
        else {
            helpParent.SetActive(true);
        }
    }

    public void Exit() {
        Application.Quit();
    }
}
