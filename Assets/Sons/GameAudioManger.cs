using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManger : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioClip buttonClickSound;
    public AudioClip cards;
    public AudioClip victory;
    public AudioClip errorMessage;
    public static GameAudioManger Instance;

    void Awake()
    {
        // Trouve l'instance du AudioManager principal
        PersistentAudioManager mainAudioManager = FindObjectOfType<PersistentAudioManager>();

        // Si une instance du AudioManager principal est trouvée, détruit-la
        if (mainAudioManager != null)
        {
            Destroy(mainAudioManager.gameObject);
        }

        Instance = this;
        PlayMusic();

    }

    public void PlayMusic()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.loop = true;  // Définit la musique pour qu'elle soit jouée en boucle
            musicSource.Play();       // Joue la musique
        }
        else
        {
            Debug.LogError("Music AudioSource or clip is not set.");
        }
    }

    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            musicSource.PlayOneShot(buttonClickSound);
        }
    }

    public void PlayCards()
    {
        if (cards != null)
        {
            musicSource.PlayOneShot(cards);
        }
    }

    public void PlayVictory()
    {
        if (victory != null)
        {
            musicSource.PlayOneShot(victory);
        }
    }

    public void PlayErrorSound()
    {
        if (errorMessage != null)
        {
            musicSource.PlayOneShot(errorMessage);
        }
    }
}
