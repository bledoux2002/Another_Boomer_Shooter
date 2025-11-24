using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button resumeButton;

    void Start()
    {
        if (resumeButton != null) ;
        resumeButton.onClick.AddListener(() => GameManager.Instance.Resume());
    }
}