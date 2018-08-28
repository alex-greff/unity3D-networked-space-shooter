using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Health : NetworkBehaviour {
    public int maxHealth;
	[SyncVar(hook = "OnHealthChanged")]public int health;
	private Text healthText;
    private Image healthBar;
	variables variablesScript;
	public bool shouldDie = false;
	public bool isDead = false;

	public delegate void DieDelegate();
	public event DieDelegate EventDie;

	public delegate void RespawnDelegate();
	public event RespawnDelegate EventRespawn;

	Vector3 randomSpin;

    Player_GUI playerGUIScript;

    public GameObject lastHitBy;

	// Use this for initialization
	void Start () {
		healthText = GameObject.Find ("Health Text").GetComponent<Text> ();
        healthBar = GameObject.Find("Health Bar").GetComponent<Image>();
        playerGUIScript = GetComponent<Player_GUI>();
		variablesScript = GetComponent<variables>();

		playerGUIScript.SetHealthText ();

		randomSpin = new Vector3(Random.Range(20F,40F),Random.Range(20F,40F),Random.Range(20F,40F));
        health = maxHealth;

        /*
        if (isLocalPlayer) {
            healthText.text = health + "";
            healthBar.fillAmount = 1;
        }
        */
	}
	
	// Update is called once per frame
	void Update () {
		//if (variablesScript.isDead){
			//transform.Rotate(randomSpin*Time.deltaTime); //Make the ship spin out of control
			//transform.Translate(new Vector3(0,0,variablesScript.maxSpeed* Time.deltaTime));
		//}

		//CheckCondition();
        //if (variablesScript.isDead) {
        //    GetComponent<Player_Death>().HidePlayer();
        //}
        //else if (!variablesScript.isDead) {

        //} 

        if (health <= 0 && shouldDie == false && isDead == false) {
            shouldDie = true;
            //print("Setting shouldDie to true");
        }
        //print("Should die: " + shouldDie + " isDead: " + isDead + " health: " + health);
        if (health <= 0 && shouldDie == true) {
            //print("Killing player");
            //GetComponent<Player_Death>().DisablePlayer();
            if (isLocalPlayer) {
                Screen.lockCursor = false;
            }
            EventDie();

            shouldDie = false;
            isDead = true;
        }
        if (health > 0 && isDead == true) {
            //print("Respawning player");
            EventRespawn();
        }

        
        /*
        if (health <= 0 && !shouldDie && !variablesScript.isDead){
			shouldDie = true;
		}
		if (health <= 0 && shouldDie){
            print("Killing player");
			//if (EventDie != null){
				//EventDie();
			//}
            GetComponent<Player_Death>().DisablePlayer();
            
			shouldDie = false;
		}

		if (health > 0 && isDead){
            
			//if(EventRespawn != null){
				//EventRespawn();

			//}
            GetComponent<Player_Respawn>().EnablePlayer();
			isDead = false;
		}
        */
	}
    
    /*
	void CheckCondition(){
		if (health <= 0 && !shouldDie && !variablesScript.isDead){
			shouldDie = true;
		}
		if (health <= 0 && shouldDie){
            print("Killing player");
			//if (EventDie != null){
				EventDie();
			//}
            GetComponent<Player_Death>().DisablePlayer();
            
			shouldDie = false;
		}

		if (health > 0 && isDead){
            
			//if(EventRespawn != null){
				EventRespawn();

			//}
            GetComponent<Player_Respawn>().EnablePlayer();
			isDead = false;
		}
	}
    */
	

	public void DeductHealth (int dmg){
		health -= dmg;
	}

	void OnHealthChanged(int hlth){
		health = hlth;
        if (playerGUIScript != null) {
		    playerGUIScript.SetHealthText ();
        }
	}

	public void ResetHealth(){
		health = maxHealth;
	}

	//IEnumerator DestroyShip(){
	//	yield return new WaitForSeconds(0.5f);
	//}

}
