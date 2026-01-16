using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DescriptionModal : MonoBehaviour
    {
        private Image _image;
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
        }
        
        public void Show(IngredientSO ingredient)
        {
            if (ingredient == null) return;
            if (_image == null) _image = GetComponentInChildren<Image>();
            if (_text == null) _text = GetComponentInChildren<TextMeshProUGUI>();
            
            _image.sprite = ingredient.icon;
            _text.text = ingredient.description;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}