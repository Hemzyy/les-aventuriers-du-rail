using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAudioManager : MonoBehaviour
{
    public static PersistentAudioManager Instance;
    public AudioSource musicSource;
    private bool musicPlaying = false;     // Indique si la musique est en train d'être jouée
    public AudioClip buttonClickSound;
    public AudioClip errorMessage;

    public void Awake()
    {
        // Si aucune instance AudioManager n'existe
        if (Instance == null || Instance != this)
        {
            // Définit l'instance actuelle sur cette instance AudioManager
            Instance = this;

            // Ne pas détruire cet AudioManager lors du chargement d'une nouvelle scène
            DontDestroyOnLoad(gameObject);

            // Joue la musique de fond au démarrage si elle n'est pas déjà en cours de lecture
            if (!musicPlaying)
            {
                PlayMusic();
                Debug.Log("Nouvelle instance du AudioManager créée.");
            }
        }
        else
        {
            // Si une instance AudioManager existe déjà, détruit celle-ci
            Destroy(gameObject);
            Debug.Log("Ancienne instance du AudioManager détruite.");
        }
    }



    public void PlayMusic()
    {
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.loop = true;  // Définit la musique pour qu'elle soit jouée en boucle
            musicSource.Play();       // Joue la musique
            musicPlaying = true;
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
            //musicSource.PlayOneShot(buttonClickSound);
        }
    }

    public void PlayErrorMessage()
    {
        if (errorMessage != null)
        {
            musicSource.PlayOneShot(errorMessage);
        }
    }
}
