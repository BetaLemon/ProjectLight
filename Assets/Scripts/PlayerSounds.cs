using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    [FMODUnity.EventRef]
    public string walkSound;

    public void PlayWalkAnimationSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(walkSound);
    }
}
