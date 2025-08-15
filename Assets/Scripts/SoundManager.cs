/*
* Author: Lim En Xu Jayson
* Date: 16/8/2025
* Description: Handles universal sound effects and music.
*/
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
    /// <summary>
    /// EndLevelSound is the audio clip played when the level ends.
    /// </summary>
    [SerializeField]
    private AudioClip endLevelSound;
    /// <summary>
    /// Called when the menu button is clicked. Plays sound effects for menu interactions.
    /// </summary>
    public void MenuButtonClicked()
    {
        audioSource.PlayOneShot(menuClickSound);
    }
    /// <summary>
    /// Called when the menu music should be played.
    /// </summary>
    public void MenuMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = menuMusic;
        audioSource.Play();
    }
    /// <summary>
    /// Called when the level music should be played.
    /// </summary>
    public void LevelMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = levelMusic;
        audioSource.Play();
    }
    /// <summary>
    /// Called when the office music should be played.
    /// </summary>
    public void OfficeMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.clip = officeMusic;
        audioSource.Play();
    }
    /// <summary>
    /// Called when the player enters a new level.
    /// </summary>
    public void EnterLevelSound()
    {
        audioSource.PlayOneShot(whooshSound);
    }
    /// <summary>
    /// Called when the level ends.
    /// </summary>
    public void EndLevelSound()
    {
        audioSource.Stop(); // Stop any currently playing music
        audioSource.PlayOneShot(endLevelSound);
    }
    /// <summary>
    /// Called when the music should be stopped.
    /// </summary>
    public void StopMusic()
    {
        audioSource.Stop(); // Stop any currently playing music
    }
}
