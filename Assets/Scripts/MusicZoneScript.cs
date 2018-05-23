using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZoneScript : MonoBehaviour {

    [FMODUnity.EventRef]
    public string areaMusic;

    FMOD.Studio.EventInstance areaSong;

    void Start () {
        areaSong = FMODUnity.RuntimeManager.CreateInstance(areaMusic);
    }

    void OnTriggerStay(Collider other)
    {
        FMOD.Studio.PLAYBACK_STATE musicState;
        areaSong.getPlaybackState(out musicState);
        if (other.CompareTag("Player") && musicState != FMOD.Studio.PLAYBACK_STATE.PLAYING) //Check it's the player and avoid music overlapping
        {
            areaSong.start();
        }
    }
}
