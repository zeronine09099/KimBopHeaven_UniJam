using System;
using Core;
using UnityEngine;

namespace Reward
{
    public class RewardUI : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.RewardUI = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            
        }
        
        private void OnDisable()
        {
            
        }
    }
    
}