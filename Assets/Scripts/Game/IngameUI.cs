using System;
using Core;
using Machamy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    
    public class IngameUI : MonoBehaviour
    {

        private void Awake()
        {
            UIManager.Instance.InGameUI = this;
        }

        private void OnEnable()
        {
       
        }

        private void OnDisable()
        {

        }
        
    }
    
}