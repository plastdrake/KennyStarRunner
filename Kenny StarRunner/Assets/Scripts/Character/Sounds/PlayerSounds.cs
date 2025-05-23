using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioClip _wollieBollie;
    //public AudioClip _ischloPischlo;

    private AudioClip _currentSound;
    private bool _isPlaying;
    public void JumpSound()
    {
        _currentSound = _wollieBollie;
        StartCoroutine(PlaySound());
    }

    public void Dead()
    {
        //_currentSound = _ischloPischlo;
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        _isPlaying = true;
        _audioSource.PlayOneShot(_currentSound);
        yield return new WaitForSeconds(_currentSound.length);
        _isPlaying = false;
    }
}
