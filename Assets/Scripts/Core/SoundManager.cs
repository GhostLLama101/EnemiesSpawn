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
    
    public float volume = 1f;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, bool isSong = false)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);

        audioSource.volume = volume;
                
        audioSource.clip = audioClip;

        audioSource.loop = true;

        if (isSong)
        {
            if (currentSongSource != null)
            {
                Destroy(currentSongSource.gameObject);
            }
            currentSongSource = audioSource;

            audioSource.volume = 0.5f*volume;
        }

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        if (!isSong)
        {
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}
