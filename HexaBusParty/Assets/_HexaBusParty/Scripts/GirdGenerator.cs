using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GirdGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject hexagon;

    [Header("Settings")]
    [OnValueChanged(nameof(GenerateGrid))]
    [SerializeField] private int gridSize;

    [SerializeField] private List<Color> hexagonColors;

    private void GenerateGrid()
    {
        transform.Clear(); // Destruye todos los child que tenga el transform
        
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
               Vector3 spawnPost =  grid.CellToWorld(new Vector3Int(x, y, 0));
               if (spawnPost.magnitude > grid.CellToWorld(new Vector3Int(1,0,0)).magnitude * gridSize)
                continue;
               Instantiate(hexagon, spawnPost, Quaternion.identity, transform);
            }
        }
        
    }
}
