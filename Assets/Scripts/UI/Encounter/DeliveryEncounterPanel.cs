using BandoWare.GameplayTags;
using System.Collections.Generic;
using Player;
using UI.Encounter;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

struct EncounterStruct
{
    public GameplayTag targetTag;
    public int upgradeValue;
    public Sprite encounterImage;
    public Sprite encounterResultImage;
    public string buttonText;

    public EncounterStruct(GameplayTag targetTag, int upgradeValue, Sprite encounterImage, Sprite encounterResult, string buttonText)
    {
        this.targetTag = targetTag;
        this.upgradeValue = upgradeValue;
        this.encounterImage = encounterImage;
        this.encounterResultImage = encounterResult;
        this.buttonText = buttonText; 
    }
}

public class DeliveryEncounterPanel : MonoBehaviour
{
    [SerializeField] private Image displayImage1;
    [SerializeField] private Image displayImage2;
    [SerializeField] private TextMeshProUGUI leftButtonText;
    [SerializeField] private TextMeshProUGUI rightButtonText;
    [SerializeField] private Sprite meatShopImage;
    [SerializeField] private Sprite fishShopImage;
    [SerializeField] private Sprite vegetableShopImage;
    [SerializeField] private Sprite meatResultImage;
    [SerializeField] private Sprite fishResultImage;
    [SerializeField] private Sprite vegetableResultImage;
    [SerializeField] private GameplayTag upgradeTargetTag;
    [SerializeField] private DeliveryEncounterResult deliveryEncounterResult;
    [SerializeField] private int upgradeValue;
    [SerializeField] List<EncounterStruct> encounterStructs;
    private Sprite selectedResultImg;
    private EncounterUI encounterUI;
    private int leftIndex;
    private int rightIndex;
    bool upgradeSelected = false;
    
    public GameplayTag UpgradeTargetTag => upgradeTargetTag;
    public int UpgradeValue => upgradeValue;

    public void Initialize(EncounterUI encounterUI, int selectedA, int selectedB)
    {
        Debug.Log("deliveryEncounterPanel 이니셜라이즈 진입");
        this.encounterUI = encounterUI;
        this.leftIndex = selectedA;
        this.rightIndex = selectedB;
        encounterStructs = new List<EncounterStruct>();
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Meat.Get(), 10, meatShopImage, meatResultImage,  "정육점을 믿기"));
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Vegetable.Get(), 10, vegetableShopImage, vegetableResultImage ,"채소 가게를 믿기"));
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Seafood.Get(), 10, fishShopImage, fishResultImage, "수산시장을 믿기"));

        displayImage1.sprite = encounterStructs[selectedA].encounterImage;
        displayImage2.sprite = encounterStructs[selectedB].encounterImage;
        leftButtonText.text = encounterStructs[selectedA].buttonText;
        rightButtonText.text = encounterStructs[selectedB].buttonText;
    }
    void Update()
    {
        
    }

    public void OnClickLeftButton()
    {
        upgradeTargetTag = encounterStructs[leftIndex].targetTag;
        upgradeValue = encounterStructs[leftIndex].upgradeValue;
        selectedResultImg = encounterStructs[leftIndex].encounterResultImage;
        upgradeSelected = true;
    }

    public void OnClickRightButton()
    {
        upgradeTargetTag = encounterStructs[rightIndex].targetTag;
        upgradeValue = encounterStructs[rightIndex].upgradeValue;
        selectedResultImg = encounterStructs[rightIndex].encounterResultImage;
        upgradeSelected = true;
    }

    public void OnClickSelectButton()
    {
        if (upgradeSelected == false)
        {
            return;
        }

        deliveryEncounterResult.gameObject.SetActive(true);
        deliveryEncounterResult.Initialize(upgradeValue, upgradeTargetTag, selectedResultImg);
        gameObject.SetActive(false);
        // 가기전에 확인 이미지 판넬 띄우기
    }

}

