using UnityEngine;
using System.Collections;

public class RadarCamera_sync : MonoBehaviour {
	public GameObject targetGameObject;

	float wantedYRot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //wantedYRot = targetGameObject.transform.eulerAngles.y;

        //transform.position = targetGameObject.transform.position;

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, wantedYRot, transform.eulerAngles.z);
        if (targetGameObject != null) {
            transform.position = targetGameObject.transform.position;
            transform.rotation = targetGameObject.transform.rotation;
        }
	}
}
