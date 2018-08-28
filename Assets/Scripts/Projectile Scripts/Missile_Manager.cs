using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Missile_Manager : NetworkBehaviour {
    public float speed;
    public int damage;
    public float aliveRange;
    public float range;

    Rigidbody missile;
    public GameObject Shooter;
    Transform target;
    public GameObject hit_prefab;
    
    float delayBeforeTargetting;

    public GameObject smokePrefab;
    public GameObject smokeTrail;

    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position; //Set the start pos

        missile = GetComponent<Rigidbody>();
        Fire();
        delayBeforeTargetting = 1 + Time.time; //Have a 1 sec delay before the missile targets anyone

        if (smokePrefab != null) {
            //Create the missle smoke
            smokeTrail = (GameObject)Instantiate(smokePrefab, transform.position, transform.rotation);
            smokeTrail.GetComponent<FollowScript>().target = gameObject; //Set the smoke trail's target to this missile
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Destroy the missile after it has traveled a set distance
		if (aliveRange > 0){ //If its left at zero then assume it's infinite
			//Destroy the projectile when the distance is greater than the aliveRange
			if (Vector3.Distance (startPos, transform.position) > aliveRange) {
                DestroyMissile();
			}
		}

        //missile.velocity = transform.forward * speed;
        //transform.Translate (new Vector3 (0, 0, speed * Time.deltaTime));
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Fire();

	    if(target == null || missile == null)
        return;
        
        if (delayBeforeTargetting <= Time.time) {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
 
            missile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 360)); //Used to be "turn"
        }
	}

    public void DestroyMissile() { //Call this function to destroy the current missile instance
        //if (smokeTrail != null) { //UNITY 5.1.3 has problems with particle systems so I bult this failsafe just in case I disable them
        print("Destroying smoke trail");
            smokeTrail.GetComponent<ParticleSystem>().startSize = 0; //Make the particle system not spawn anymore smoke
            Destroy(smokeTrail, 5F); //Destroy it after 5 seconds
        //}

        //NetworkServer.Destroy(gameObject);
        //Shooter.GetComponent<Projectile_daisyChain>().CmdDestroy(gameObject);
        Destroy(gameObject);
    }

    void Fire() {
        float distance = Mathf.Infinity;
 
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            
            if (go != Shooter) { //Make sure the missile doesnt track the player who shot it
                //print("Target Name: " + go.transform.name);
                float diff = (go.transform.position - transform.position).sqrMagnitude;
 
                if(diff < distance)
                {
                    if (diff <= range) {
                        distance = diff;
                        target = go.transform;
                    }
                }
            }
        }
    }
    //Collisions
	void OnCollisionEnter(Collision col){
		//If projectile hit a player
		if (col.gameObject.tag == "Player") {
			//print ("The projectile hit player: " + col.transform.name);
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, damage, "Player", gameObject, Shooter); //Run function in the daisychain
            if (col.gameObject != Shooter) {
                Shooter.GetComponent<Audio_Sync>().PlaySound(6, transform.position); //Play missile impact sfx via the shooter
                DestroyMissile();
            }
		}
		//If projectile hit an asteroid
		if (col.gameObject.tag == "Asteroid"){
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, damage, "Asteroid", gameObject, Shooter); //Run function in the daisychain
            Shooter.GetComponent<Audio_Sync>().PlaySound(6, transform.position); //Play missile impact sfx via the shooter
            DestroyMissile();
		}
    }
}
