using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource audioSourcePrefab;
    public AudioClip hitSound;
    public AudioClip startWave;
    public AudioClip completeWave;
    public AudioClip shootSound;
    public AudioClip selectSound;

    public AudioClip easySong;
    public AudioClip mediumSong;
    public AudioClip endlessSong;
    public AudioClip mainMenuSong;

    public AudioSource currentSongSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume, bool isSong = false)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);

        if (isSong)
        {
            if (currentSongSource != null)
            {
                Destroy(currentSongSource.gameObject);
            }
            currentSongSource = audioSource;
        }
        
        audioSource.clip = audioClip;
        
        audioSource.volume = volume;

        audioSource.loop = true;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        if (!isSong)
        {
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}
