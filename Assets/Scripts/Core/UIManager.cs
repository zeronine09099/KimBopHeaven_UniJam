using System.Collections;
using Common.Singleton;
using UI;
using UI.Encounter;
using UI.Game;
using UI.Reward;
using UI.Title;
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
        [field:SerializeField] public DeckUI DeckUI { get; set; }
        [field:SerializeField] public SettingUI SettingUI { get; set; }
        [field:SerializeField] public BillingUI BillingUI { get; set; }
        [field:SerializeField] public GameoverUI GameoverUI { get; set; }


        public void GoToTitleUI()
        {
            TitleUI.Show();
            InGameUI.Hide();
            RewardUI.Hide();
            EncounterUI.Hide();
            DeckUI.Hide();
            SettingUI.Hide();
            BillingUI.Hide();
            GameoverUI.Hide();
        }
        
        public void GoToInGameUI()
        {
            TitleUI.Hide();
            InGameUI.Show();
            RewardUI.Hide();
            EncounterUI.Show();
            DeckUI.Hide();
            SettingUI.Hide();
            BillingUI.Hide();
            GameoverUI.Hide();
        }

        public void HideAll()
        {
            TitleUI.Hide();
            InGameUI.Hide();
            RewardUI.Hide();
            EncounterUI.Hide();
            DeckUI.Hide();
            SettingUI.Hide();
            BillingUI.Hide();
            GameoverUI.Hide();
        }
    }
    
}