using UnityEngine;

public class MusicZoneScript : MonoBehaviour {

    private GameStateScript gameStateScript;

    public int areaMusicIndex;

    public bool isPlaying = false;

    void Start () {
        gameStateScript = GameObject.Find("GameState").GetComponent<GameStateScript>();
    }

    void OnTriggerStay(Collider other)
    {
        //FMOD.Studio.PLAYBACK_STATE musicState;
        //areaSong.getPlaybackState(out musicState);
        if (other.CompareTag("Player") && !isPlaying) //Check it's the player and avoid music overlapping
        {
            gameStateScript.playOST(areaMusicIndex);
            isPlaying = true;
        }
    }

    void Update() {
        if (isPlaying && gameStateScript.getCurrentIndexPlaying() != areaMusicIndex) //Check if we're really still playing in the gamestate script
        {
             isPlaying = false;
        }
    }
}
