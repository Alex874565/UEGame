using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("Lose Popup")]
    [SerializeField] private Animator losePopupAnimator;
    [SerializeField] private TextMeshProUGUI losePopupTextHeader;
    [SerializeField] private TextMeshProUGUI losePopupTextDescription;

    private int popup1AnimHash = Animator.StringToHash("Popup");

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ResourceManager.Instance.LoseAction += LosePopup;
    }

    public void LosePopup(string headerText, string description)
    {
        losePopupTextHeader.text = headerText;
        losePopupTextDescription.text = description;

        losePopupAnimator.Play(popup1AnimHash);
    }
}
