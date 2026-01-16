using System.Collections;
using Common.Singleton;
using Encounter;
using Game;
using Game.UI;
using Reward;
using Title;
using UnityEngine;

namespace Core
{
    public class UIManager : Singleton<UIManager>
    {
        protected override void AfterAwake()
        {
            
        }
        
        public IEnumerator Init()
        {
            yield return null;
        }

        [field:SerializeField] public TitleUI TitleUI { get; set; }
        [field:SerializeField] public IngameUI InGameUI { get; set; }
        [field:SerializeField] public RewardUI RewardUI { get; set; }
        [field:SerializeField] public EncounterUI EncounterUI { get; set; }
    }
    
}