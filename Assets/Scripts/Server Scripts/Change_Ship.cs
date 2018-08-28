using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Change_Ship : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void NeutralAllBut (int index) { //This function sets all all objects in the array to the neutral color but the specified
        /*
        for (int i=0; i < ShipSelectButtons.Length; i++) {
            if (i == index) {
                ShipSelectButtons[i].GetComponent<Image>().color = activeColor; //Set to active color
            }
            else {
                ShipSelectButtons[i].GetComponent<Image>().color = neutralColor; //Set to neutral color
            }
        }
        */
    }
}
