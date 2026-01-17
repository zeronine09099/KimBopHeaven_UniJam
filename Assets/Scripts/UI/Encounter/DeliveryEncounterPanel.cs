using BandoWare.GameplayTags;
using System.Collections.Generic;
using UI.Encounter;
using UnityEngine;
using UnityEngine.UI;

struct EncounterStruct
{
    public GameplayTag targetTag;
    public int upgradeValue;
    public Image encounterImage;

    public EncounterStruct(GameplayTag targetTag, int upgradeValue, Image encounterImage)
    {
        this.targetTag = targetTag;
        this.upgradeValue = upgradeValue;
        this.encounterImage = encounterImage;
    }
}

public class DeliveryEncounterPanel : MonoBehaviour
{
    private Image displayImage1;
    private Image displayImage2;
    [SerializeField] private Image meatShopImage;
    [SerializeField] private Image fishShopImage;
    [SerializeField] private Image vegetableShopImage;
    [SerializeField] private GameplayTag upgradeTargetTag;
    [SerializeField] private DeliveryEncounterResult deliveryEncounterResult;
    [SerializeField] private int upgradeValue;
    List<EncounterStruct> encounterStructs;
    private EncounterUI encounterUI;
    private int leftIndex;
    private int rightIndex;
    bool upgradeSelected = false;

    public void Initialize(EncounterUI encounterUI, int selectedA, int selectedB)
    {
        this.encounterUI = encounterUI;
        this.leftIndex = selectedA;
        this.rightIndex = selectedB;
        encounterStructs = new List<EncounterStruct>();
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Meat.Get(), 10, meatShopImage));
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Vegetable.Get(), 10, vegetableShopImage));
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Seafood.Get(), 10, vegetableShopImage));

        displayImage1 = encounterStructs[selectedA].encounterImage;
        displayImage2 = encounterStructs[selectedB].encounterImage;
    }
    void Update()
    {
        
    }

    public void OnClickLeftButton()
    {
        upgradeTargetTag = encounterStructs[leftIndex].targetTag;
        upgradeValue = encounterStructs[leftIndex].upgradeValue;
        upgradeSelected = true;
    }

    public void OnClickRightButton()
    {
        upgradeTargetTag = encounterStructs[rightIndex].targetTag;
        upgradeValue = encounterStructs[rightIndex].upgradeValue;
        upgradeSelected = true;
    }

    public void OnClickSelectButton()
    {
        if (upgradeSelected == false)
        {
            return;
        }

        deliveryEncounterResult.Initialize(upgradeValue, upgradeTargetTag);
        // 가기전에 확인 이미지 판넬 띄우기
    }

}

