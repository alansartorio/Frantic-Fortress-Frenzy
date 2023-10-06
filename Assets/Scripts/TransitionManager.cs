using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{

    public void YouLost()
    {
        GameObject youLostMenu = GameObject.Find("YouLostMenu");
        
        CanvasGroup canvasGroup = youLostMenu.GetComponentInChildren<CanvasGroup>();
        
        canvasGroup.alpha = 0.4f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}