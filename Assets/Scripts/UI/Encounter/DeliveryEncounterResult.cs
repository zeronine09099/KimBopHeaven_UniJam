using BandoWare.GameplayTags;
using Core;
using Database;
using Database.Generated;
using Interaction;
using TMPro;
using UI.Encounter;
using UnityEngine;

public class DeliveryEncounterResult : MonoBehaviour
{

    private GameplayTag targetTag;
    private string koreanName ="";
    private int upgradeValue;
    [SerializeField] TextMeshProUGUI deliveryResultDescription;

    public void Initialize(int upgradeValue, GameplayTag targetTag)
    {
        Debug.Log("delivery 결과창 초기화 진입");
        this.targetTag = targetTag;
        this.upgradeValue = upgradeValue;
        foreach (var encounterReward in DatabaseManager.Instance.Database.EncounterRewardList)
        {
            if(encounterReward.target == targetTag)
            {
                koreanName = encounterReward.koreanName;
            }

        }
        deliveryResultDescription.text = $"모든 {koreanName}류의 기본점수가 {upgradeValue} 점 증가하였습니다";
    }
    
    public void OnClickCheckButton()
    {
        Debug.Log("delivery 결과창 확인 버튼 진입");
        UIManager.Instance.EncounterUI.OnDeliveryEncounterClick();
        this.gameObject.SetActive(false);
        return;
    }

}
