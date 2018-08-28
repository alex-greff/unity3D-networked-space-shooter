using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class Chat_Manager : MonoBehaviour {
    public List<string> chatHistory = new List<string>();
    Text[] chatTextSlots;

	// Use this for initialization
	void Start () {
        chatTextSlots = GetComponent<GameManager_References>().chatTextSlots;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateChat() {
        for (int i = 0; i < chatHistory.Count; i++)  {
            //print(chatHistory[i]);
            if (i<chatTextSlots.Length) {
                //List<string> reversedChatHistory = chatHistory;
                //reversedChatHistory.Reverse();
                int historyLength = chatHistory.Count;
                chatTextSlots[i].text = chatHistory[historyLength - 1 - i];
            }
        }
    }
}
