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

    //Shop sounds
    [Header("Shop sounds")]
    public AudioClip _buy;
    public AudioClip _lanaPengar;
    public AudioClip _equip;

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
        _currentSound = _barrelExplosion;
        StartCoroutine(PlaySound());
    }

    public void MineExplosionSound()
    {
        _currentSound = _mineExplosion;
        StartCoroutine(PlaySound());
    }




    //Buy sounds
    public void BuySound()
    {
        _currentSound = _buy;
        StartCoroutine(PlaySound());
    }
    public void NotEnoughMoneySound()
    {
        _currentSound = _lanaPengar;
        StartCoroutine(PlaySound());
    }
    public void EquipSound()
    {
        _currentSound = _equip;
        StartCoroutine(PlaySound());
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
