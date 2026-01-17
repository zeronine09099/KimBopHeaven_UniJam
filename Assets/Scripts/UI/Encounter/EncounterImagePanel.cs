using BandoWare.GameplayTags;
using NUnit.Framework;
using System.Collections.Generic;
using UI.Encounter;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

struct EncounterStruct
{
    private GameplayTag targetTag;
    private int upgradeValue;
    private Image encounterImage;

    public EncounterStruct(GameplayTag targetTag, int upgradeValue, Image encounterImage)
    {
        this.targetTag = targetTag;
        this.upgradeValue = upgradeValue;
        this.encounterImage = encounterImage;
    }
}

public class EncounterImagePanel : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    private Image displayImage1;
    private Image displayImage2;
    [SerializeField] private Image meatShopImage;
    [SerializeField] private Image fishShopImage;
    [SerializeField] private Image vegetableShopImage;
    [SerializeField] private GameplayTag upgradeTargetTag;
    [SerializeField] private int upgradeValue;
    List<EncounterStruct> encounterStructs;
    private EncounterUI encounterUI;

    public void Initialize(EncounterUI encounterUI, int selectedA, int selectedB)
    {
        this.encounterUI = encounterUI;
        encounterStructs = new List<EncounterStruct>();
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Meat.Get(), 10, meatShopImage));
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Vegetable.Get(), 10, vegetableShopImage));
        encounterStructs.Add(new(AllGameplayTags.Ingredient.Seafood.Get(), 10, vegetableShopImage));

    }
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        encounterUI.OnEncounterClick();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}

