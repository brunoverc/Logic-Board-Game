using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text dialogText;
    public Text dialogTitle;
    public Button okButton;

    void Start()
    {
        if (dialogPanel != null) 
        {
            dialogPanel.SetActive(false);
        }
        
        if (okButton != null) 
        {
            okButton.onClick.AddListener(HideDialog);
        }
    }

    public void ShowDialog(string message, string title)
    {
        if (dialogText != null && dialogPanel != null) 
        {
            dialogText.text = message;
            dialogTitle.text = title;
            dialogPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Dialog Text or Dialog Panel is not assigned in the Inspector");
        }
    }

    void HideDialog()
    {
        if (dialogPanel != null) 
        {
            dialogPanel.SetActive(false);
        }
    }
}
