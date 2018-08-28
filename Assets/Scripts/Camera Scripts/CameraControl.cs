using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public GameObject Fighter;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Fighter.transform.position;
	}
}
