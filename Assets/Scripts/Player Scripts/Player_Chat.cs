/**********************************************************
Alex Greff
12/01/2016
Player Inteface
This script is run on the client side when the player inputs
a new chat messsage. 
////NOTE: keep in mind that I may have referenced functions
from other scripts\\\\\\
***********************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player_Chat : NetworkBehaviour {
    Button chatSend; //The send button

    //Script references
    variables variablesScript;
    GameObject gameManager;
    InputField chatInput;
    Chat_Manager chatManager;
    Player_ID playerIDScript;

/****************
An array
*****************/
    //A nice list of swear words to block (sheild your eyes!!!)
    string[] swearWords = new string[] { "fuck", "shit", "cunt", "fag", "ass", "nigger", "nigga", "bitch", "blowjob", "cock", "cum", "dick", "dildo", "gay", "handjob", "homo", "hump", "jizz", "lesbian", "negro", "pussy", "pussies", "rimjob", "slut", "tit", "vag", "vagina", "whore", "koon" };

	// Use this for initialization
	void Start () {
        //Get references to other scripts on the player's gameobject
        variablesScript = GetComponent<variables>();
        gameManager = GameObject.Find("GameManager");
        chatManager = gameManager.GetComponent<Chat_Manager>();
        chatInput = gameManager.GetComponent<GameManager_References>().chatInput.GetComponent<InputField>();
        playerIDScript = GetComponent<Player_ID>();

	    if (isLocalPlayer) { //If its the local player
            //Get the chat send button from the references stored in the GameManager
            chatSend = gameManager.GetComponent<GameManager_References>().chatSendButton.GetComponent<Button>(); 
            chatSend.onClick.AddListener(SendChat); //Add a listener to that button

            //Add listeners to the inputbox for the chat
            chatInput.onValueChanged.AddListener(Typing);
            chatInput.onEndEdit.AddListener(ExitEditing);
        }
	}

    void Update() {
        if (isLocalPlayer) { //If it's the local player
            if (variablesScript.isTyping) { //If the player is typing in the chat box
                if (Input.GetKeyDown (KeyCode.Tab) == true) { //If the player presses enter
                    SendChat(); //Send the chat
                }
            }
            if (!variablesScript.isTyping) { //If the player is not already typing
                if (Input.GetKeyDown (KeyCode.C) == true) { //If the player presses c
                    chatInput.Select(); //Select the input box so the player can type
                }
            }
        }
    }

    public void Typing(string value) { //Enter typing mode
        if (isLocalPlayer) {
            variablesScript.isTyping = true;
            Screen.lockCursor = false; //Show cursor
        }
    }

    public void ExitEditing(string value) { //Exit editing
        if (isLocalPlayer) {
            variablesScript.isTyping = false;
            Screen.lockCursor = true; //Hide cursor
        }
    }

/**************************
Function without parameters
***************************/
    public void SendChat() {
        if (isLocalPlayer) {
            print("sending chat");
            string PlayerID = playerIDScript.playerUniqueIdentity; //Get the player's name
            string input = chatInput.text; //Get the chat that was typed
            chatInput.text = ""; //Clear the input box
        
    /****************
    String function
    *****************/
            input = input.Trim(); //Trim spaces
            input = CheckForSwearWords(input); //Run a chat filter
            if (input != null) { //If the chat is not empty
                string message = PlayerID + ": " + input; //Add the player's name in front of the message
                Cmd_UpdateChat(message); //Send chat message to the server
            }
            ExitEditing(""); //Exit the chat
        }
    }

/**************************
Function with parameters
and return type
***************************/
    string CheckForSwearWords(string original) {
        string corrected = original;
/****************
String function
*****************/
        for (int i = 0; i < swearWords.Length; i++) { //Iterate through the swear words
            if (corrected.Contains(swearWords[i])){ //If the string has a swear word in it
                string bleep = ""; //The bleep string
                for (int x = 0; x < swearWords[i].Length; x++) { //For each character in the swear word, add an * to the bleep string
                    bleep += "*";
                }
                corrected = corrected.Replace(swearWords[i], bleep); //Replace all found swear words with a BLEEP
            }
        }

        return corrected; //Returned the corrected, swear-free version of the chat message
    }

/**************************
Function with parameters
***************************/
    [Command] 
    public void Cmd_UpdateChat(string message) { //Send the chat message to the server
        Rpc_ChatToClients(message); //Send it to all the clients
    }

    [ClientRpc]
    void Rpc_ChatToClients(string message) { //This function is run on all clients
        chatManager.chatHistory.Add(message); //Add the new message to the list containing all the chat
        chatManager.UpdateChat(); //Update the chat 
    }
}
