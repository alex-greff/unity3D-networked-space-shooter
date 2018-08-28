// Smooth Follow from Standard Assets
// Converted to C# because I fucking hate UnityScript and it's inexistant C# interoperability
// If you have C# code and you want to edit SmoothFollow's vars ingame, use this instead.
using UnityEngine;
using System.Collections;

public class Camera_sync3 : MonoBehaviour {
	
	// The target we are following
	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	// Place the script in the Camera-Control group in the component menu
	//[AddComponentMenu("Camera-Control/Smooth Follow")]
	
	void LateUpdate () {
		// Early out if we don't have a target
		if (!target) return;
		
		// Calculate the current rotation angles
		float wantedRotationAngleY = target.eulerAngles.y;
		float wantedRotationAngleX = target.eulerAngles.x;
		float wantedRotationAngleZ = target.eulerAngles.z;
		float wantedHeight = target.position.y + height;
		
		float currentRotationAngleY = transform.eulerAngles.y;
		float currentRotationAngleX = transform.eulerAngles.x;
		float currentRotationAngleZ = transform.eulerAngles.z;
		float currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationDamping * Time.deltaTime);
		currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotationDamping * Time.deltaTime);
		currentRotationAngleZ = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleZ, rotationDamping * Time.deltaTime);


		// Damp the height
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		//var currentRotation = Quaternion.Euler(0, currentRotationAngleY, 0);
		var currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, currentRotationAngleZ);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		
		// Set the height of the camera
		transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z);
		
		// Always look at the target
		transform.LookAt(target);
	}
}