using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButons : MonoBehaviour
{
    private GameObject _menuButtons;
    private GameObject _loadingSection;
    private GameObject _loadingText;
    
    private Timer _loadAnimationTimer;

    private static readonly int maxDots = 4;
    private int currentDots = 0;
    
    private void Start()
    {
        _menuButtons = GameObject.Find("MenuButtons");
        _loadingSection = GameObject.Find("LoadingSection");
        _loadingText = GameObject.Find("LoadingText");
        _loadingSection.SetActive(false);
        _loadAnimationTimer = new Timer(1000f, true);
        _loadAnimationTimer.onTick.AddListener(LoadingAnimationStep);
        _loadAnimationTimer.Stop();
    }

    public void Play()
    {
        _menuButtons.SetActive(false);
        _loadingSection.SetActive(true);
        _loadAnimationTimer.Restart();
        StartCoroutine(LoadYourAsyncScene());
        // SceneManager.LoadScene("SampleScene");
    }

    private void LoadingAnimationStep()
    {
        var textMesh = _loadingText.GetComponent<TextMeshProUGUI>();
        var dots = "";
        for(int i = 0; i < currentDots; i++)
        {
            dots += ".";
        }
        textMesh.text = "Loading"+dots;
        currentDots = (currentDots + 1) % maxDots;
    }
    
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
