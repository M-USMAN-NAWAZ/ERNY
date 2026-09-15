using System.Collections;
using TMPro;
using UnityEngine;

public class CopyReadOnlyInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void HighlightAndCopy()
    {
        StartCoroutine(HighlightAndCopyRoutine());
    }

    private IEnumerator HighlightAndCopyRoutine()
    {
        // Focus the field
        inputField.ActivateInputField();

        // Wait one frame so TMP processes the focus
        yield return null;

        // Highlight all text
        inputField.selectionAnchorPosition = 0;
        inputField.selectionFocusPosition = inputField.text.Length;

        // Copy to clipboard
        GUIUtility.systemCopyBuffer = inputField.text;

        Debug.Log("Copied: " + inputField.text);
    }
}