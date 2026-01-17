using Core;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Toggle toggle;
        
        private void Awake()
        {
            UIManager.Instance.SettingUI = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            masterSlider.value = Sound.SoundManager.Instance.MasterVolume;
            musicSlider.value = Sound.SoundManager.Instance.MusicVolume;
            sfxSlider.value = Sound.SoundManager.Instance.SfxVolume;
            toggle.isOn = Sound.SoundManager.Instance.UseVibration;
            
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            toggle.onValueChanged.AddListener(OnVibrationToggled);
        }
        
        private void OnDisable()
        {
            masterSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
            toggle.onValueChanged.RemoveListener(OnVibrationToggled);
        }
        
        private void OnMasterVolumeChanged(float value)
        {
            Sound.SoundManager.Instance.SetMasterVolume(value);
        }
        private void OnMusicVolumeChanged(float value)
        {
            Sound.SoundManager.Instance.SetMusicVolume(value);
        }

        private void OnSfxVolumeChanged(float value)
        {
            Sound.SoundManager.Instance.SetSfxVolume(value);
        }
        
        private void OnVibrationToggled(bool isOn)
        {
            Sound.SoundManager.Instance.UseVibration = isOn;
            SoundManager.Instance.PlaySfx(SoundReference.ButtonClickSFX);
            if (isOn)
            {
                Sound.SoundManager.Instance.VibePop();
            }
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
}