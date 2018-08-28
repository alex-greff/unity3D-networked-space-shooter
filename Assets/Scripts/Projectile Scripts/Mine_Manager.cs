using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Mine_Manager : NetworkBehaviour {
    public int speed;
    public int damage;
    public float aliveTime;
    Rigidbody mine;
    public GameObject Shooter;
    //MeshCollider[] shooterColliders;
    Transform target;
    public GameObject hit_prefab;
    
    public float range;

	// Use this for initialization
	void Start () {
        mine = GetComponent<Rigidbody>();
        CheckForPlayers();

        //shooterColliders = Shooter.GetComponentsInChildren<MeshCollider>();
        if (aliveTime > 0){
			//Destroy the projectile after being alive for the aliveTime length 
			Destroy (gameObject, aliveTime);
		}
	}
	
	// Update is called once per frame
	void Update () {
        //for (int i = 0; i < shooterColliders.Length; i++) {
        //    Physics.IgnoreCollision(GetComponent<SphereCollider>(), shooterColliders[i]);
        //}
        if (target == null) { //If the mine hasnt locked onto anyone yet
            CheckForPlayers();
        }


	    if(target == null || mine == null)
        return;
        
        //mine.velocity = transform.forward * speed;
        
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
 
        mine.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 360)); //Used to be "turn"

        if (target != null) {
            //mine.velocity = transform.forward * speed;
            transform.Translate (new Vector3 (0, 0, speed * Time.deltaTime));
        }
	}

    public void DestroyMine() { //Call this function to destroy the current mine instance
        //NetworkServer.Destroy(gameObject);
        //Shooter.GetComponent<Projectile_daisyChain>().CmdDestroy(gameObject);
        Destroy(gameObject);
    }

    void CheckForPlayers() {
        float distance = Mathf.Infinity;
 
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            
            if (go != Shooter) { //Make sure the missile doesnt track the player who shot it
                //print("Target Name: " + go.transform.name);
                float diff = (go.transform.position - transform.position).sqrMagnitude;
 
                if(diff < distance)
                {
                    //print("Difference: " + diff);
                    distance = diff;

                    if (diff <= range) {
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
                Shooter.GetComponent<Audio_Sync>().PlaySound(7, transform.position); //Play mine impact sfx via the shooter
                Destroy(gameObject);
            }
		}
		//If projectile hit an asteroid
		if (col.gameObject.tag == "Asteroid"){
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, damage, "Asteroid", gameObject, Shooter); //Run function in the daisychain
            Shooter.GetComponent<Audio_Sync>().PlaySound(7, transform.position); //Play mine impact sfx via the shooter
            Destroy(gameObject);
		}
    }
}
