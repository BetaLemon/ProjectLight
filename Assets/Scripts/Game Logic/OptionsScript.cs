using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using FMODUnity;

public class OptionsScript : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    [FMODUnity.EventRef]
    public string clicksound;

    private FMOD.Studio.EventInstance SFXVolumeTestEvent;

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus Sounds;

    private GameStateScript gameStateDataScriptRef; //Reference to the Game/Global World Scene State

    void Awake()
    {
        Master = RuntimeManager.GetBus("bus:/Master");
        Music = RuntimeManager.GetBus("bus:/Master/Music");
        Sounds = RuntimeManager.GetBus("bus:/Master/Sounds");

        SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Collectables/LargeGemGet");
    }

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //Reference Initializations:
        gameStateDataScriptRef = GameStateScript.instance;
    }

    void Update()
    {
        //Debug.Log("Music: " + musicVolume + "Sounds: " + sfxVolume + "Master: " + masterVolume);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        Master.setVolume(newMasterVolume);
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        Music.setVolume(newMusicVolume);
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        Sounds.setVolume(newSFXVolume);

        FMOD.Studio.PLAYBACK_STATE PbState;
        SFXVolumeTestEvent.getPlaybackState(out PbState);
        if (PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SFXVolumeTestEvent.start();
        }
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
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
