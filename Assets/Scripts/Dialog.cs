using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private string[] texts =
    {
        "Hey!",
        "I am Cody.",
        "I was trapped inside this maze years ago.",
        "And it's completely dark out here.",
        "I was able to find a place with a bit light.",
        "Some things are being told about this place.",
        "1. There are three paths which I could follow",
        "2. Only two lead to an exit",
        "3. Just one exit is open at the time.",
        "I am too scared to leave.",
        "I don't know what's out there.",
        "Can you help me?"
    };

    private int _currentIndex;

    private void Start()
    {
        text.text = texts[0];    
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ProgressDialog();
        }
    }

    private void ProgressDialog()
    {
        if (_currentIndex == texts.Length - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            return;
        }

        _currentIndex++;
        text.text = texts[_currentIndex];
    }
}
