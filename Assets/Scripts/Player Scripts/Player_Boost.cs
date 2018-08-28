using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(Audio_Sync))]
public class Player_Boost : NetworkBehaviour {
    ShipControl shipControlScript;
    variables variablesScript;
    bool inBoost;
    float speed;
    float originalSpeed;
    public float useDelay;
    float cooldown;

    Audio_Sync audioSync;

    public float timeAtMax;
    float timeAtMaxCounter;


    public float maxSpeed;

	// Use this for initialization
	void Start () {
        shipControlScript = GetComponent<ShipControl>();
        variablesScript = GetComponent<variables>();
        audioSync = GetComponent<Audio_Sync>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (inBoost && isLocalPlayer) {
            if (timeAtMaxCounter >= Time.time) { //If the player has reached max acceleration
                speed = maxSpeed; //Go at full throttle
            }

            if (timeAtMaxCounter <= Time.time) { //If the boost has ended
                inBoost = false;
                variablesScript.isBoosting = false;
            }

            shipControlScript.speed = speed; //Set the ship's speed
        }
	}

    public bool canBoost() { //Call this function to check if a boost can be used
        if (cooldown <= Time.time) {
            return true;
        }
        else {
            return false;
        }
    }

    public void RunBoost(float currentSpeed) {
        cooldown = useDelay + Time.time; //Set the timer for the next use
        originalSpeed = currentSpeed; //Store the original speed the player was at
        timeAtMaxCounter = timeAtMax + Time.time; //Set the timer for the boost duration

        GetComponent<Player_GUI>().StartBoostProgress();
        //source.PlayOneShot(boostSfx); //Play boost sfx
        audioSync.PlaySound(9, transform.position); //Play boost sfx
        variablesScript.isBoosting = true;
        inBoost = true;
    }
}
