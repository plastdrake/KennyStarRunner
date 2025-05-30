using System.Collections;
using UnityEngine;

public class LobbyBackgroundMusic : MonoBehaviour
{
    //https://breakingcopyright.com/song/john-tasoulas-voyager-1
    //Song: Voyager 1
    //Composer: John Tasoulas
    //Website: https://soundcloud.com/john-tasoulas
    //License: Creative Commons(BY 3.0) https://creativecommons.org/licenses/by/3.0/
    //Music powered by BreakingCopyright: https://breakingcopyright.com

    public AudioSource _audioSource;
    public AudioClip _lobbyMusic;

    private AudioClip _currentSound;
    private bool _isPlaying;

    void Start()
    {
        _currentSound = _lobbyMusic;
        StartCoroutine(LobbyBackgroundLoop());
    }

    private IEnumerator LobbyBackgroundLoop()
    {
        while (true)
        {
            if (!_isPlaying)
            {
                _isPlaying = true;
                _audioSource.PlayOneShot(_currentSound);
                yield return new WaitForSeconds(_currentSound.length);
                _isPlaying = false;
            }
            yield return null;
        }
    }
}
