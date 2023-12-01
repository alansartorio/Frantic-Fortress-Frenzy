using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButons : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
