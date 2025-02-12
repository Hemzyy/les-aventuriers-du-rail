using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volume_settings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;

    public void SetMusicVolume()
    {
        float volume = slider.value;
        mixer.SetFloat("Music", volume);
    }
}
