using Core;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.TutorialUI = this;
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
