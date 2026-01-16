using System;
using Core;
using UnityEngine;

namespace UI.Encounter
{
    public class EncounterUI : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.EncounterUI = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            
        }
        
        private void OnDisable()
        {
            
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}