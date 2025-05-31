using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource _audioSource;

    [Header("Player sounds")]
    public AudioClip _wollieBollie;      // Jump sound
    public AudioClip _ischloPischlo;     // Death sound
    public AudioClip _footstep;          // Footstep sound
    public AudioClip _slide;             // Slide sound
    public AudioClip _jump;              // Jump sound

    [Range(0f, 1f)]
    public float footstepVolume = 0.3f;
    [Range(0f, 1f)]
    public float slideVolume = 0.5f;
    [Range(0f, 1f)]
    public float jumpVolume = 0.5f;
    [Range(0f, 1f)]
    public float deathVolume = 0.7f;

    [Header("Voice Lines")]
    public AudioClip _spawnVoiceLine;    // Voice line after spawn
    [Range(0f, 1f)]
    public float spawnVoiceLineVolume = 1.0f;
    [Tooltip("Delay in seconds before playing the spawn voice line.")]
    public float spawnVoiceLineDelay = 10f;

    [Header("Explosion sounds")]
    public AudioClip _barrelExplosion;
    public AudioClip _mineExplosion;
    public AudioClip _electricity;

    [Range(0f, 1f)]
    public float barrelExplosionVolume = 1.0f;
    [Range(0f, 1f)]
    public float mineExplosionVolume = 1.0f;
    [Range(0f, 1f)]
    public float electricityVolume = 1.0f;

    [Header("Pickup sounds")]
    public AudioClip _coinPickup;
    [Range(0f, 1f)]
    public float coinPickupVolume = 1.0f;

    //Active sound
    private AudioClip _currentSound;
    private float _currentVolume = 1.0f;
    private bool _isPlaying;
    private bool _isDead;

    void Start()
    {
        if (_spawnVoiceLine != null && spawnVoiceLineDelay > 0f)
            StartCoroutine(PlaySpawnVoiceLineAfterDelay());
    }

    private IEnumerator PlaySpawnVoiceLineAfterDelay()
    {
        yield return new WaitForSeconds(spawnVoiceLineDelay);
        PlaySpawnVoiceLine();
    }

    //Player sounds
    public void JumpSound()
    {
        if (_jump != null)
            _audioSource.PlayOneShot(_jump, jumpVolume);
    }

    public void DeadSound()
    {
        _isDead = true;
        _currentSound = _ischloPischlo;
        _currentVolume = deathVolume;
        StartCoroutine(PlaySound());
        _isDead = false;
    }

    public void FootstepSound()
    {
        if (_footstep != null)
            _audioSource.PlayOneShot(_footstep, footstepVolume);
    }

    public void SlideSound()
    {
        if (_slide != null)
            _audioSource.PlayOneShot(_slide, slideVolume);
    }

    // Voice line
    public void PlaySpawnVoiceLine()
    {
        if (_spawnVoiceLine != null)
            _audioSource.PlayOneShot(_spawnVoiceLine, spawnVoiceLineVolume);
    }

    //Explosion sounds
    public void BarrelExplosionSound()
    {
        if (_barrelExplosion != null)
            _audioSource.PlayOneShot(_barrelExplosion, barrelExplosionVolume);
    }

    public void MineExplosionSound()
    {
        if (_mineExplosion != null)
            _audioSource.PlayOneShot(_mineExplosion, mineExplosionVolume);
    }

    public void Electricuted()
    {
        if (_electricity != null)
            _audioSource.PlayOneShot(_electricity, electricityVolume);
    }

    //Pickup sounds
    public void CoinPickupSound()
    {
        if (_coinPickup != null)
            _audioSource.PlayOneShot(_coinPickup, coinPickupVolume);
    }

    private IEnumerator PlaySound()
    {
        if (_isPlaying && !_isDead)
            yield break;
        _isPlaying = true;
        _audioSource.PlayOneShot(_currentSound, _currentVolume);
        yield return new WaitForSeconds(_currentSound.length);
        _isPlaying = false;
    }
}
