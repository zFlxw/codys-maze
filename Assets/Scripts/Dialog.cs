using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private Image overlay;

    [Header("Text Reference")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timeBetweenLetters;
    [SerializeField] private float timeBetweenWords;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip[] typingClips;
    [SerializeField] private AudioClip typingSpaceClip;
    [SerializeField] private float typingVolume;
    [SerializeField] private float typingSpaceVolume;

    private string[] texts =
    {
        "Hey!",
        "I am Cody.",
        "I was trapped inside this maze years ago.",
        "And it's completely dark out here.",
        "I was able to find a place with a bit of light.",
        "Some things are being told about this place.",
        "1. There are three paths which I could follow",
        "2. Only two lead to an exit",
        "3. Just one exit is open at the time",
        "I am too scared to leave alone.",
        "I don't know what's out there.",
        "Can you help me?"
    };

    private int _currentIndex;
    private bool _isRunning = false;

    private void Start()
    {
        overlay.color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        EndCheck();
    }

    private void Update()
    {
        EndCheck();
    }

    private IEnumerator ShowText()
    {
        _isRunning = true;
        text.ForceMeshUpdate();
        int visibleChars = text.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (visibleChars + 1);
            text.maxVisibleCharacters = visibleCount;
            sfxAudioSource.PlayOneShot(typingClips[Random.Range(0, typingClips.Length)], typingVolume);

            if (visibleCount >= visibleChars)
            {
                _currentIndex++;
                sfxAudioSource.PlayOneShot(typingSpaceClip, typingSpaceVolume);
                _isRunning = false;
                break;
            }

            counter++;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    private void EndCheck()
    {
        if (_isRunning)
        {
            return;
        }

        if ((_currentIndex <= texts.Length && Keyboard.current.spaceKey.wasPressedThisFrame) || _currentIndex == 0)
        {
            if (_currentIndex <= texts.Length - 1)
            {
                text.text = texts[_currentIndex];
                StartCoroutine(ShowText());
            }
            else
            {
                _isRunning = true;
                DOTween
                    .Sequence()
                    .Append(DOTween.To(() => overlay.fillAmount, x => overlay.fillAmount = x, 1.0f, 0.5f))
                    .OnComplete(() =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    });
            }
        }
        /*else if (_currentIndex == texts.Length && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _isRunning = true;

            //overlay.color = Color.Lerp(overlay.color, new Color(255.0f, 255.0f, 255.0f, 255.0f), 2.0f);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }*/
    }
}
