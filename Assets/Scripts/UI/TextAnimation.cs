using System.Collections;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float timeBetweenLetters;
    [SerializeField] private float timeBetweenWords;

    private int i = 0;
    public string[] stringArray;

    // Use this for initialization
    void Start()
    {
        EndCheck();
    }

    private IEnumerator ShowText()
    {
        textMeshPro.ForceMeshUpdate();
        int visibleChars = textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (visibleChars + 1);
            textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= visibleChars)
            {
                i++;
                Invoke(nameof(EndCheck), timeBetweenWords);
            }

            counter++;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    private void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            textMeshPro.text = stringArray[i];
            StartCoroutine(ShowText());
        }
    }
}