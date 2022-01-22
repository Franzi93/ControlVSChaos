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

        private void Awake()
        {
            FillTestGrid();
        }

        public void Clear()
        {
            foreach (GameObject cell in cells)
            {
                Destroy(cell);
            }
            cells.Clear();
        }

        public void FillNewGrid(int width, int height, bool fromEditor = false)
        {
            if (!fromEditor)
            {
                Clear();
            }

            if (cellPrefabs.Count == 0)
            {
                Debug.LogError("No tile prefabs set!");
                return;
            }
            
            Random rnd = new Random();
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    int randomIndex = rnd.Next(cellPrefabs.Count);
                    Vector2 position = new Vector2(x, y) * cellSize + cellSize / 2.0f;
                    GameObject cell =  Instantiate(cellPrefabs[randomIndex], transform.position + new Vector3(position.x, 0, position.y), Quaternion.identity, transform);
                    cells.Add(cell);
                }   
            }

            this.width = width;
            this.height = height;
        }

        [ContextMenu("FillTestGrid")]
        public void FillTestGrid()
        {
            FillNewGrid(10, 10, true);
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