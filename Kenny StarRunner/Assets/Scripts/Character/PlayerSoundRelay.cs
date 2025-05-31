using UnityEngine;

public class PlayerSoundRelay : MonoBehaviour
{
    public PlayerSounds playerSounds;

    public void FootstepSound()
    {
        if (playerSounds != null)
            playerSounds.FootstepSound();
    }
}