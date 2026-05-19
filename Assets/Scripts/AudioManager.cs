using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource; // Background music audio source
    public Slider musicSlider; // UI slider for music volume control

    void Start()
    {
        musicSlider.value = musicSource.volume; //Sync slider with current volume at start
        musicSlider.onValueChanged.AddListener(SetVolume); //When slider moves -> call SetVolume()
    }
    void SetVolume(float value)
    {
        musicSource.volume = value; //For changing the actual music volume based on slider value
    }
}
