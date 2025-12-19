using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    // Load scene asynchronously when this object starts
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}