using System;
using Common.Singleton;
using Game.Field;
using UnityEngine;

namespace Interaction
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        protected override void AfterAwake()
        {
            
        }

        [SerializeField] private float longPressThreshold = 0.75f;
        [SerializeField] private float clickThreshold = 0.2f;
        
        private bool isPressing = false;
        private float pressTime = -100f;
        private Vector2 pointerPosition = Vector2.zero;
        private Tile pressedTile = null;
        
        public event Action<Tile> OnTilePressed;
        public event Action<Tile> OnTileReleased;
        public event Action<Tile> OnTileClicked;
        public event Action<Tile> OnTileLongPressed;
        public event Action<Tile> OnTileLongReleased;
        public event Action<Tile, Tile> OnTileDragging;
        public event Action<Tile, Tile> OnTileDragReleased;

        private void LateUpdate()
        {
            

            if (isPressing && pressTime >= 0)
            {
                float heldDuration = Time.time - pressTime;
                // 오래 누르고, 인터랙터블이 동일하면 홀드 이벤트 발생
                if (heldDuration >= longPressThreshold)
                {
                    Physics2D.queriesStartInColliders = false;
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pointerPosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        var interactable = hit.collider.GetComponent<Tile>();
                        if (interactable != null)
                        {
                            if (pressedTile != null && pressedTile == interactable)
                            {
                                OnTileLongPressed?.Invoke(interactable);
                            }
                        }
                    }
                }
            }
        }

        public void OnPress(Vector2 position)
        {
            isPressing = true;
            pressTime = Time.time;
            pointerPosition = position;
            
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
            if (hit.collider != null)
            {
                var interactable = hit.collider.GetComponent<Tile>();
                if (interactable != null)
                {
                    pressedTile = interactable;
                    OnTilePressed?.Invoke(interactable);
                }
            }
        }

        public void OnPointMoved(Vector2 position)
        {
            if (pressTime < 0) return;

            this.pointerPosition = position;

            // 인터랙터블이 다르면 드래그 이벤트 발생
            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
            if (hit.collider != null)
            {
                var interactable = hit.collider.GetComponent<Tile>();
                if (interactable != null)
                {
                    if (pressedTile != null && pressedTile != interactable)
                    {
                        OnTileDragging?.Invoke(pressedTile, interactable);
                    }
                }
            }
        }

        public void OnRelease(Vector2 position)
        {
            isPressing = false;
            if (pressTime < 0) return;
            float heldDuration = Time.time - pressTime;
            pressTime = -100f;

            Physics2D.queriesStartInColliders = false;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
            if (hit.collider != null)
            {
                var interactable = hit.collider.GetComponent<Tile>();
                if (interactable != null)
                {
                    OnTileReleased?.Invoke(interactable);
                    if (heldDuration <= clickThreshold)
                    {
                        OnTileClicked?.Invoke(interactable);
                    }
                    if (heldDuration >= longPressThreshold)
                    {
                        OnTileLongReleased?.Invoke(interactable);
                    }
                    if (pressedTile != null && pressedTile != interactable)
                    {
                        OnTileDragReleased?.Invoke(pressedTile, interactable);
                    }
                }
            }
            
        }
    }
}