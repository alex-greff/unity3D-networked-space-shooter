using UnityEngine;
using System.Collections;

public class LayerIgnore_Manager : MonoBehaviour {

	//This script simply manages all the layers that ignore each other
	void Update () {
        Physics.IgnoreLayerCollision(12, 11, true); //Asteroid detector and projectiles
        Physics.IgnoreLayerCollision(12, 8, true); //Asteroid detector and ships
        Physics.IgnoreLayerCollision(12, 12, true); //Asteroid detector with itself
        
        Physics.IgnoreLayerCollision(14,1, true); //Special and transparentFX layers
        Physics.IgnoreLayerCollision(11,1, true); //Projectile and transparentFX layers
        Physics.IgnoreLayerCollision(11,14, true); //Projectile and special layers
        Physics.IgnoreLayerCollision(11,11, true); //Projectile with itself
        Physics.IgnoreLayerCollision(14,14, true); //Special with itself
	}
}
