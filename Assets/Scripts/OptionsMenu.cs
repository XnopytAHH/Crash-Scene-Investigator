/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles option menu settings
*/
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
public class OptionsMenu : MonoBehaviour
{
    /// <summary>
    /// Audio mixer for controlling sound levels of the whole game
    /// </summary>
    public AudioMixer audioMixer;
    /// <summary>
    /// Text element for displaying the current volume level
    /// </summary>
    public TextMeshProUGUI volumeText;
    /// <summary>
    /// Initializes the options menu default settings0
    /// </summary>
    void Awake()
    {
        volumeText.text = "6"; 
    }
    /// <summary>
    /// Sets the volume level for the game through the audio mixer
    /// </summary>
    public void SetVolume(float volume)
    {
        volumeText.text = volume.ToString("0");
        volume = Mathf.InverseLerp(0f, 10f, volume);
        volume = Mathf.Lerp(-30f, 10f, volume); // Convert the slider value (0 to 1) to a logarithmic scale (-30 to 50 dB)
        audioMixer.SetFloat("MasterVolume", volume); // Set the volume in the audio mixer
    }
}
