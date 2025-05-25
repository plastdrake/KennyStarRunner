using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    //Player sounds
    public AudioSource _audioSource;
    public AudioClip _wollieBollie;
    public AudioClip _ischloPischlo;
    public AudioClip _lanaPengar;

    //Explosion sounds
    public AudioClip _barrelExplosion;
    public AudioClip _mineExplosion;

    //Active sound
    private AudioClip _currentSound;
    private bool _isPlaying;
    public void JumpSound()
    {
        _currentSound = _wollieBollie;
        StartCoroutine(PlaySound());
    }

    public void DeadSound()
    {
        _currentSound = _ischloPischlo;
        StartCoroutine(PlaySound());
    }

    public void BarrelExplosionSound()
    {
        _currentSound = _barrelExplosion;
        StartCoroutine(PlaySound());
    }

    public void MineExplosionSound()
    {
        _currentSound = _mineExplosion;
        StartCoroutine(PlaySound());
    }
    public void NotEnoughMoneySound()
    {
        _currentSound = _lanaPengar;
        StartCoroutine(PlaySound());
    }
    private IEnumerator PlaySound()
    {
        if(_isPlaying)
            yield break;
        _isPlaying = true;
        _audioSource.PlayOneShot(_currentSound);
        yield return new WaitForSeconds(_currentSound.length);
        _isPlaying = false;
    }
}
