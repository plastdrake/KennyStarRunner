using System.Collections;
using UnityEngine;

public class ShopSound : MonoBehaviour
{
    public AudioSource _audioSource;

    //Shop sounds
    [Header("Shop sounds")]
    public AudioClip _buy;
    public AudioClip _lanaPengar;
    public AudioClip _equip;

    private AudioClip _currentSound;
    private bool _isPlaying;

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
