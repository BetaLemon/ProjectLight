using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZoneScript : MonoBehaviour {

    private GameStateScript gameStateScript;

    [FMODUnity.EventRef]
    public string areaMusic;

    public bool isPlaying = false;

    FMOD.Studio.EventInstance areaSong;

    void Start () {
        gameStateScript = GameObject.Find("GameState").GetComponent<GameStateScript>();
    }

    void OnTriggerStay(Collider other)
    {
        FMOD.Studio.PLAYBACK_STATE musicState;
        areaSong.getPlaybackState(out musicState);
        if (other.CompareTag("Player") && !isPlaying) //Check it's the player and avoid music overlapping
        {
            gameStateScript.playOST(areaMusic);
            areaSong.start();

            isPlaying = true;
        }
    }

    void update() {
        if (isPlaying && gameStateScript.getCurrentLinkPlaying() != areaMusic) //Check if we're really still playing in the gamestate script
        {
             isPlaying = false;
        }
    }
}
