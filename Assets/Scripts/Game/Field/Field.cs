using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Attributes;
using Core;
using Machamy.Attributes;
using Machamy.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Field
{
    
    /// <summary>
    /// 게임 필드를 나타내는 클래스
    /// </summary>
    [RequireComponent(typeof(Grid))]
    public class Field : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private FieldConfigurationSO fieldConfiguration;
        [SerializeField, VisibleOnly(EditableIn.EditMode)] int width = 6;
        [SerializeField, VisibleOnly(EditableIn.EditMode)] int height = 6;
        [SerializeField, VisibleOnly(EditableIn.EditMode)] Tile _tilePrefab;
        // [SerializeField] private bool _initOnAwake = true;
        [Header("References")]
        [SerializeField] private Grid _grid;
        [SerializeField, VisibleOnly(EditableIn.EditMode)] private Transform tileParent;
        [SerializeField, VisibleOnly] private TileLine[] _tileContainer;
        
        
        
        public Grid Grid => _grid;
        public int Width => width;
        public int Height => height;
        


        private void Awake()
        {
            _grid = GetComponent<Grid>();
            GameManager.Instance.Field = this;
        }

        [ContextMenu("Init Field")]
        public void InitField()
        {
            InitField(width, height);
        }
        
        public void InitField(int width, int height)
        {
            LogEx.Log($"Size {width}x{height}");
            ClearTiles();
            this.width = width;
            this.height = height;
            _tileContainer = new TileLine[height];
            var tileParentTransform = tileParent != null ? tileParent : transform;
            for (int i = 0; i < height; i++)
            {
                _tileContainer[i] = new TileLine(width);
                for (int j = 0; j < width; j++)
                {

                    Tile tile = Instantiate(_tilePrefab, tileParentTransform);
                    tile.Initialize(this, new TileVector(i, j));
                    tile.name = $"Tile_({i},{j})";
                    tile.transform.position = Grid.GetCellCenterWorld(new Vector3Int(j,i, 0));
                    tile.transform.localScale = Vector3.one * fieldConfiguration.TileScale;
                    _tileContainer[i][j] = tile;
                }
            }

        }

        [ContextMenu("Clear Tiles")]
        public void ClearTiles()
        {
            if (_tileContainer == null)
                return;

            foreach (var row in _tileContainer)
            {
                foreach (var tile in row)
                {
                    if (tile != null)
                    {
                        #if UNITY_EDITOR
                        if (Application.isPlaying)
                        {
                            Destroy(tile.gameObject);
                        }
                        else
                        {
                            DestroyImmediate(tile.gameObject);
                        }
                        #else
                        Destroy(tile.gameObject);  
                        #endif  
                    }
                }
            }
        }
        
        
        [Serializable]
        internal class TileLine : IEnumerable<Tile>
        {
            [SerializeField] private Tile[] tiles;
            
            
            internal TileLine(int size = 3)
            {
                tiles = new Tile[size];
            }

            public Tile this[int index]
            {
                get
                {
                    if (index < 0 || index >= tiles.Length)
                        throw new IndexOutOfRangeException($"Index {index} is out of range for TileLine.");
                    return tiles[index];
                }
                set
                {
                    if (index < 0 || index >= tiles.Length)
                        throw new IndexOutOfRangeException($"Index {index} is out of range for TileLine.");
                    tiles[index] = value;
                }
            }

            public int Length => tiles.Length;
            public IEnumerator<Tile> GetEnumerator()
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    yield return tiles[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public static implicit operator Tile[](TileLine line)
            {
                return line.tiles;
            }
        }

    }
}