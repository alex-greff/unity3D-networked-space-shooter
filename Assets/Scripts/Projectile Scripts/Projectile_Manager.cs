using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Projectile_Manager : NetworkBehaviour {
	//Properties 
	public float direction;
	public float speed;
	public float aliveRange;
	public float aliveTime;
	public int damage;


	//Rigidbody rb;

	public GameObject Shooter; //Will be assigned in the Player_Fire script. Each projectile uses the daisyChain script attached to the player who shot it.

	public GameObject hit_prefab; //The visual effect that will be called when the projectile hits something

	Vector3 startPos;



	// Use this for initialization
	void Start () {
		startPos = transform.position;
		//rb = GetComponent<Rigidbody>();
		//print (startPos);
		//ShooterName.transform.name = ShooterName.GetComponent<Projectile_daisyChain>().ShooterName; //Set the daisyChain's projectile to this gameobject

		//Sync a bunch of variables up (Im actually doing it manually for now because its really resource instensive)
		//Shooter.GetComponent<Projectile_daisyChain>().Shooter = Shooter;
		//Shooter.GetComponent<Projectile_daisyChain>().hit_prefab = hit_prefab;

        //If over 0 then activate
		if (aliveTime > 0){
			//Destroy the projectile after being alive for the aliveTime length 
			Destroy (gameObject, aliveTime);
		}

	}

	
	// Update is called once per frame
	void Update () {
        //Physics.IgnoreCollision(GetComponent<Collider>(), Shooter.GetComponent<MeshCollider>());

		transform.Translate(new Vector3(0,0,direction * speed * Time.deltaTime)); //Move the projectile
		//transform.Translate(new Vector3(0,0,0));
		//rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
		//rb.AddForce(speed);
		//rb.AddForce(new Vector3(0,0,speed*-1));


		//If over 0 then activate
		if (aliveRange > 0){
			//Destroy the projectile when the distance is greater than the aliveRange
			if (Vector3.Distance (startPos, transform.position) > aliveRange) {
				Destroy(gameObject);
			}
		}
		

		//Some random stuff of finding all the children colliders in the shooter's gameObject. Turns out I don't need it because the parent rigidbody automatically detects collisions from child colliders... Awesome!
		//foreach(Transform child in Shooter.transform){
			//print (child.name);
		//	if (child.name == "Ship"){
		//		foreach(Transform child2 in child.transform){
		//			//print ("Child2: " + child2);
		//			if(child2.tag == "Ship"){
		//				//print ("Child2: " + child);
		//				Physics.IgnoreCollision(GetComponent<Collider>(), child2.GetComponent<Collider>());
		//			}
		//		}
		//	}
		//}

	}
	//Collisions
	void OnCollisionEnter(Collision col){
		//If projectile hit a player
		if (col.gameObject.tag == "Player") {
			//print ("The projectile hit player: " + col.transform.name);
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, damage, "Player", gameObject, Shooter); //Run function in the daisychain
            //print("Destroying projectile in its script");
            if (col.gameObject != Shooter) {
                Destroy(gameObject);
            }
		}
		//If projectile hit an asteroid
		if (col.gameObject.tag == "Asteroid"){
			GameObject uIdentity = col.gameObject;

			Shooter.GetComponent<Projectile_daisyChain>().CmdTellServerWhoWasShot(uIdentity, damage, "Asteroid", gameObject, Shooter); //Run function in the daisychain
            //print("Destroying projectile in its script");
            Destroy(gameObject);
		}

		//TODO: Add one for the powerups
	}
}
