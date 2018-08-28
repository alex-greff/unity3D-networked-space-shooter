using UnityEngine;
using System.Collections;

public class Random_Rotation : MonoBehaviour {
	float randomX;
	float randomY;
	float randomZ;

	public float range = 0.5F;
	// Use this for initialization
	void Start () {
		randomX = Random.Range (-range, range);
		randomY = Random.Range (-range, range);
		randomZ = Random.Range (-range, range);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (randomX,randomY,randomZ);
	}
}
