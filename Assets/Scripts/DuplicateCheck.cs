using UnityEngine;
using System.Collections;

public class DuplicateCheck : MonoBehaviour {
	static DuplicateCheck instance = null;

	// Use this for initialization
	void Start () {
		if (instance != null) { //If there is an instance already
			Destroy (gameObject); //Destroy the pendning gameobject
		} else {
			instance = this; //"Claim" the instance
			GameObject.DontDestroyOnLoad (gameObject); //Don't destroy pending instance
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
