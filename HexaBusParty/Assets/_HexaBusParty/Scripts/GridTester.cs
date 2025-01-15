using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GridTester : MonoBehaviour
{
    [Header(" Elements ")] 
    [SerializeField] private Grid grid;

    [Header(" Settings ")] [SerializeField]
    [OnValueChanged(nameof(UpdateGridPos))]
    private Vector3Int gridPos;

    private void UpdateGridPos() => transform.position = grid.CellToWorld(gridPos);
    
}
