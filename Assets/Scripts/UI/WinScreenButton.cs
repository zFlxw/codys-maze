using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void HandleHoverEnter()
    {
        textMeshPro.fontStyle = FontStyles.Underline;
    }

    public void HandleHoverExit()
    {
        textMeshPro.fontStyle = FontStyles.Normal;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}