using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource _audioSource;
    public AudioClip _kennyIntro;

    private AudioClip _currentSound;
    private bool _isPlaying;

    void Start()
    {
        _currentSound = _kennyIntro;
        StartCoroutine(BackgroundLoop());
    }

    private IEnumerator BackgroundLoop()
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
