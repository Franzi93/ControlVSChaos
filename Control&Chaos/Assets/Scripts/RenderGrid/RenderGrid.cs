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

        [SerializeField] private List<GameObject> preExistingCells;


        public int Width => width;
        public int Height => height;

        private readonly List<GameObject> cells = new List<GameObject>();

        private Vector2Int deleteCoord;

        private void Start()
        {
            // FillTestGrid();
        }

        private int GetCellIndexInListFromCoords(int x, int y)
        {
            return y * width + x;
        }

        private bool IsValidCoords(int x, int y)
        {
            return GetCellIndexInListFromCoords(x, y) < cells.Count;
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
            if (Application.isPlaying)
            {
                foreach (GameObject cell in cells)
                {
                    Destroy(cell);
                }

                cells.Clear();
            }
            else if (Application.isEditor)
            {
                for (int i = this.transform.childCount; i > 0; --i)
                    DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        public void FillNewGridInstant(int width, int height)
        {
            Clear();

            if (cellPrefabs.Count == 0)
            {
                Debug.LogError("No tile prefabs set!");
                return;
            }

            Random rnd = new Random();

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int randomIndex = rnd.Next(cellPrefabs.Count);
                    Vector2 position = new Vector2(x, y) * cellSize + cellSize / 2.0f;
                    GameObject cell = Instantiate(cellPrefabs[randomIndex],
                        transform.position + new Vector3(position.x, 0, position.y), Quaternion.identity, transform);
                    cells.Add(cell);
                }
            }

            this.width = width;
            this.height = height;
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
                    GameObject cell = Instantiate(cellPrefabs[randomIndex],
                        transform.position + new Vector3(position.x, 0, position.y), Quaternion.identity, transform);
                    cells.Add(cell);

                    yield return new WaitForSeconds(0.01f);
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

        public void Setup(GameGrid gameGrid)
        {
            FillNewGridInstant(width, height);
        }

        public void SetupWithNewSize(int width, int height)
        {
            FillNewGridInstant(width, height);
        }

        public void Setup()
        {
            FillNewGridInstant(width, height);
        }

        public void SetupFromExistingCells(int width, int height)
        {
            // Check if the list of existing cells is exactly the size to fill the whole grid
            int expectedSize = width * height;

            if (expectedSize == preExistingCells.Count)
            {
                foreach (GameObject preExistingCell in preExistingCells)
                {
                    cells.Add(preExistingCell);
                }
            }
            else
            {
                throw new Exception("RenderGrid with pre-existing cells is setup with wrong size!");
            }
        }
    }


    [CustomEditor(typeof(RenderGrid))]
    class RenderGridEditor : Editor
    {
        private RenderGrid renderGrid;

        private void OnEnable()
        {
            renderGrid = (RenderGrid) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Clear"))
            {
                renderGrid.Clear();
            }

            if (GUILayout.Button("FillGrid"))
            {
                renderGrid.Setup();
            }
        }
    }
}