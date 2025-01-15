using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask hexagonLayer;
    [SerializeField] private LayerMask gridHexagonLayer;
    [SerializeField] private LayerMask groundLayer;
    private HexStack currentStack;
    private Vector3 currentHexStackInitialPos;
    [Header("Data")]
    private GridCell targetCell;

    [Header("Actions")] public static Action<GridCell> onStackedPlaced;


    private void Update()
    {
       ManageControl();
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManageMoouseDown();
        }
        else if (Input.GetMouseButton(0) && currentStack != null)
        {
            ManageMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && currentStack != null)
        {
            ManageMouseUp();
        }
    }

    private void ManageMoouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayer);
        if (hit.collider == null)
        {
            Debug.Log("No hexagon detected");
            return;
        }
        currentStack = hit.collider.GetComponentInParent<Hexagon>().HexStack;
        currentHexStackInitialPos = currentStack.transform.position;
        currentStack.FreeCurrentGridCell();
    }

    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayer);
        if (hit.collider == null)
        {
            DraggingAboveGround();
        }
        else
        {
            DraggingAboveCell(hit);
        }
    }

    private void DraggingAboveCell(RaycastHit hit)
    {
        GridCell gridCell = hit.collider.GetComponentInParent<GridCell>();
        if (gridCell.IsOccupied)
        {
            DraggingAboveGround();
        }
        else
        {
            DragginAboveFreeCell(gridCell, hit);
        }
    }

    private void DragginAboveFreeCell(GridCell gridCell, RaycastHit hit)
    {
        Vector3 currenTargerPostion = hit.point.With(y: 2);
        currentStack.transform.position =
            Vector3.MoveTowards(currentStack.transform.position, currenTargerPostion, Time.deltaTime * 30);
        targetCell = gridCell;
    }

    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayer);
        if (hit.collider == null)
        {
            Debug.Log("No ground detected");
            return;
        }

        Vector3 stackTargetPosition = hit.point.With(y: 1.8f);
        currentStack.transform.position =
            Vector3.MoveTowards(currentStack.transform.position, stackTargetPosition, Time.deltaTime * 30);
        targetCell = null;
    }

    private void ManageMouseUp()
    {
        if (targetCell == null)
        {
            currentStack.transform.position = currentHexStackInitialPos;
            currentStack = null;
            return;
        }
        
        currentStack.transform.position = targetCell.transform.position.With(y: .2f);
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Place();
        targetCell.AssignStack(currentStack);
        currentStack.OccupyCurrentGridCell(targetCell);
        onStackedPlaced?.Invoke(targetCell);
        targetCell = null;
        currentStack = null;
    }
    
    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
