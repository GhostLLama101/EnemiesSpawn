using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{
    public AudioMixer mixer;
    // make bools for the active screne like credis or setings so it can go back
    public void PlayeGame()
    {
        // sceen transition to the main scene
        Debug.Log("PlayeGame");
        SceneManager.LoadScene("Main");
    }

    public void credits()
    {
        Debug.Log("Credits");
        // awake the credits menu
    }
    
    public void quit() // close the game
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void optionsMenu()
    {
        // make the options menu appear
    }

    public void setVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }
}
