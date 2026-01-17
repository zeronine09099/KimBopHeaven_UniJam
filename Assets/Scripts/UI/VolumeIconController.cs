using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeIconController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private Image icon;
    [SerializeField] private Sprite volumeIconImage_mute;
    [SerializeField] private Sprite volumeIconImage_on;

    private void Awake()
    {
        icon = GetComponent<Image>();
        volumeSlider = GetComponentInChildren<Slider>();

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        UpdateVolumeIcon(volumeSlider.value);
    }

    private void OnVolumeChanged(float value)
    {
        UpdateVolumeIcon(value);
    }

    private void UpdateVolumeIcon(float volume)
    {
        if (Math.Abs(volume) < 0.01f)
        {
            icon.sprite = volumeIconImage_mute;
        }
        else
        {
            icon.sprite = volumeIconImage_on;
        }
    }
}
