
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGameDark()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void SetAudioValue(float value) {
       GameObject music = GameObject.FindGameObjectWithTag("music");
       music.GetComponent<AudioSource>().volume = value;
    }
}
