using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Player_Trail : NetworkBehaviour {
    public ParticleSystem trail;
    public Color[] colorList = new Color[9];
    public Color randomColor;

	// Use this for initialization
	void Start () {
        if (isLocalPlayer) {
            /*
            colorList[0] = new Color(255, 193, 212); //Light pink
            colorList[1] = new Color(218, 160, 255); //Light purple
            colorList[2] = new Color(147, 176, 255); //Light blue
            colorList[3] = new Color(183, 247, 255); //Light cyan
            colorList[4] = new Color(163, 255, 212); //Light green
            colorList[5] = new Color(188, 255, 189); //Light lime
            colorList[6] = new Color(255, 251, 186); //Light yellow
            colorList[7] = new Color(255, 187, 147); //Light orange
            colorList[8] = new Color(255, 112, 116); //Light red
            */
            randomColor = colorList[Random.Range(0, colorList.Length -1 )];
         
            Cmd_SyncRandomColor(randomColor);
        }

        
	}
	
    [Command]
    void Cmd_SyncRandomColor(Color clr) {
        Rpc_SyncRandomColor(clr);
    }
    [ClientRpc]
    void Rpc_SyncRandomColor(Color clr) {
        randomColor = clr;
    }

	// Update is called once per frame
	void Update () {
        if (isLocalPlayer) {
            Cmd_SyncRandomColor(randomColor);
        }
        trail.startColor = randomColor;
	}
}
