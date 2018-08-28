using UnityEngine;
using System.Collections;

public class Camera_sync_bad : MonoBehaviour {
	
	public Transform target;
	public float distance = 20.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	
	public float lookAtHeight = 0.0f;
	
	public Rigidbody parentRigidbody;
	
	public float rotationSnapTime = 0.3F;
	
	public float distanceSnapTime;
	public float distanceMultiplier;
	
	private Vector3 lookAtVector;
	
	private float usedDistance;

	float wantedRotationAngleX;
	float wantedRotationAngleY;
	float wantedHeight;

	float currentRotationAngleX;
	float currentRotationAngleY;
	float currentHeight;
	
	Quaternion currentRotation;
	Vector3 wantedPosition;
	
	private float yVelocity = 0.0F;
	private float zVelocity = 0.0F;
	
	void Start () {
		
		lookAtVector =  new Vector3(0,lookAtHeight,0);
		
	}
	
	void LateUpdate () {

		wantedHeight = target.position.y + height;
		currentHeight = transform.position.y;

		//wantedRotationAngleX = target.eulerAngles.x;
		wantedRotationAngleY = target.eulerAngles.y;

		//currentRotationAngleX = transform.eulerAngles.x;
		currentRotationAngleY = transform.eulerAngles.y;

		//currentRotationAngleX = Mathf.SmoothDampAngle(currentRotationAngleX, wantedRotationAngleX, ref yVelocity, rotationSnapTime);
		currentRotationAngleY = Mathf.SmoothDampAngle(currentRotationAngleY, wantedRotationAngleY, ref yVelocity, rotationSnapTime);
		
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		wantedPosition = target.position;
		wantedPosition.y = currentHeight;
		
		usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (parentRigidbody.velocity.magnitude * distanceMultiplier), ref zVelocity, distanceSnapTime); 
		
		wantedPosition += Quaternion.Euler(0, currentRotationAngleY, 0) * new Vector3(0, 0, -usedDistance);
		//wantedPosition += Quaternion.Euler(currentRotationAngleX, 0, 0) * new Vector3(0, -usedDistance, 0);
		//wantedPosition += Quaternion.Euler(0, currentRotationAngleY, 0) * new Vector3(0, 0, 0);
		
		transform.position = wantedPosition;
		
		transform.LookAt(target.position + lookAtVector);
		
	}
	
}