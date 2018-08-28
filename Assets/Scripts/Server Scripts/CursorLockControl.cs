using UnityEngine;
using System.Collections;

public class CursorLockControl : MonoBehaviour {
    public bool lockCursor;
	// Use this for initialization
	void Start () {
        LockCursor(lockCursor);
	}
	
	// Update is called once per frame
	void Update () {
	    LockCursor(lockCursor);
	}

    public void LockCursor(bool yesOrNo) {
        if (yesOrNo == true) {
            Screen.lockCursor = true;
        }
        else {
            Screen.lockCursor = false;
        }
    }
}
