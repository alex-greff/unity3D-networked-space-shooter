using UnityEngine;
using System.Collections;

public class SunFlareSpacing : MonoBehaviour {
	GameObject sun;
	Vector3 distanceFromPlayer = new Vector3(550F,550F,550F);

	// Use this for initialization
	void Start () {
		sun = GameObject.Find("Sun");
	}
	
	// Update is called once per frame
	void Update () {
		//Local to each player so each sun has a difference loc
		sun.transform.position = transform.position + distanceFromPlayer; //Keep the sun a set distance from the player
	}
}
