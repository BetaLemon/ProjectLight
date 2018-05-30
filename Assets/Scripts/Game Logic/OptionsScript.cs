using UnityEngine;

public class OptionsScript : MonoBehaviour {

    private FMOD.Studio.EventInstance SFXVolumeTestEvent;

    private FMOD.Studio.Bus Master;
    private FMOD.Studio.Bus Music;
    private FMOD.Studio.Bus Sounds;
    public float masterVolume = 1f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.5f;

    [FMODUnity.EventRef]
    public string clicksound;

    private GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        Sounds = FMODUnity.RuntimeManager.GetBus("bus:/Master/Sounds");
        SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Collectables/LargeGemGet");
    }

    void Start()
    {
        Music.setVolume(musicVolume);
        Sounds.setVolume(sfxVolume);
        Master.setVolume(masterVolume);

        //Reference Initializations:
        gameStateDataScriptRef = GameStateScript.instance;
    }

    void Update()
    {
        Debug.Log("Music: " + musicVolume + "Sounds: " + sfxVolume + "Master: " + masterVolume);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        masterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        sfxVolume = newSFXVolume;

        FMOD.Studio.PLAYBACK_STATE PbState;
        SFXVolumeTestEvent.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeTestEvent.start();
        }
    }

    public void GoToMainMenu()
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);
        if (gameStateDataScriptRef.GetSceneSide() == true) //Came from ingame side
        {
            OptionsMode(true);
        }
        else
        {
            SendToMenu();
        }
    }
    public void BackToGame() //This button only appears if you just came from ingame and not from the main menu
    {
        FMODUnity.RuntimeManager.PlayOneShot(clicksound);
        gameStateDataScriptRef.SetSceneState(GameStateScript.SceneState.INGAME);
    }
    public void SendToMenu()
    {
        OptionsMode(false);
        gameStateDataScriptRef.SetSceneState(0);
    }
    public void OptionsMode(bool tf) //Activates(true)/Disactivates(false) back to menu confirmation so the player doesn't leave by mistake (Options divided in two states basically)
    {
            transform.GetChild(0).gameObject.SetActive(!tf); //Volume
            transform.GetChild(1).gameObject.SetActive(!tf); //Exit
            transform.GetChild(2).gameObject.SetActive(!tf); //BackToGame
            transform.GetChild(3).gameObject.SetActive(tf); //AreYouSureYouWantToLeave
            transform.GetChild(4).gameObject.SetActive(tf); //YES
            transform.GetChild(5).gameObject.SetActive(tf); //NO
    }

}
