using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindFirstObjectByType<AudioManager>();

            return _instance;
        }
    }

    [SerializeField] private float sfxVolume = 0.25f;
    [SerializeField] private float bgVolume = 0.2f;

    [SerializeField] AudioClip bgAudioClip;
    [SerializeField] AudioClip shootSFXClip;
    [SerializeField] AudioClip hitSFXClip;
    [SerializeField] AudioClip roundSFXClip;

    [SerializeField] AudioSource bgSource;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void PlayBG()
    {
        bgSource.clip = bgAudioClip;
        bgSource.loop = true;
        bgSource.volume = bgVolume;
        bgSource.Play();
    }

    public void PlayShootSFX()
    {
        PlaySFX(shootSFXClip);
    }

    public void PlayHitSFX()
    {
        PlaySFX(hitSFXClip);
    }

    public void PlayRoundSFX()
    {
        PlaySFX(roundSFXClip);
    }

    void PlaySFX(AudioClip clip)
    {
        var audioSource = new GameObject().AddComponent<AudioSource>();
        audioSource.transform.SetParent(transform);
        audioSource.PlayOneShot(clip);
        audioSource.volume = sfxVolume;
    }

}
