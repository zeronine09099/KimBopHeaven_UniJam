using BandoWare.GameplayTags;
using Core;
using Interaction;
using TMPro;
using UI.Encounter;
using UnityEngine;

public class DeliveryEncounterResult : MonoBehaviour
{

    private GameplayTag targetTag;
    private int upgradeValue;
    [SerializeField] TextMeshProUGUI deliveryResultDescription;

    public void Initialize(int upgradeValue, GameplayTag targetTag)
    {
        this.targetTag = targetTag;
        this.upgradeValue = upgradeValue;
        deliveryResultDescription.text = $"모든 {targetTag.Name}류의 기본점수가 {upgradeValue} 점 증가하였습니다";
    }
    
    public void OnClickCheckButton()
    {
        UIManager.Instance.EncounterUI.OnDeliveryEncounterClick();
        this.gameObject.SetActive(false);
        return;
    }

}
