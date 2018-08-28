using UnityEngine;
using System.Collections;

public class Smoke_Destroy : MonoBehaviour {
    FollowScript followScript;



	// Use this for initialization
	void Start () {
        followScript = GetComponent<FollowScript>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (followScript.target == null) {
            //print("Destroying smoke trail");
            Destroy(gameObject, 2F);
        }
	}
}
