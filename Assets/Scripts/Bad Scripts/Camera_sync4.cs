using UnityEngine;
using System.Collections;

public class Camera_sync4 : MonoBehaviour {
	public Transform target;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + offset;
		//transform.forward = target.forward;
		transform.rotation = target.rotation;
	}
}
