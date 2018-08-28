using UnityEngine;
using System.Collections;

public class Player_Collisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Boundary") {
            print("Hit Border!");
        }   
    }

    void OnCollisionExit(Collision col) {
        if (col.gameObject.tag == "Boundary") {
            print("Left Border");
        }
    }
}
