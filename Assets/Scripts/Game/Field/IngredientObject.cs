using System;
using BandoWare.GameplayTags;
using Game;
using UnityEngine;

/// <summary>
/// 씬에서 실제 재료를 시각적으로 보여주는 오브젝트
/// IngredientSO 데이터를 받아서 스프라이트를 렌더링
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class IngredientObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 현재 이 오브젝트가 표현하고 있는 데이터
    /// </summary>
    public IngredientSO Data { get; private set; }
    
    public GameplayTag IngredientTag => Data != null ? Data.Tag : GameplayTag.None;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 데이터를 입력받아서 스프라이트를 갱신
    /// </summary>
    /// <param name="data">표시할 재료 데이터</param>
    public void Initialize(IngredientSO data)
    {
        Data = data;

        if (spriteRenderer != null & data != null)
        {
            if(data.icon != null)
            {
                spriteRenderer.sprite = data.icon;
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = data.debugColor;
            }
        }
    }

    /// <summary>
    /// 재료 오브젝트의 이미지를 바꾸는 함수
    /// </summary>
    /// <param name="icon">바꿀 아이콘</param>
    public void SetIngredientIcon (Sprite icon)
    {
        spriteRenderer.sprite = icon;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
