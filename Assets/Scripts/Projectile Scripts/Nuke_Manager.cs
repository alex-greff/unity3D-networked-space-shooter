using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Nuke_Manager : NetworkBehaviour {
    public float speed;
    public int impactDamage;
    public int firstDegreeDamage;
    public int secondDegreeDamage;
    public float aliveRange;

    public GameObject explosionPrefab;
    Rigidbody nuke;
    public GameObject Shooter;
    public GameObject hit_prefab;

    AudioSource source;
    public AudioClip explosionSfx;

    Vector3 startPos;

    public GameObject smokePrefab;
    public GameObject smokeTrail;

	// Use this for initialization
	void Start () {
        nuke = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();

        if (smokePrefab != null) {
            //Create the nuke's smoke
            smokeTrail = (GameObject)Instantiate(smokePrefab, transform.position, transform.rotation);
            smokeTrail.GetComponent<FollowScript>().target = gameObject; //Set the smoke trail's target to this missile
        }

        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(nuke == null)
        return;

        //nuke.velocity = transform.forward * speed;
        transform.Translate (new Vector3 (0, 0, speed * Time.deltaTime));
        
        //Self destruct script to make sure the nuke doesnt keep going on forever
        if (aliveRange > 0){ //If over 0 then activate option
			//Destroy the projectile when the distance is greater than the aliveRange
			if (Vector3.Distance (startPos, transform.position) > aliveRange) {
                Detonate();
			}
		}

	}

    public void Detonate() {
        if (explosionPrefab != null) {
            GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 5F);
            NetworkServer.Spawn(explosion);
        }

        Player_Fire shooterFireScript = Shooter.GetComponent<Player_Fire>();
        shooterFireScript.currentNuke = null; //Remove the current nuke

        CheckForPlayers();
        if(smokeTrail != null) { //UNITY 5.1.3 has problems with particle systems so I bult this failsafe just in case I disable them
            smokeTrail.GetComponent<ParticleSystem>().startSize = 0;
            Destroy(smokeTrail, 5F);
        }
        print("Playing nuke sfx");
        //source.PlayOneShot(explosionSfx); //Play explosion sound
        Shooter.GetComponent<Audio_Sync>().PlaySound(8, transform.position); //Play nuke explosiion sfx via the shooter
        //Shooter.GetComponent<Projectile_daisyChain>().CmdDestroy(gameObject);
        Destroy(gameObject);
        //NetworkServer.Destroy(gameObject);
    }

    

    void CheckForPlayers() {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            //if (go != Shooter) { //Make sure the missile doesnt track the player who shot it
                //print("Target Name: " + go.transform.name);
                float diff = (go.transform.position - transform.position).sqrMagnitude;                
                //print("Difference: " + diff + " Shooter:" + go.transform.name);

                if (diff <= 400) { //If the player is in the direct blast range
                    go.GetComponent<Player_Health>().DeductHealth(firstDegreeDamage);
                }
                else if (diff > 400 & diff <= 1000) { //If the player is in the 2nd range
                    go.GetComponent<Player_Health>().DeductHealth(secondDegreeDamage);
                }
            //}
        }
    }

    //Collisions
	void OnCollisionEnter(Collision col){
		//If projectile hit a player
		if (col.gameObject.tag == "Player") {
			//print ("The projectile hit player: " + col.transform.name);
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, impactDamage, "Player", gameObject, Shooter); //Run function in the daisychain
            if (col.gameObject != Shooter) {
                Detonate();
                //Destroy(gameObject);
            }
		}
		//If projectile hit an asteroid
		if (col.gameObject.tag == "Asteroid"){
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, impactDamage, "Asteroid", gameObject, Shooter); //Run function in the daisychain
            Detonate();
            //Destroy(gameObject);
		}
    }
}
