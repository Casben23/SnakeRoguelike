using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum SFXType
{
    NewWave,
    EnemySpawn,
    EnemyHit,
    LevelUp,
    ProjectilFire,
    BodyPartDestroyed,

    PurchasePart,
    HighlightPart,
    InsuficientFunds,

    MortarExplode,
    MortarFly,

    PartLevelUp3,
    PartLevelUp2,

    ShowStatText,
    EnemyDie,
    CritHit,

    OnPlayerDie,
    OnPlayerDieEffect,

    DefaultButtonClick
}

[Serializable]
public class SFX
{
    public SFXType Type;
    public AudioClip SoundFXClip;

    [Range(0, 1)]
    public float Volume = 1;
}

[Serializable]
public class Music
{
    public EGameState StateType;
    public AudioClip MusicClip;
}

public class SoundManager : MonoBehaviour
{
    #region Singleton
    private static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Check if an instance already exists in the scene
                instance = FindObjectOfType<SoundManager>();

                // If no instance exists, create a new one
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SoundManager).Name);
                    instance = singletonObject.AddComponent<SoundManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }
    #endregion

    [SerializeField] private GameObject m_BackgroundMusicPlayer;

    [Header("Settings")]
    [SerializeField] private float m_SFXSoundMulitplier = 1;
    [SerializeField] private float m_MusicSoundMulitplier = 1;

    [Header("General Sounds")]
    [SerializeField] private List<SFX> m_SFX;
    [SerializeField] private List<Music> m_Music;

    private bool m_IsChangingSong = false;

    private void Start()
    {
        GameState.Instance.OnGameStateChange.AddListener(StartNewMusic);

        m_SFXSoundMulitplier = GameState.Instance.SFXMultiplier;
        m_MusicSoundMulitplier = GameState.Instance.MusicMultiplier;
    }

    private void OnDisable()
    {
        GameState.Instance.OnGameStateChange.RemoveListener(StartNewMusic);
    }

    public void PlaySound(SFX InAudioClip, bool InRandomPitch)
    {
        GameObject soundObject = new GameObject("TemporarySoundObject");

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = InAudioClip.SoundFXClip;
        audioSource.volume = InAudioClip.Volume * m_SFXSoundMulitplier;

        if (InRandomPitch)
        {
            float randomPitchValue = Random.Range(0.9f, 1.1f); // Adjust the range as needed
            audioSource.pitch = randomPitchValue;
        }

        audioSource.Play();

        Destroy(soundObject, InAudioClip.SoundFXClip.length);
    }

    public void PlayGeneralSound(SFXType InSFXType, bool InRandomPitch)
    {
        foreach(SFX sfx in m_SFX)
        {
            if(InSFXType == sfx.Type)
            {
                PlaySound(sfx, InRandomPitch);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartNewMusic(GameState.Instance.CurrentGameState);
        }

        if (m_BackgroundMusicPlayer == null)
            return;

        if (!TryChangeMusic())
            return;

        StartNewMusic(GameState.Instance.CurrentGameState);
    }

    private bool TryChangeMusic()
    {
        AudioSource backgroundMusicSource = m_BackgroundMusicPlayer.GetComponent<AudioSource>();
        if (backgroundMusicSource == null)
            return false;

        if (backgroundMusicSource.isPlaying)
            return false;

        if (m_IsChangingSong)
            return false;

        return true;
    }

    public void StartNewMusic(EGameState InGameState)
    {
        List<Music> songsByGameState = new List<Music>();
        foreach(Music music in m_Music)
        {
            if(music.StateType == InGameState)
            {
                songsByGameState.Add(music);
            }
        }

        if(songsByGameState.Count <= 0)
        {
            Debug.LogWarning("No Song Found For GameState: " + InGameState.ToString());
            return;
        }

        int randomSongIndex = Random.Range(0, songsByGameState.Count);

        Music chosenSong = songsByGameState[randomSongIndex];

        StartCoroutine(ChangeSong(chosenSong.MusicClip));
    }

    public void SetMusicVolume(Slider InSlider)
    {
        m_MusicSoundMulitplier = InSlider.value;

        if(m_BackgroundMusicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.volume = InSlider.value;
        }

        GameState.Instance.SetMusicMultiplier(m_MusicSoundMulitplier);
    }

    public void SetSFXVolume(Slider InSlider)
    {
        m_SFXSoundMulitplier = InSlider.value;

        GameState.Instance.SetSFXMultiplier(m_SFXSoundMulitplier);
    }

    IEnumerator ChangeSong(AudioClip InNewSong)
    {
        m_IsChangingSong = true;
        AudioSource backgroundMusicSource = m_BackgroundMusicPlayer.GetComponent<AudioSource>();

        // Check if there is a background music source
        if (backgroundMusicSource == null)
        {
            Debug.LogError("Background music source not found!");
            m_IsChangingSong = false;
            yield break;
        }

        // Start fading out the current song
        float startVolume = m_MusicSoundMulitplier;
        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= Time.deltaTime * 0.4f;
            yield return null;
        }

        // Change the clip
        backgroundMusicSource.clip = InNewSong;

        // Start fading in the new song
        backgroundMusicSource.Play();

        // Ensure volume is set to the desired level
        backgroundMusicSource.volume = startVolume;
        m_IsChangingSong = false;
    }
}
