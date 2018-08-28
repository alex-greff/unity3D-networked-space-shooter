using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Spotlight : NetworkBehaviour {
	public GameObject spotlight;
    variables variablesScript;

	// Use this for initialization
	void Start () {
        variablesScript = GetComponent<variables>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
            if (!variablesScript.isTyping && !variablesScript.isDead) {
			    if (Input.GetKeyDown (KeyCode.F)) {
				    variablesScript.spotlightOn = !variablesScript.spotlightOn;
			    }

			    if (variablesScript.spotlightOn == true) {
				    DisableSpotLight ();
			    } 
			    else if (variablesScript.spotlightOn == false) {
				    EnableSpotLight ();	
			    }
            }
            else if (variablesScript.isDead) {
                DisableSpotLight();
            }
		}
	}

	void DisableSpotLight() {
        if (spotlight != null) 
		spotlight.SetActive (true);
	}
	void EnableSpotLight(){
        if (spotlight != null) 
		spotlight.SetActive (false);
	}
}
