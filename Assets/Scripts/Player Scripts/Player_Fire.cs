using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(Audio_Sync))]

public class Player_Fire : NetworkBehaviour {
    private Audio_Sync audioSync;

	variables variablesScript;
    Player_GUI playerGUIScript;
	public GameObject projectilePrefab;
    
    string SpecialType;
    public GameObject specialPrefab;
	[SerializeField] private Transform playerTransform;

    public GameObject currentNuke;
	//[SerializeField] private Transform shipTransform;

	public GameObject[] muzzleLocations;
    public GameObject specialLaunchLocation;

	public float fireDelayPrimary;
	float cooldownPrimary;

    public float fireDelaySpecial;
    float cooldownSpecial;

	// Use this for initialization
	void Start () {
        audioSync = GetComponent<Audio_Sync>();
		variablesScript = GetComponent<variables>();
        playerGUIScript = GetComponent<Player_GUI>();
        //fireDelay = variablesScript.fireDelay;
	}
	
	// Update is called once per frame
	void Update () {

	//	CheckIfShooting ();
		if (!variablesScript.isDead && !variablesScript.isTyping && !variablesScript.isPaused){ //If player is not dead, typing, or paused
			if (!isLocalPlayer) { //If not the local player then don't go any further
				return;
			}
			if (Input.GetKey(KeyCode.Mouse0)){
                if (cooldownPrimary <= Time.time && isLocalPlayer){ //Cooldown check
                    Vector3[] muzzleLocs = new Vector3[muzzleLocations.Length];
                    Quaternion[] muzzleRots = new Quaternion[muzzleLocations.Length];

                    for (int i = 0; i < muzzleLocations.Length; i++) {
                        muzzleLocs[i] = muzzleLocations[i].transform.position;
                        muzzleRots[i] = muzzleLocations[i].transform.rotation;
                    }
                    
				    Cmd_ShootPrimary(muzzleLocs, muzzleRots, gameObject);

                    cooldownPrimary = Time.time + fireDelayPrimary; //Put delay on firing speed
                    playerGUIScript.StartPrimaryFireProgress(); //Start the graphical cooldown on the player's screen
                    //print("Ran shoot cmd");
                    //source.PlayOneShot(firePrimarySfx);
                    GameObject shipType = GetComponent<Player_Respawn>().playerShip;
                        if (shipType.transform.name.Contains("Small")){
                            audioSync.PlaySound(0, transform.position);
                        }
                        else if (shipType.transform.name.Contains("Medium")) {
                            audioSync.PlaySound(1, transform.position);
                        }
                        else if (shipType.transform.name.Contains("Large")) {
                            audioSync.PlaySound(2, transform.position);
                        }
                }
			}
            if (Input.GetKeyDown(KeyCode.S)) {
                if (currentNuke == null) {
                    //print("Shooting");
                    if (cooldownSpecial <= Time.time && isLocalPlayer) {
                        Vector3 specialLaunchLoc = specialLaunchLocation.transform.position;
                        Quaternion specialLaunchRot = specialLaunchLocation.transform.rotation;
                        
                        Cmd_ShootSpecial(specialLaunchLoc, specialLaunchRot, gameObject);
                        
                        cooldownSpecial = Time.time + fireDelaySpecial;
                        playerGUIScript.StartSpecialFireProgress(); //Start the graphical cooldown on the player's screen
                        //source.PlayOneShot(fireSpecialSfx);
                        GameObject shipType = GetComponent<Player_Respawn>().playerShip;
                        if (shipType.transform.name.Contains("Small")){
                            audioSync.PlaySound(3, transform.position);
                        }
                        else if (shipType.transform.name.Contains("Medium")) {
                            audioSync.PlaySound(4, transform.position);
                        }
                        else if (shipType.transform.name.Contains("Large")) {
                            audioSync.PlaySound(5, transform.position);
                        }
                    }
                }
                else if (currentNuke != null){
                    //print("Detonating Nuke");
                    DetonateNuke();
                }
            }
		}
	}

	//void CheckIfShooting(){
	//	if (!isLocalPlayer) {
	//		return;
	//	}
	//	if (Input.GetKeyDown(KeyCode.Mouse0)){
	//		Shoot();
	//	}
	//}
    void DetonateNuke() {
        currentNuke.GetComponent<Nuke_Manager>().Detonate();
    }

    [Command]
    public void Cmd_ShootSpecial(Vector3 pos, Quaternion rot, GameObject shooter) {
        print("shooting special");
        GameObject special = (GameObject)Instantiate(specialPrefab, pos, rot);
        SpecialType = special.GetComponent<Projectile_Identity>().identity;
        NetworkServer.Spawn(special);
        //Rpc_SpawnSound(fireSpecialSfx, source); //Play sound effect
        if (SpecialType == "Missile") {
            special.GetComponent<Missile_Manager>().Shooter = shooter;
            Rpc_SyncShooterMissile(special, shooter);
        }
        else if (SpecialType == "Mine") {
            special.GetComponent<Mine_Manager>().Shooter = shooter;
            Rpc_SyncShooterMine(special, shooter);
        }
        else if (SpecialType == "Nuke") {
            //print("Assigning nuke to placeholder");
            shooter.GetComponent<Player_Fire>().currentNuke = special; //Set current nuke to the current launched one
            special.GetComponent<Nuke_Manager>().Shooter = shooter;  
            Rpc_SyncShooterNuke(special, shooter);    
        }
    }

    //[ClientRpc]
    //void Rpc_SpawnSound(AudioClip snd, AudioSource src) {
    //    src.PlayOneShot(snd);
    //}

    [Command]
	public void Cmd_ShootPrimary(Vector3[] pos, Quaternion[] rot, GameObject shooter){
        /*
		for (int i=0; i<muzzleLocs.Length; i++){
            print("Spawning projectile");
			//print ("MuzzleLocation["+i+"].transform.position = " + muzzleLocations[i].transform.position +" \nMuzzleLocation["+i+"].transform.rotation = "+muzzleLocations[i].transform.position);
			GameObject projectile = (GameObject)Instantiate(projectilePrefab, muzzleLocs[i].transform.position, muzzleLocs[i].transform.rotation);
			projectile.GetComponent<Projectile_Manager>().Shooter = shooter; //Set it to this gameobject
			NetworkServer.Spawn (projectile);
	    }
        */
        //Rpc_SpawnSound(firePrimarySfx, source); //Play sound effect

        for (int i=0; i<pos.Length; i++){
            GameObject projectile = (GameObject)Instantiate(projectilePrefab, pos[i], rot[i]);
	        
		    NetworkServer.Spawn (projectile);
            Rpc_SyncShooterProjectile(projectile, shooter);
        }
	}

    [ClientRpc]
    void Rpc_SyncShooterMissile(GameObject projectile, GameObject shooter) {
        projectile.GetComponent<Missile_Manager>().Shooter = shooter;
    }

    [ClientRpc]
    void Rpc_SyncShooterMine(GameObject projectile, GameObject shooter) {
        projectile.GetComponent<Mine_Manager>().Shooter = shooter;
    }

    [ClientRpc]
    void Rpc_SyncShooterNuke(GameObject projectile, GameObject shooter) {
        projectile.GetComponent<Nuke_Manager>().Shooter = shooter;
    }

    [ClientRpc]
    void Rpc_SyncShooterProjectile(GameObject projectile, GameObject shooter) {
        projectile.GetComponent<Projectile_Manager>().Shooter = shooter; //Set it to this gameobject
    }
}
