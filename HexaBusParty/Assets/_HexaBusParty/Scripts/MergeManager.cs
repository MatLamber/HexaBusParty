using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [Header("Elements")] private List<GridCell> updateCells = new List<GridCell>();

    private void OnEnable()
    {
        StackController.onStackedPlaced += StackPlacedCallback;
    }


    private void OnDisable()
    {
        StackController.onStackedPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridCell gridCell)
    {
        StartCoroutine(StackCoroutine(gridCell));
    }

    IEnumerator StackCoroutine(GridCell gridCell)
    {
        updateCells.Add(gridCell);
        while (updateCells.Count > 0)
        {
            yield return CheckForMergeCoroutine(updateCells[0]);
        }
    }

    IEnumerator CheckForMergeCoroutine(GridCell gridCell)
    {
        updateCells.Remove(gridCell);
        if (!gridCell.IsOccupied)
            yield break;
        List<GridCell> neighborGridCells = GetNeighboursGridCells(gridCell);
        if (neighborGridCells.Count <= 0) yield break;
        Color gridCellTopHexagonColor = gridCell.Stack.GetTopHexagonColor();
        List<GridCell> similarNeighborGridCells =
            GetSimilarNeighbourGridCells(gridCellTopHexagonColor, neighborGridCells);
        List<Hexagon> hexagonsToAdd =
            GetHexagonsToAdd(gridCellTopHexagonColor, neighborGridCells, similarNeighborGridCells);
        RemoveHexagonsFromStacks(similarNeighborGridCells, hexagonsToAdd);
        MoveHexagons(gridCell, hexagonsToAdd);
        yield return new WaitForSeconds(((hexagonsToAdd.Count + 1) * .12f) + .3f);
        yield return CheckForCompleteStacks(gridCell, gridCellTopHexagonColor);
    }

    IEnumerator CheckForCompleteStacks(GridCell gridCell, Color gridCellTopHexagonColor)
    {
        if (gridCell.Stack.Hexagons.Count < 10)
            yield break;
        List<Hexagon> similarHexaongs = new List<Hexagon>();
        for (int i = gridCell.Stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hexagon = gridCell.Stack.Hexagons[i];
            if (!AreColorsSimilar(gridCellTopHexagonColor, hexagon.Color, 0.1f))
                break;
            similarHexaongs.Add(hexagon);
        }
        if (similarHexaongs.Count < 10)
            yield break;
        int similarHexaongsCount = similarHexaongs.Count;
        float delay = 0;
        while (similarHexaongs.Count > 0)
        {
            similarHexaongs[0].SetParent(null);
            similarHexaongs[0].Vanish(delay);
            delay += .05f;
            //DestroyImmediate(similarHexaongs[0].gameObject);
            gridCell.Stack.Remove(similarHexaongs[0]);
            similarHexaongs.RemoveAt(0);
        }
        
        updateCells.Add(gridCell);
        yield return new WaitForSeconds((similarHexaongsCount + 1) * .1f + .3f);
    }

    private void MoveHexagons(GridCell gridCell, List<Hexagon> hexagonsToAdd)
    {
        float initalY = gridCell.Stack.Hexagons.Count * .2f;
        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];
            float targetY = initalY + (i * .2f);
            Vector3 targetLocalPosition = Vector3.up * targetY;
            gridCell.Stack.Add(hexagon);
            hexagon.MoveToLocal(targetLocalPosition, i);

        }
    }

    private void RemoveHexagonsFromStacks(List<GridCell> similarNeighborGridCells, List<Hexagon> hexagonsToAdd)
    {
        foreach (GridCell neighborGridCell in similarNeighborGridCells)
        {
            HexStack neihborCellHexStack = neighborGridCell.Stack;
            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (neihborCellHexStack.Contains(hexagon))
                    neihborCellHexStack.Remove(hexagon);
            }
        }
    }

    private List<Hexagon> GetHexagonsToAdd(Color gridCellTopHexagonColor, List<GridCell> neighborGridCells,
        List<GridCell> similarNeighborGridCells)
    {
        List<Hexagon> hexagonsToAdd = new List<Hexagon>();
        foreach (GridCell neighborGridCell in similarNeighborGridCells)
        {
            HexStack neihborCellHexStack = neighborGridCell.Stack;

            for (int i = neihborCellHexStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = neihborCellHexStack.Hexagons[i];
                if (hexagon.Unstackable)
                    break;
                if (!AreColorsSimilar(gridCellTopHexagonColor, hexagon.Color, 0.1f))
                    break;
                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }
        

        return hexagonsToAdd;
    }

    private List<GridCell> GetSimilarNeighbourGridCells(Color gridCellTopHexagonColor, List<GridCell> neighborGridCells)
    {
        List<GridCell> similarNeighborGridCells = new List<GridCell>();

        foreach (GridCell neighborGridCell in neighborGridCells)
        {
            Color neighborHexagonColor = neighborGridCell.Stack.GetTopHexagonColor();
            if (AreColorsSimilar(gridCellTopHexagonColor, neighborHexagonColor, 0.1f))
                similarNeighborGridCells.Add(neighborGridCell);
        }



        updateCells.AddRange(similarNeighborGridCells);

        return similarNeighborGridCells;
    }

    private List<GridCell> GetNeighboursGridCells(GridCell gridCell)
    {
        List<GridCell> neighborGridCells = new List<GridCell>();
        LayerMask gridLayer = 1 << gridCell.gameObject.layer;

        Collider[] neighborCells = Physics.OverlapSphere(gridCell.transform.position, 2, gridLayer);

        foreach (Collider gridCellCllider in neighborCells)
        {
            Debug.Log($"{gridCellCllider.name}");
            GridCell neighborCell = gridCellCllider.GetComponentInParent<GridCell>();
            if (!neighborCell.IsOccupied)
                continue;
            if (neighborCell == gridCell)
                continue;
            neighborGridCells.Add(neighborCell);
        }

        return neighborGridCells;
    }

    public bool AreColorsSimilar(Color color1, Color color2, float threshold)
    {
        float rDif = Math.Abs(color1.r - color2.r);
        float gDif = Math.Abs(color1.g - color2.g);
        float bDif = Math.Abs(color1.b - color2.b);
        float aDif = Math.Abs(color1.a - color2.a);
        return rDif <= threshold && gDif <= threshold && bDif <= threshold && aDif <= threshold;
    }
}