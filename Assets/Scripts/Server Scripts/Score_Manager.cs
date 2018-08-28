/**********************************************************
Alex Greff
12/01/2016
Score Manager
This script manages and displays the players and their kill
counts from highest to least.
////NOTE: keep in mind that I may have referenced functions
from other scripts\\\\\\
////NOTE 2: The reason I have this processing chain split up
into so many functions is so that I can access certain parts
externally\\\\\\
***********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class Score_Manager : NetworkBehaviour {
    public GameObject[] players; 
    public int[] playerScores;
    Text[] scoreSlots;

	// Use this for initialization
	void Start () {
        scoreSlots = GetComponent<GameManager_References>().scoreTextSlots;
        StartProcessingChain(); //Start the processing chain 
	}
	
	// Update is called once per frame
	void Update () {
        StartProcessingChain(); //Start the processing chain 
	}

/**************************
Function without parameters
***************************/
    public void StartProcessingChain() {
/****************
An array
*****************/
        players = GameObject.FindGameObjectsWithTag("Player"); //Find all the players
        playerScores = new int[players.Length]; //Create an array with the size of how many players are in the game
        for (int i = 0; i < players.Length; i++) { //Iterate through the players
            playerScores[i] = players[i].GetComponent<variables>().kills; //Get each player's score and assign it to a parallel array
        }
        SortScoreBoard(players, playerScores); //Sort the scoreboard now
    }

    public void SortScoreBoard(GameObject[] playersInOrder, int[]playerScoresInOrder) {
        int first = 0;
        int second = 0;
        GameObject firstGO;
        GameObject secondGO;

        //Get the initial values for the arrays
        playerScoresInOrder = playerScores; 
        playersInOrder = players;

/****************
Buble Sorting
*****************/
        //Buble sort the list of playerScores from greatest to least
        //Also mimic the same resasignments with the array of players
        //So the scores to player relationship is parallel
        for (int t = 1; t < playerScoresInOrder.Length; t++){ 
            for (int p = 0; p < playerScoresInOrder.Length-1; p++){ 
                first = playerScoresInOrder[p];
                second = playerScoresInOrder[p+1];
                firstGO = playersInOrder[p];
                secondGO = playersInOrder[p + 1];

                if (first < second){
                    playerScoresInOrder[p] = second;
                    playerScoresInOrder[p+1] = first;

                    playersInOrder[p] = secondGO;
                    playersInOrder[p + 1] = firstGO;
                }
             }
        }
        UpdateScoreBoard(playersInOrder, playerScoresInOrder); //Update the scoreboard GUI
    }

    public void UpdateScoreBoard(GameObject[] playersInOrder, int[] playerScoresInOrder) {
        for (int i = 0; i < scoreSlots.Length; i++) { //For all the score slots on the screen
            if (i < playerScoresInOrder.Length) { //Make sure we don't get an out of index range error
                scoreSlots[i].text = playerScoresInOrder[i] + "  " + playersInOrder[i].transform.name; //<Player Score>  <Player Name>
            }
            else {
                scoreSlots[i].text = ""; //Leave the rest of the score slots blank
            }
        }
    }
}
