using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof (AudioSource))]
public class Audio_Sync : NetworkBehaviour {
    AudioSource source;
    public AudioClip[] clips;

	void Start () {
        source = GetComponent<AudioSource>();
	}

    public void PlaySound (int id, Vector3 location) {
        if (id >= 0 && id < clips.Length) {
            Cmd_SendServerSoundID(id, location);
        }
    }

    [Command]
    void Cmd_SendServerSoundID (int id, Vector3 loc) {
        Rpc_SendSoundIdToClients(id, loc);
    }

    [ClientRpc]
    void Rpc_SendSoundIdToClients (int id, Vector3 loc) {
        //source.PlayOneShot(clips[id]);
        AudioSource.PlayClipAtPoint(clips[id], loc);
    }
}
