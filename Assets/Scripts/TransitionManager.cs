using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public GameObject youLostMenu;

    public void YouLost()
    {
        youLostMenu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}