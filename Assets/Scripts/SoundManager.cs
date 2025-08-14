using UnityEngine;

public class SoundManager : MonoBehaviour
{
    ///<summary>
    /// MenuClickSound is the audio clip played when the menu button is clicked.
    /// </summary>
    [SerializeField]
    private AudioClip menuClickSound;
    ///<summary>
    /// audio source for playing sounds
    /// </summary>
    public AudioSource audioSource;
    ///<summary>
    /// menuMusic is the audio clip played during the menu.
    /// </summary>
    [SerializeField]
    private AudioClip menuMusic;
    ///<summary>
    /// levelMusic is the audio clip played during the level.
    /// </summary>
    [SerializeField]
    private AudioClip levelMusic;
    /// <summary>
    /// officeMusic is the audio clip played during the office.
    /// </summary>
    [SerializeField]
    private AudioClip officeMusic;
    /// <summary>
    /// WhooshSound is the audio clip played when entering a level
    /// </summary>
    [SerializeField]
    private AudioClip whooshSound;
    [SerializeField]
    private AudioClip endLevelSound;
    public void MenuButtonClicked()
    {
        audioSource.PlayOneShot(menuClickSound);
    }
    public void MenuMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = menuMusic;
        audioSource.Play();
    }
    public void LevelMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = levelMusic;
        audioSource.Play();
    }
    public void OfficeMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = officeMusic;
        audioSource.Play();
    }
    public void EnterLevelSound()
    {
        audioSource.PlayOneShot(whooshSound);
    }
    public void EndLevelSound()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.PlayOneShot(endLevelSound);
    }
}
