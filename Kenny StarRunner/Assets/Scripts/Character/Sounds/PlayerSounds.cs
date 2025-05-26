using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource _audioSource;
    //Player sounds
    [Header("Player sounds")]
    public AudioClip _wollieBollie;
    public AudioClip _ischloPischlo;

    //Explosion sounds
    [Header("Explosion sounds")]
    public AudioClip _barrelExplosion;
    public AudioClip _mineExplosion;
    public AudioClip _electricity;

    //Pickup sounds
    [Header("Pickup sounds")]
    public AudioClip _coinPickup;

    //Active sound
    private AudioClip _currentSound;
    private bool _isPlaying;
    private bool _isDead;

    //Player sounds
    public void JumpSound()
    {
        _currentSound = _wollieBollie;
        StartCoroutine(PlaySound());
    }

    public void DeadSound()
    {
        _isDead = true;
        _currentSound = _ischloPischlo;
        StartCoroutine(PlaySound());
        _isDead = false;
    }



    //Explosion sounds
    public void BarrelExplosionSound()
    {
        _audioSource.PlayOneShot(_barrelExplosion);
    }

    public void MineExplosionSound()
    {
        _audioSource.PlayOneShot(_mineExplosion);
    }

    public void Electricuted()
    {
        _audioSource.PlayOneShot(_electricity);
    }

    //Pickups sounds
    public void CoinPickupSound()
    {
        _audioSource.PlayOneShot(_coinPickup);
    }
    private IEnumerator PlaySound()
    {
        if(_isPlaying && !_isDead)
            yield break;
        _isPlaying = true;
        _audioSource.PlayOneShot(_currentSound);
        yield return new WaitForSeconds(_currentSound.length);
        _isPlaying = false;
    }
}
