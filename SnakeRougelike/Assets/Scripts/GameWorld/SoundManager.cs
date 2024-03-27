using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    OnPlayerDieEffect
}

[Serializable]
public class SFX
{
    public SFXType Type;
    public AudioClip SoundFXClip;

    [Range(0, 1)]
    public float Volume = 1;
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

    [Header("General Sounds")]
    [SerializeField] private List<SFX> m_SFX; 

    public void PlaySound(SFX InAudioClip, bool InRandomPitch)
    {
        GameObject soundObject = new GameObject("TemporarySoundObject");

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = InAudioClip.SoundFXClip;
        audioSource.volume = InAudioClip.Volume;

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
}
