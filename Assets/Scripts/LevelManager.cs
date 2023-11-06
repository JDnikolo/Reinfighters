using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public void LoadGameVSAI(){
        SceneManager.LoadScene("GameVSAI");
    }
    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadPractice(){
        SceneManager.LoadScene("Practice");
    }

    public void Quit(){
        Application.Quit();
    }

    public void Escape(){
        if (SceneManager.GetActiveScene().name=="Practice"){
            LoadMainMenu();
        }
    }
}
