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
        
        
        public TileVector GetWrappedCoordinate(TileVector tile)
        {
            var wrappedI = (tile.i % height + height) % height;
            var wrappedJ = (tile.j % width + width) % width;
            return new TileVector(wrappedI, wrappedJ);
        }
        public TileVector GetWrappedCoordinate(int i, int j)
        {
            var wrappedI = (i % height + height) % height;
            var wrappedJ = (j % width + width) % width;
            return new TileVector(wrappedI, wrappedJ);
        }
        public Vector3 GetTileCenterWorld(TileVector tile){
            return _grid.GetCellCenterWorld(new Vector3Int(tile.j, tile.i, 0));
        }
        
        public Vector3 GetCenterPosition()
        {
            var LeftBottom = _grid.GetCellCenterWorld(new Vector3Int(0, 0, 0));
            var RightTop = _grid.GetCellCenterWorld(new Vector3Int(width - 1, height - 1, 0));
            return (LeftBottom + RightTop) / 2;
        }
        
        public Tile WorldToTile(Vector3 worldPosition)
        {
            var localPosition = transform.InverseTransformPoint(worldPosition);
            var cellPosition = _grid.WorldToCell(localPosition);
            if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height)
                return null;
            return _tileContainer[cellPosition.y][cellPosition.x];
        }
        
        public TileVector WorldToCoordinate(Vector3 worldPosition)
        {
            var localPosition = transform.InverseTransformPoint(worldPosition);
            var cellPosition = _grid.WorldToCell(localPosition);
            return new TileVector(cellPosition.y, cellPosition.x);
        }
        
        public Tile GetTile(TileVector tile)
        {
            return GetTile(tile.i, tile.j);
        }
        public Tile GetTile(int i, int j)
        {
            if (i < 0 || i >= height || j < 0 || j >= width)
            {
                LogEx.LogError($"GetTile: Index out of range. ({i}, {j}) is not a valid tile coordinate.");
                return null;
            }
            return _tileContainer[i][j];
        }
        public Tile GetTileByDirection(Tile tile, Direction direction, bool wrapAround = false, int distance = 1)
        {
            return GetTileByDirectionWrap(tile, direction, out bool _, distance);
        }
        
        public Tile GetTileByDirectionWrap(Tile tile, Direction direction, out bool wrapped, int distance = 1)
        {
            wrapped = false;
            var coordinate = tile.Coordinate;
            var nextCoordinate = coordinate + direction.ToTileVector() * distance;
            if (nextCoordinate.i < 0 || nextCoordinate.i >= height ||
                nextCoordinate.j < 0 || nextCoordinate.j >= width)
            {
                wrapped = true;
                while (nextCoordinate.i < 0)
                    nextCoordinate.i += height;
                while (nextCoordinate.i >= height)
                    nextCoordinate.i -= height;

                while (nextCoordinate.j < 0)
                    nextCoordinate.j += width;
                while (nextCoordinate.j >= width)
                    nextCoordinate.j -= width;
            }
            return GetTile(nextCoordinate);
        }
        
        public Tile GetTileByDelta(Tile tile, TileVector delta, bool wrapAround = false)
        {
            var coordinate = tile.Coordinate;
            var nextCoordinate = coordinate + delta;
            if (wrapAround)
            {
                while (nextCoordinate.i < 0)
                    nextCoordinate.i += height;
                while (nextCoordinate.i >= height)
                    nextCoordinate.i -= height;

                while (nextCoordinate.j < 0)
                    nextCoordinate.j += width;
                while (nextCoordinate.j >= width)
                    nextCoordinate.j -= width;
            }
            return GetTile(nextCoordinate);
        }
        
        
        public Vector3 GetTilePosition(TileVector tile)
        {
            return GetTilePosition(tile.i, tile.j);
        }
        public Vector3 GetTilePosition(int i, int j)
        {
            if (i < 0 || i >= height || j < 0 || j >= width)
            {
                LogEx.LogError($"GetTilePosition: Index out of range. ({i}, {j}) is not a valid tile coordinate.");
                return Vector3.zero;
            }
            return _tileContainer[i][j].transform.position;
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