using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_GUI : NetworkBehaviour {
    Player_Health playerHealthScript;
    variables variablesScript;
    Player_Fire playerFireScript;
    Player_Boost playerBoostScript;

    Text healthText;
    Image healthBar;

    Text fireText;
    Image fireBar;

    Text specialText;
    Image specialBar;

    Text boostText;
    Image boostBar;

    bool inPrimaryFireCooldown = false;
    bool inSpecailFireCooldown = false;
    bool inBoostCooldown = false;

	// Use this for initialization
	void Start () {
        playerHealthScript = GetComponent<Player_Health>();
        variablesScript = GetComponent<variables>();
        playerFireScript = GetComponent<Player_Fire>();
        playerBoostScript = GetComponent<Player_Boost>();

        healthText = GameObject.Find ("Health Text").GetComponent<Text> ();
        healthBar = GameObject.Find("Health Bar").GetComponent<Image>();
        fireText = GameObject.Find("Fire Text").GetComponent<Text>();
        fireBar = GameObject.Find("Fire Bar").GetComponent<Image>();
        specialText = GameObject.Find("Special Text").GetComponent<Text>();
        specialBar = GameObject.Find("Special Bar").GetComponent<Image>();
        boostText = GameObject.Find("Boost Text").GetComponent<Text>();
        boostBar = GameObject.Find("Boost Bar").GetComponent<Image>();


        healthText.text = playerHealthScript.health.ToString();
        healthBar.fillAmount = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            /*

            //float boostPercent = Mathf.Clamp(Time.time / playerBoostScript.cooldown, 0, 1);
            float boostPercent = Time.time / playerBoostScript.cooldown;
            boostBar.fillAmount = boostPercent;
            boostText.text = Mathf.Round(boostPercent * 100) +"%";
            */
            //healthText.text = ""+playerHealthScript.health;
            
            //healthBar.fillAmount = playerHealthScript.maxHealth / playerHealthScript.health;

            if (inPrimaryFireCooldown) {
                if (isLocalPlayer) {
                    float fps = 1.0F/Time.deltaTime;
                    float totalFrames = fps * playerFireScript.fireDelayPrimary;
                    float ammount = 1 / totalFrames;
               
                    fireBar.fillAmount += ammount;
                    fireText.text = Mathf.Round(fireBar.fillAmount * 100) + "%";
                    if (fireBar.fillAmount == 1) {
                        inPrimaryFireCooldown = false;
                    }
                }
            }
            if (inSpecailFireCooldown) {
                if (isLocalPlayer) {
                    float fps = 1.0F/Time.deltaTime;
                    float totalFrames = fps * playerFireScript.fireDelaySpecial;
                    float ammount = 1 / totalFrames;

                    specialBar.fillAmount += ammount;
                    specialText.text = Mathf.Round(specialBar.fillAmount * 100) + "%";
                    if (specialBar.fillAmount == 1) {
                        inSpecailFireCooldown = false;
                    }
                }
            }
            if (inBoostCooldown) {
                if (isLocalPlayer) {
                    float fps = 1.0F/Time.deltaTime;
                    float totalFrames = fps * playerBoostScript.useDelay;
                    float ammount = 1 / totalFrames;

                    boostBar.fillAmount += ammount;
                    boostText.text = Mathf.Round(boostBar.fillAmount * 100) + "%";
                    if (boostBar.fillAmount == 1) {
                        inBoostCooldown = false;
                    }
                }
            }
        }

	}
    public void StartPrimaryFireProgress () {
        if (isLocalPlayer) {
            fireBar.fillAmount = 0;
            inPrimaryFireCooldown = true;
        }
    }
    public void StartSpecialFireProgress () {
        if (isLocalPlayer) {
            specialBar.fillAmount = 0;
            inSpecailFireCooldown = true;
        }
    }
    public void StartBoostProgress () {
        if (isLocalPlayer) {
            boostBar.fillAmount = 0;
            inBoostCooldown = true;
        }
    }
    

    public void SetHealthText(){
		if (isLocalPlayer) {
            float percentOff;
            if (playerHealthScript != null) {
			    if (playerHealthScript.health <= 0){
				    playerHealthScript.health = 0;
				    healthText.text = "0";
                    healthBar.fillAmount = 0;
			    }
			    else {
				    healthText.text = playerHealthScript.health.ToString();
                
                    float health = playerHealthScript.health; 
                    float maxhealth = playerHealthScript.maxHealth;
                    percentOff = health/maxhealth;

                    //print(playerHealthScript.health + "/" +playerHealthScript.maxHealth + "=" + percentOff);
                    healthBar.fillAmount = percentOff;
			    }
            }
		}
	}
}
