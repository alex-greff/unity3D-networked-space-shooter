using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Pause : NetworkBehaviour {
    variables variablesScript;
    GameManager_References gameManager;
    GameObject pauseButton;
    GameObject resumeButton;
    GameObject exitButton;
    GameObject volumeSlider;


	// Use this for initialization
	void Start () {
        variablesScript = GetComponent<variables>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager_References>();
        pauseButton = gameManager.pauseButton;
        resumeButton = gameManager.resumeButton;
        exitButton = gameManager.exitButton;
        volumeSlider = gameManager.volumeSlider;

        if (isLocalPlayer) {
            //Add the listeners to the buttons
            pauseButton.GetComponent<Button>().onClick.AddListener(Pause);
            resumeButton.GetComponent<Button>().onClick.AddListener(Resume);
            exitButton.GetComponent<Button>().onClick.AddListener(Exit);
        }
	}

    void Update() {
        if (isLocalPlayer && !variablesScript.isTyping && !variablesScript.isDead) { //If the player is not typing
            if (Input.GetKeyDown (KeyCode.P) == true) { //Pause the game using a keyboard button
                if (variablesScript.isPaused == false) {
                    variablesScript.isPaused = true;
                    Pause();
                }
                else if (variablesScript.isPaused == true) {
                    variablesScript.isPaused = false;
                    Resume();
                }
            }
            if (Input.GetKeyDown (KeyCode.X) == true) { //Exit the game
                Exit();
            }
        }
    }
	
	public void Pause() {
        if (!variablesScript.isDead && !variablesScript.isTyping) {
            variablesScript.isPaused = true; //Pause the player
            pauseButton.SetActive(false);
            resumeButton.SetActive(true);
            exitButton.SetActive(true);
            volumeSlider.SetActive(true);
            Screen.lockCursor = false;
        }
    }

    public void Resume() { //Unpause the player
        variablesScript.isPaused = false;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        exitButton.SetActive(false);
        volumeSlider.SetActive(false);
        Screen.lockCursor = true;
    }

    public void Exit() {
        NetworkManager.singleton.StopHost();
    }
}
