using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Camera_sync : MonoBehaviour {
	variables variablesScript;
	//public Transform targetPlayer; 
	public GameObject targetGameObject;

	public float lerpRatePos;
	public float lerpRateRot;

	public GameObject playerCamera;
    public GameObject pivot;
	Animator playerCameraAnimator;

    float lerpRate = 7F;
    float slerpRate = 5F;

	//Image whiteFlash;
	//Color redColor = new Color(1f,0f,0f, 0f);
	//public bool firstTime = true;

	public Vector3 offsetPosBack;

	private Quaternion lookOffset;

	private Vector3 wantedPos;
	private Quaternion wantedRot;

	Vector3 randomRot;

	Vector2 mouseStart, mouseEnd, mouseDistance;
	Vector2 cameraOffset;

    float mouseScrollWheel = 0;

    private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	// Use this for initialization
	void Start () {
		variablesScript = targetGameObject.GetComponent<variables>();
		playerCameraAnimator = playerCamera.GetComponent<Animator>();
		//whiteFlash = GameObject.Find("WhiteFlash").GetComponent<Image>();
		//whiteFlash.color = redColor;

		playerCameraAnimator.SetTrigger("SceneStart");

		randomRot = new Vector3 (Random.Range(5F,10F),Random.Range(5F,10F),Random.Range(5F,10F));

        playerCamera.transform.localPosition = offsetPosBack; //Set how far back the player camera is initially from the ship
        wantedPos = offsetPosBack;
	}

	// Update is called once per frame
	void LateUpdate () {
        
		//if (firstTime == true){
		//	whiteFlash.color = Color.Lerp(Color.red, redColor, 5*Time.deltaTime);
		//	firstTime = false;
		//}


		//wantedPos = targetGameObject.transform.position;
		//wantedRot = targetGameObject.transform.rotation;
		if (!variablesScript.isDead){ //If the player is not dead

			if (variablesScript.cameraFreeLook == false){ //If the camera is not in free look
                wantedRot = Quaternion.Euler(0,0,0); //Freeze the rotation angles to 0
                pivot.transform.localRotation = Quaternion.Slerp(pivot.transform.localRotation, wantedRot, slerpRate * Time.deltaTime); //Apply the wanted roation to the camera
                playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, offsetPosBack, slerpRate * Time.deltaTime); //Apply the wanted position to the camera
			}
			else if (variablesScript.cameraFreeLook == true){ //If the camera is in free look  
                
            //VERSION 1             
                //Right click to look around
                if (Input.GetMouseButtonDown(1)){
                    variablesScript.isCameraLooking = true;
                    Screen.lockCursor = false;
				    mouseStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			    }
			    if (Input.GetMouseButton(1)){
				    mouseEnd = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			    }
			    if (Input.GetMouseButtonUp(1)){
                    variablesScript.isCameraLooking = false;
                    Screen.lockCursor = true;
                    //Reset the mouse start and end points
				    mouseStart = new Vector2(0,0); 
				    mouseEnd = new Vector2 (0,0);

			    }

			    mouseDistance = mouseStart-mouseEnd; //Calculate the distance the mouse traveled
			    mouseDistance /= 100; //Make it a little smaller
                //Put a max and min on the distances
                mouseDistance.x = Mathf.Clamp(mouseDistance.x, -5, 5); 
                mouseDistance.y = Mathf.Clamp(mouseDistance.y, -5, 5);

                //Make mouse distance smaller
                mouseDistance.x /= 2;
                mouseDistance.y /= 2;

			    cameraOffset += mouseDistance; //Add the mouse distance to the camera offset

                wantedRot = Quaternion.Euler(cameraOffset.y, -cameraOffset.x, 0); //Apply the offset to the wanted rotation

                pivot.transform.localRotation = Quaternion.Slerp(pivot.transform.localRotation, wantedRot, slerpRate * Time.deltaTime); //Apply the wanted rotation to the camera's pivot gameobject
                

            //VERSION 2
            /*
                if (Input.GetMouseButton(1)){
                    variablesScript.isCameraLooking = true;
                    rotationX += Input.GetAxis("Mouse X") * 90 * Time.deltaTime; //90 is a good maneuverability setting
		            rotationY += Input.GetAxis("Mouse Y") * 90 * Time.deltaTime;
		            rotationY = Mathf.Clamp (rotationY, -90, 90);

                    transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

		            if (Input.GetKeyDown (KeyCode.End))
		            {
			            Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
		            }
                }
                else {
                    variablesScript.isCameraLooking = false;
                }
            */

                mouseScrollWheel = Input.GetAxis ("Mouse ScrollWheel"); //Get the mouse scroll wheel input
                if (mouseScrollWheel != 0) { //If the mouse wheel was scrolled
                    wantedPos = playerCamera.transform.localPosition + new Vector3(0,0, 50*mouseScrollWheel); //Set the wanted position's z to the player's location plus the scroll ammount
                    wantedPos = new Vector3(offsetPosBack.x, offsetPosBack.y, wantedPos.z);
                    if (wantedPos.z > offsetPosBack.z) { //If the camera gets too close in
                        wantedPos = new Vector3(wantedPos.x, wantedPos.y, offsetPosBack.z); //Clamp it the the original starting point of the camera
                    }
                    if (wantedPos.z < -50F) { //If the camera gets too far
                        wantedPos = new Vector3(wantedPos.x, wantedPos.y, -50F); //Clamp it to the max
                    }
                    //print("Wanted Pos: " + wantedPos);
                }

                playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, wantedPos, slerpRate * Time.deltaTime); //Apply the wanted position to the camera
                //playerCamera.transform.localPosition = wantedPos;
			}

			//Have the camera follow the ship
            if (targetGameObject != null) {
			    transform.position = Vector3.Lerp(transform.position, targetGameObject.transform.position, lerpRatePos*Time.deltaTime);
			    transform.rotation = Quaternion.Lerp(transform.rotation, targetGameObject.transform.rotation, lerpRateRot*Time.deltaTime);
            }
		}

		else if (variablesScript.isDead){ //If the player is dead
			//transform.LookAt(targetGameObject.transform);
			transform.Rotate(randomRot*Time.deltaTime); //Rotate randomly
		}
	}
}
