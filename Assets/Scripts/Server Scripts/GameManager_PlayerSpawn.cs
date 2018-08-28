using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

class Player : NetworkBehaviour
{
    [SyncVar]
    public Color color;
}


public class GameManager_PlayerSpawn : NetworkManager {
    public GameObject smallShipPrefab;
    public GameObject mediumShipPrefab;
    public GameObject largeShipPrefab;

    public GameObject[] spawnlocations;

    [SerializeField]
    private GameObject _playerPrefab;


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (PlayerPrefs.GetString("ShipType") == "Small") {
            _playerPrefab = smallShipPrefab;
        }
        else if (PlayerPrefs.GetString("ShipType") == "Medium") {
            _playerPrefab = mediumShipPrefab;
        }
        else if (PlayerPrefs.GetString("ShipType") == "Large") {
            _playerPrefab = largeShipPrefab;
        }
        else {
           _playerPrefab = mediumShipPrefab;
        }

        spawnlocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int r = Random.Range(0, spawnlocations.Length - 1);
        GameObject player = (GameObject)Instantiate(_playerPrefab, spawnlocations[r].transform.position, spawnlocations[r].transform.rotation);
        player.GetComponent<Player_ID>().playerConnection = conn;
        player.GetComponent<Player_ID>().playerControllerID = playerControllerId;
        //player.GetComponent<Player>().color = Color.red;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public void ChangeShip(NetworkConnection conn, short playerControllerId, GameObject oldShip, GameObject shipPrefab, int kills, string playerUniqueID) {
        spawnlocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int r = Random.Range(0, spawnlocations.Length - 1);
        GameObject player = (GameObject)Instantiate(shipPrefab, spawnlocations[r].transform.position, spawnlocations[r].transform.rotation);

        player.GetComponent<Player_ID>().playerConnection = conn;
        player.GetComponent<Player_ID>().playerControllerID = playerControllerId;
        player.GetComponent<variables>().kills = kills; //Set the player's kills
        player.GetComponent<Player_ID>().OverrideName(playerUniqueID); //Set the player's name
        player.GetComponent<Player_Radar>().EnableRadar();
        //var conn = oldPlayer.connectionToClient;
        //var newPlayer = Instantiate<GameObject>(playerPrefab);

        Destroy(oldShip);
        
        NetworkServer.ReplacePlayerForConnection(conn, player, 0);
        //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    /*
    private void Setup()
    {
     // ...
 
     NetworkServer.RegisterHandler(MsgType.AddPlayer, OnClientAddPlayer);
     // ...
    }
 
     private void OnClientAddPlayer(NetworkMessage netMsg)
     {
         AddPlayerMessage msg = netMsg.ReadMessage<AddPlayerMessage>();
         if (msg.playerControllerId == 0) // if you wanna check this
         {
             Debug.Log("Spawning player...");
             SpawnPlayer(netMsg.conn); // the above function
         }
     }
     */
}
