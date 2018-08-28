using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Audio_Controller : MonoBehaviour {

    public void SetVolume (Slider s) {
        AudioListener.volume = s.value;
    }
}
