using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuManager_control : MonoBehaviour {
    public Color neutralColor;
    public Color activeColor;

    public GameObject SmallShip;
    public GameObject MediumShip;
    public GameObject LargeShip;
    public GameObject[] ShipSelectButtons;

	// Use this for initialization
	void Start () {
        InputField address = GameObject.Find("Address").GetComponent<InputField>(); //Get the inputbox's text for the ip
        string ip = Network.player.ipAddress; //Get the ip
        address.text = ip; //Set the ip in the inputbox

	    //NetworkManager.singleton.playerPrefab = MediumShip; //Default ship is medium

        if (PlayerPrefs.GetString("ShipType") == "Small") { //If its the small ship
            //NetworkManager.singleton.playerPrefab = SmallShip; //Set the networkmanager's player spawn prefab to the small ship
            NeutralAllBut(0);
        }
        else if (PlayerPrefs.GetString("ShipType") == "Large") { //If its the large ship
            //NetworkManager.singleton.playerPrefab = LargeShip; //Set the networkmanager's player spawn prefab to the small ship
            NeutralAllBut(2);
        }
        else { //If player pref doesnt exist yet or is comprimized default is medium ship
            //NetworkManager.singleton.playerPrefab = MediumShip; //Set the networkmanager's player spawn prefab to the small ship
            NeutralAllBut(1);
        }

        SetName();
	}

    public void SetName() {
        InputField name = GameObject.Find("Name").GetComponent<InputField>();
        string customName = name.text;
        PlayerPrefs.SetString("Name", customName);
    }

    void SetDefaultShip (string type) { //Sets the default ship
        PlayerPrefs.SetString("ShipType", type);
    }
	
	public void ChangeShip(string type) { //Called when the corrisponding button is pressed
        if (type == "Small") {
            //NetworkManager.singleton.playerPrefab = SmallShip; //Set the networkmanager's player spawn prefab to the small ship
            NeutralAllBut(0);
            SetDefaultShip(type);
        }
        else if (type == "Medium") {
            //NetworkManager.singleton.playerPrefab = MediumShip;
            NeutralAllBut(1);
            SetDefaultShip(type);
        }
        else if (type == "Large") {
            //NetworkManager.singleton.playerPrefab = LargeShip;
            NeutralAllBut(2);
            SetDefaultShip(type);
        }
    }

    void NeutralAllBut (int index) { //This function sets all all objects in the array to the neutral color but the specified
        for (int i=0; i < ShipSelectButtons.Length; i++) {
            if (i == index) {
                ShipSelectButtons[i].GetComponent<Image>().color = activeColor; //Set to active color
            }
            else {
                ShipSelectButtons[i].GetComponent<Image>().color = neutralColor; //Set to neutral color
            }
        }
    }
}
