using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Animation anim;
    [SerializeField] private string fadeInAnimationName;
    [SerializeField] private string fadeOutAnimationName;

    private TextMeshProUGUI textMeshPro;


    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void HandleHoverEnter()
    {
        textMeshPro.fontStyle = FontStyles.Underline;
        anim.Play(fadeInAnimationName);
    }

    public void HandleHoverExit()
    {
        textMeshPro.fontStyle = FontStyles.Normal;
        anim.Play(fadeOutAnimationName);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
