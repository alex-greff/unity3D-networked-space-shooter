using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent (typeof(Audio_Sync))]
public class Projectile_daisyChain : NetworkBehaviour {
	
	public GameObject Shooter;
    Audio_Sync audioSync;
	//public GameObject hit_prefab;
	
	//public bool[] projectileTaken;


	// Use this for initialization
	void Start () {
        audioSync = GetComponent<Audio_Sync>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	//This command is ran from the Projectile_Manager script because only player gameObjects can run server commands -_-
	[Command]
	public void CmdTellServerWhoWasShot (GameObject uniqueID, int dmg, string type, GameObject projectile, GameObject shooter){
		//If player was hit
		if (type == "Player"){
			if (uniqueID.transform.name != Shooter.transform.name){ //Make sure the collision isn't with the shooter
				print ("The projectile hit: " + uniqueID);
				uniqueID.GetComponent<Player_Health> ().DeductHealth (dmg); //Apply Damage to that player
                uniqueID.GetComponent<Player_Health>().lastHitBy = shooter;
                string identity = projectile.GetComponent<Projectile_Identity>().identity;
                if (identity == "Projectile") {
                    audioSync.PlaySound(10, projectile.transform.position);
                }
                else if (identity == "Missile") {
                    audioSync.PlaySound(6, projectile.transform.position);
                }
                else if (identity == "Mine") {
                    audioSync.PlaySound(7, projectile.transform.position);
                }
                else if (identity == "Nuke") {
                    //Play no sound
                }
                
				CmdDestroyProjectile(projectile); //Run function that destroys projectile
			}
		}
		if (type == "Asteroid"){
			//TODO: deduct health from that asteroid
			print ("The projectile hit: " + uniqueID.transform.name);

            Shooter.GetComponent<Player_Health>().DeductHealth(1); //TEMPORARY


			CmdDestroyProjectile(projectile); //Run function that destroys projectile

		}
	}

	//[Command]
	void CmdDestroyProjectile(GameObject go){
		//TODO: actually make this despawn it server wide
		if (go != null){
            string identity = go.GetComponent<Projectile_Identity>().identity;
            //print(go.transform.name + "is supposed to be destroyed!");
            if (identity == "Projectile") {
			    GameObject hit_prefab = go.GetComponent<Projectile_Manager>().hit_prefab; //Import the hit prefab from the specific projectile's Projectile_Manager script
                GameObject hit = (GameObject)Instantiate(hit_prefab, go.transform.position, go.transform.rotation); //Instantiate the hit effect
			    NetworkServer.Spawn (hit); //Spawn the hit effect server wide
                Destroy (hit, 3F); //Destroy the hit effect after 3 seconds

                //NetworkServer.Destroy(go); //Destroy the projectile on the server
			    //Destroy(go); //Destroy the projectile locally (not sure if needed)
            }
            else if (identity == "Missile") {
                print("Destroying missile");
                //go.GetComponent<Missile_Manager>().DestroyMissile();
                //NetworkServer.Destroy(go); //Destroy the missile
	            //Destroy(go);
            }
            else if (identity == "Mine") {
                //go.GetComponent<Mine_Manager>().DestroyMine();
                //NetworkServer.Destroy(go); //Destroy the missile
	            //Destroy(go);
            }
            else if (identity == "Nuke") {
                //go.GetComponent<Nuke_Manager>().Detonate();
                //NetworkServer.Destroy(go); //Destroy the missile
	            //Destroy(go);
            }
            else { //Just in case
                //NetworkServer.Destroy(go); 
			    //Destroy(go); 
            }

            //NetworkServer.Destroy(go);
            //Destroy(go);
            //Network.Destroy(go);
		}
	}

    [Command]
    public void CmdDestroy(GameObject go) { //Call this whenever external destroying is needed for a projectile
        NetworkServer.Destroy(go); 
        //Network.Destroy(go);
	    //Destroy(go); 
    }
}
