/**********************************************************
Alex Greff
12/01/2016
Player Inteface
This script focuses on providing controls for the player
to interface and interact in the game
////NOTE: keep in mind that I may have referenced functions
from other scripts\\\\\\
***********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShipControl : NetworkBehaviour {
	variables variablesScript;
    Player_Boost boostScript;

	public GameObject MainShip;
	//public GameObject Ship;
	public GameObject Camera;
	public float direction = 1;
	public float maxSpeed;
	[SyncVar]public float speed;

	public float maneuverability;

	float mouseScrollWheel;
    Vector2 mouseStart, mouseEnd, mouseDistance, mousePos, mouseLastPos;
    

    Quaternion wantedRot;

	Rigidbody rb;

    /*
    float lookSpeed = 15;
    float moveSpeed = 15;
    float rotationX = 0;
    float rotationY = 0;
    */

 
	private float rotationX = 0.0f;
	private float rotationY = 0.0f;
    private float rotationZ = 0.0f;

	// Use this for initialization
	void Start () {
		variablesScript = GetComponent<variables>();
        boostScript = GetComponent<Player_Boost>();

		rb = GetComponent<Rigidbody>();
		//maxSpeed = variablesScript.maxSpeed;
		speed = maxSpeed;

        //Screen.lockCursor = true;
        mousePos = Input.mousePosition;
        mouseLastPos = Input.mousePosition;

        Screen.lockCursor = true;
        rotationX = MainShip.transform.rotation.x;
        rotationX = MainShip.transform.rotation.y;
	}
    [Command]
    void Cmd_SyncSpeed (float spd) {
        speed = spd;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isLocalPlayer) {
			if (!variablesScript.isDead && !variablesScript.isTyping && !variablesScript.isPaused) { //Make sure player is not dead, typing, or paused
                //if (!variablesScript.cameraFreeLook) { //If the player is not in free look or boosting
                    //print("can slow down");
                if (!variablesScript.cameraFreeLook) {
				    mouseScrollWheel = Input.GetAxis ("Mouse ScrollWheel");
				    speed += (4 * mouseScrollWheel);
                }
                if (!variablesScript.isBoosting) {
                    if (speed > maxSpeed) {
				       speed = maxSpeed;
				    }
                }

				if (speed < 0) {
				    speed = 0;
				}
                //}

                


				//Move the ship forward
    		    
                /*
				//Up
				if (Input.GetKey (KeyCode.W) == true) {
					MainShip.transform.Rotate (new Vector3 (-maneuverability * Time.deltaTime, 0, 0));
				}
				//Down
				if (Input.GetKey (KeyCode.S) == true) {
					MainShip.transform.Rotate (new Vector3 (maneuverability * Time.deltaTime, 0, 0));
				}
				//Right
				if (Input.GetKey (KeyCode.D) == true) {
					MainShip.transform.Rotate (new Vector3 (0, maneuverability * Time.deltaTime, 0));
				}
				//Left
				if (Input.GetKey (KeyCode.A) == true) {
					MainShip.transform.Rotate (new Vector3 (0, -maneuverability * Time.deltaTime, 0));
				}

				//Rotate to the left
				if (Input.GetKey (KeyCode.Q) == true) {
					MainShip.transform.Rotate (new Vector3 (0, 0, maneuverability * Time.deltaTime));
				}
				//Rotate to the right
				if (Input.GetKey (KeyCode.E) == true) {
					MainShip.transform.Rotate (new Vector3 (0, 0, -maneuverability * Time.deltaTime));
				}
                */

			}
		}
	}
    void Update(){
        if (isLocalPlayer) {
            if (variablesScript.isDead || variablesScript.isPaused) {
                Screen.lockCursor = false;
            }


            if (!variablesScript.isDead && !variablesScript.isPaused) { //If the player not dead or paused
                MainShip.transform.Translate (new Vector3 (0, 0, direction * speed * Time.deltaTime));
            }
            Cmd_SyncSpeed(speed);

            if (!variablesScript.isDead && !variablesScript.isTyping && !variablesScript.isPaused) { //Make sure player is not dead, typing, or paused
                if (Input.GetKeyDown (KeyCode.Space) == true) {
                    //Check if boost is available
                    bool canBoost = boostScript.canBoost(); //Check the script if the player is off cooldown
                    if (canBoost && !variablesScript.isBoosting) { //Check if player can use boost
                        boostScript.RunBoost(speed); //Run the boost
                    }
                }

                if(Input.GetKeyDown(KeyCode.Z) == true){ //Toggle look mode
                    if (variablesScript.cameraFreeLook == true) {
                        variablesScript.cameraFreeLook = false;
                    }
                    else {
                        variablesScript.cameraFreeLook = true;
                    }
                    //print("Toggled camera free look to: " + variablesScript.cameraFreeLook);
		        }
            }
            
            if (!variablesScript.isCameraLooking) { //If the player is not moving the camera in the free look mode
                  //VERSION 4
                    //Get the mouse's input
                    float mouseX = Mathf.Clamp(Input.GetAxis("Mouse X"), -4, 4);
                    float mouseY = Mathf.Clamp(Input.GetAxis("Mouse Y"), -4, 4);
                    float mouseZ = Mathf.Clamp(Input.GetAxis("Mouse X"), -4, 4);
                    
                    if (Input.GetMouseButton(1)){ //If the player right clicks
                        rotationZ += mouseZ * maneuverability * Time.deltaTime; //Rotate the ship on the z axis using the x axis of the mouse
                    }
                    
                    else {
                        rotationX += mouseX * maneuverability * Time.deltaTime; //The ammount to rotate on the x axis
                    }

                    rotationY += mouseY * maneuverability * Time.deltaTime; //The amount to rotate on the y axis

                    //Make the rotation amounts smaller 
                    rotationX /= 2.5F;
                    rotationY /= 2.5F;
                    rotationZ /= 2.5F;
                    
                    //Add the rotations to the ship's rotations
		            transform.localRotation *= Quaternion.AngleAxis(rotationX, Vector3.up);
		            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
                    transform.localRotation *= Quaternion.AngleAxis(rotationZ, Vector3.back);

                    

                    
                    /*
                //VERSION 2
                    mousePos = Input.mousePosition;
                    Vector2 threshold = new Vector2(0.0F, 0.0F);
                    if (Mathf.Abs(mousePos.x - mouseLastPos.x) > threshold.x) { //If the mouse has been moved
                        Vector2 distance = mousePos - mouseLastPos;
                        distance = new Vector2(distance.x * 2, distance.y * 2);
                        wantedRot = MainShip.transform.localRotation *  Quaternion.Euler(-distance.y, distance.x, 0);
                    }


                    mouseLastPos = Input.mousePosition;

                    MainShip.transform.localRotation = Quaternion.Slerp(MainShip.transform.localRotation, wantedRot, 2 * Time.deltaTime); //Apply the wanted rotation to the camera's pivot gameobject

                    */
                //VERSION 1
                    //Turn ship
                    /*
                    if (Input.GetMouseButtonDown(1)){
				        mouseStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			        }
			        if (Input.GetMouseButton(1)){
				        mouseEnd = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			        }
			        if (Input.GetMouseButtonUp(1)){
                        //Reset the mouse start and end points
				        mouseStart = new Vector2(0,0); 
				        mouseEnd = new Vector2 (0,0);
			        }

                
                    mouseDistance = mouseStart-mouseEnd; //Calculate the distance the mouse traveled
			        mouseDistance /= 10000; //Make it a little smaller
                    print("Mouse Distance: "+mouseDistance);
                    //Put a max and min on the distances
                    mouseDistance.x = Mathf.Clamp(mouseDistance.x, -0.05F, 0.05F); 
                    mouseDistance.y = Mathf.Clamp(mouseDistance.y, -0.05F, 0.05F);

                    
                    //MainShip.transform.Rotate(new Vector3(maneuverability * mouseDistance.y, 0, 0));
                    //MainShip.transform.Rotate(new Vector3(0,maneuverability * -mouseDistance.x, 0));
                    */
                }
            if (isLocalPlayer) {
                //Toggle the cursor
		            if (Input.GetKeyDown (KeyCode.End))
		            {
			            Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
		            }
            }
        }
    }
}
