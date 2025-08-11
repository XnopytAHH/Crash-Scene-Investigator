using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TextMeshProUGUI volumeText;
    void Awake()
    {
        volumeText.text = "10"; 
    }
    public void SetVolume(float volume)
    {
        volumeText.text = volume.ToString("0");
        volume = Mathf.InverseLerp(0f, 10f, volume);
        volume = Mathf.Lerp(-80f, 0f, volume); // Convert the slider value (0 to 1) to a logarithmic scale (-80 to 0 dB)
        audioMixer.SetFloat("MasterVolume", volume); // Set the volume in the audio mixer
    }
}
