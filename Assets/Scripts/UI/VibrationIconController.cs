using UnityEngine;
using UnityEngine.UI;

public class VibrationIconController : MonoBehaviour
{
    [SerializeField] private Toggle vibrationToggle;

    private Image icon;
    [SerializeField] private Sprite volumeIconImage_off;
    [SerializeField] private Sprite volumeIconImage_on;

    private void Awake()
    {
        icon = GetComponent<Image>();
        vibrationToggle = GetComponentInChildren<Toggle>();

        vibrationToggle.onValueChanged.AddListener(OnToggleChanged);
        UpdateVibrationIcon(vibrationToggle.isOn);
    }

    private void OnToggleChanged(bool value)
    {
        UpdateVibrationIcon(value);
    }

    private void UpdateVibrationIcon(bool isOn)
    {
        if (isOn)
        {
            icon.sprite = volumeIconImage_on;
        }
        else
        {
            icon.sprite = volumeIconImage_off;
        }
    }
}
