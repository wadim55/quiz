using UnityEngine;
using UnityEngine.UI;

public class WordData : MonoBehaviour
{
    [SerializeField] private Text wordText;

    [HideInInspector]
    public string wordValue;

    private Button buttonComponent;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
        if (buttonComponent)
        {
            buttonComponent.onClick.AddListener(() => WordSelected());
        }
    }

    public void SetWord(string value)
    {
        wordText.text = value + "";
        wordValue = value;
    }

    private void WordSelected()
    {
        QuizManager.instance.SelectedOption(this);
    }
    
}
