using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Duality
{
    public class RenderGrid : MonoBehaviour
    {
        [SerializeField] private List<GameObject> cellPrefabs;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Vector2 cellSize = new Vector2(2, 2);
        
        
        private readonly List<GameObject> cells = new List<GameObject>();

        public Vector2Int deleteCoord;

        private void Start()
        {
            FillTestGrid();
        }

        private int GetCellIndexInListFromCoords(int x, int y)
        {
            return  y * width + x;
        }

        private bool IsValidCoords(int x, int y)
        {
            return GetCellIndexInListFromCoords(x, y) <= cells.Count;
        }

        private GameObject GetCellFromFoords(int x, int y)
        {
            if (IsValidCoords(x, y))
            {
                return cells[GetCellIndexInListFromCoords(x, y)];
            }
            
            return null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                int x = deleteCoord.x;
                int y = deleteCoord.y;
                int calculatedIndex = GetCellIndexInListFromCoords(x, y);
                if (cells.Count > calculatedIndex)
                {
                    GameObject cell = cells[calculatedIndex];
                    Destroy(cell);
                }
                else
                {
                    Debug.Log($"Could not delete cell at pos {x}, {y}!");
                }
            }
        }

        public void Clear()
        {
            foreach (GameObject cell in cells)
            {
                Destroy(cell);
            }
            cells.Clear();
        }

        public IEnumerator FillNewGrid(int width, int height, bool fromEditor = false)
        {
            if (!fromEditor)
            {
                Clear();
            }

            if (cellPrefabs.Count == 0)
            {
                Debug.LogError("No tile prefabs set!");
                yield break;
            }
            
            Random rnd = new Random();
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int randomIndex = rnd.Next(cellPrefabs.Count);
                    Vector2 position = new Vector2(x, y) * cellSize + cellSize / 2.0f;
                    GameObject cell =  Instantiate(cellPrefabs[randomIndex], transform.position + new Vector3(position.x, 0, position.y), Quaternion.identity, transform);
                    cells.Add(cell);

                    yield return new WaitForSeconds(0.05f);
                }   
            }

            this.width = width;
            this.height = height;
        }

        [ContextMenu("FillTestGrid")]
        public void FillTestGrid()
        {
            StartCoroutine(FillNewGrid(width, height, true));
        }

        public Vector3 GetRenderPositionFromCellPosition(int cellX, int cellY)
        {
            if (IsValidCoords(cellX, cellY))
            {
                GameObject cell = GetCellFromFoords(cellX, cellY);
                if (cell != null)
                {
                    RenderCell renderCell = cell.GetComponent<RenderCell>();
                    if (renderCell)
                    {
                        return renderCell.GetCharacterTransform().position;
                    }
                }
            }
            
            Debug.Log("GetRenderPositionFromCellPosition: Invalid Cell Position!");
            return Vector3.zero;
        }
    }

    
    [CustomEditor(typeof(RenderGrid))]
    class RenderGridEditor : Editor
    {
        private SerializedProperty grid;

        private void OnEnable()
        {
            grid = serializedObject.FindProperty("grid");
            
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Clear"))
            {
                
            }
            
            if (GUILayout.Button("FillNewGrid 10, 10"))
            {
                
                // ((grid.GetType()) grid).FillNewGrid(10, 10);
            }
        }
    }
}