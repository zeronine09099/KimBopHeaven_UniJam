using System;
using Common;
using DG.Tweening;
using Interaction;
using Machamy.Attributes;
using Machamy.Utils;
using UI;
using UnityEngine;

namespace Game.Field
{
    /// <summary>
    /// 타일은 재료 오브젝트를 들고 있고
    /// 오브젝트의 데이터를 바꾸거나 리턴할 수 있음
    /// </summary>
    public class Tile : MonoBehaviour, IInteractable
    {
        [field:SerializeField,VisibleOnly]public TileVector Coordinate { get; set; }
        [field:SerializeField]public IngredientObject CurrentIngredient { get; set; }
        private Field field;
        [SerializeField] FloatingBonusScore scoreTextPrefab;
        [field:SerializeField, VisibleOnly(EditableIn.EditMode)]
        public FloatingBonusScore scoreText { get; private set; }
[SerializeField] private Transform scoreTextAnchor;
        public void Initialize(Field field, TileVector tileVector)
        {
            this.field = field;
            Coordinate = tileVector;
        }

        private void Start()
        {
            if(scoreText == null)
            {
                scoreText = Instantiate(scoreTextPrefab, transform);
                scoreText.gameObject.SetActive(false);
                if(scoreText.GetComponent<RectTransform>())
                {
                    if (FloatingUICanvas.HasInstance)
                    {
                        scoreText.transform.SetParent(FloatingUICanvas.Instance.transform, true);
                        Vector2 screenPos =
                            RectTransformUtility.WorldToScreenPoint(Camera.main, scoreTextAnchor.transform.position);
                        scoreText.transform.position = screenPos;
                    }
                    else
                    {
                        LogEx.LogWarning("FloatingUICanvas instance not found. Score text may not display correctly.");
                    }
                }
                else
                {
                    scoreText.transform.SetParent(transform, true);
                    scoreText.transform.position = scoreTextAnchor.position;
                }
            }
        }

        /// <summary>
        /// ingredientObject를 받아서 오브젝트를 해당 타일의 오브젝트에 반영하는 것
        /// </summary>
        /// <param name="ingredient">교체할 오브젝트</param>
        public void SetIngredient(IngredientObject ingredient)
        {
            //if(ingredient != null)
            //{
            //    ClearIngredient();
            //    return;
            //}

            CurrentIngredient = ingredient;
        }

        /// <summary>
        /// 현재 타일이 들고 있는 재료를 없애는 함수
        /// </summary>
        public void ClearIngredient()
        {
            CurrentIngredient = null;
        }

        public void OnClick()
        {

        }

        public void OnPress()
        {
        }

        public void OnCancel()
        {
        }

        public void OnHold()
        {
        }

        public void OnRelease()
        {
            
        }
        
        public void SetHighlighted(bool highlighted)
        {
            if (highlighted)
            {
                transform.DOKill();
                transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutQuad);
            }
            else
            {
                transform.DOKill();
                transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
            }
            
        }
        
    }
}