using System.Collections;
using UnityEngine;

public class OutroBackgroundMusic : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioClip _outroMusic;

    private AudioClip _currentSound;
    private bool _isPlaying;

    void Start()
    {
        _currentSound = _outroMusic;
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
