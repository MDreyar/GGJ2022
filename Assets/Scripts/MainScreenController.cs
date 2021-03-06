using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainScreenController : MonoBehaviour
{
    public void StartGame(TextMeshProUGUI buttonText) {
        buttonText.text = "Now Loading!";
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void BackToMainMenu() {
        SceneManager.LoadSceneAsync(0);
    }
}
