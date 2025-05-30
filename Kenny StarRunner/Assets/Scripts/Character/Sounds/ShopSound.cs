using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ShopSound : MonoBehaviour
{
    public AudioSource _audioSource;

    //Shop sounds
    [Header("Shop sounds")]
    public AudioClip _buy;
    public AudioClip _notEnoughMoney;
    public AudioClip _equip;
    public AudioClip _openMenu;

    private AudioClip _currentSound;
    private bool _isPlaying;

    //Buy sounds
    public void BuySound()
    {

        _currentSound = _buy;
        _audioSource.PlayOneShot(_buy);
    }
    public void NotEnoughMoneySound()
    {
        _currentSound = _notEnoughMoney;
        StartCoroutine(PlaySound());

    }
    public void EquipSound()
    {
        _audioSource.PlayOneShot(_equip);
    }

    public void OpenMenuSound()
    {
        _audioSource.PlayOneShot(_openMenu);
    }


    private IEnumerator PlaySound()
    {
        if (_isPlaying)
            yield break;
        _isPlaying = true;
        _audioSource.PlayOneShot(_currentSound);
        yield return new WaitForSeconds(_currentSound.length);
        _isPlaying = false;
    }
}
