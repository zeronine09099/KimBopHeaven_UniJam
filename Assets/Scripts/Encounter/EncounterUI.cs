using System;
using Core;
using UnityEngine;

namespace Encounter
{
    public class EncounterUI : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.EncounterUI = this;
        }

        private void OnEnable()
        {
            
        }
        
        private void OnDisable()
        {
            
        }
    }
}