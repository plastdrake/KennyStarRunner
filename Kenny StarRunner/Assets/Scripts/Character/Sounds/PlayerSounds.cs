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

    //Pickup sounds
    [Header("Pickup sounds")]
    public AudioClip _coinPickup;

    //Active sound
    private AudioClip _currentSound;
    private bool _isPlaying;

    //Player sounds
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



    //Explosion sounds
    public void BarrelExplosionSound()
    {
        //_currentSound = _barrelExplosion;
        //StartCoroutine(PlaySound());
        _audioSource.PlayOneShot(_barrelExplosion);
    }

    public void MineExplosionSound()
    {
        //_currentSound = _mineExplosion;
        //StartCoroutine(PlaySound());
        _audioSource.PlayOneShot(_mineExplosion);
    }








    //Pickups sounds
    public void CoinPickupSound()
    {
        _audioSource.PlayOneShot(_coinPickup);
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
