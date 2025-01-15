using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackSpawner : MonoBehaviour
{
    [Header(" Elements ")] 
    
    [SerializeField] private Transform stackPositionsParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexStack hexagonStackPrefab;

    [Header(" Settings ")] 
    [NaughtyAttributes.MinMaxSlider(2, 8)]
    [SerializeField] private Vector2Int minMaxHexCount;
    [SerializeField] private Color[] colors;
    private int stackCounter;


    private void OnEnable()
    {
        StackController.onStackedPlaced += StackPlacedCallback;
    }



    private void OnDisable()
    {
        StackController.onStackedPlaced -= StackPlacedCallback;
    }

    private void Start()
    {
        GenerateStacks();
    }

    private void GenerateStacks()
    {
        for (int i = 0; i < stackPositionsParent.childCount; i++)
            GenerateStack(stackPositionsParent.GetChild(i));
            
    }
    private void GenerateStack(Transform parent)
    {
        HexStack hexStack = Instantiate(hexagonStackPrefab, parent.position, Quaternion.identity, parent);
        hexStack.name = $" Stack {parent.GetSiblingIndex()}";
        
        int amount = Random.Range(minMaxHexCount.x, minMaxHexCount.y);
        int firstColorHexagonCount = Random.Range(0, amount);
        Color[] colorArray = GetRandomColors();
        for (int i = 0; i < amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * (i * .2f);
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);
            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity, hexStack.transform);
            hexagonInstance.Color = i < firstColorHexagonCount ? colorArray[0] : colorArray[1];
            hexagonInstance.Configure(hexStack);
            hexStack.Add(hexagonInstance);
        }
    }


    private Color[] GetRandomColors()
    {
        List<Color> colorsList = new List<Color>();
        colorsList.AddRange(colors);

        if (colorsList.Count < 0)
        {
            Debug.LogError("No colors in the list");
            return null;
        }
        
        Color firstColor = colorsList.OrderBy(x => Random.value).First();
        colorsList.Remove(firstColor);

        if (colorsList.Count <= 0)
        {
            Debug.LogError("Only one color was found");
            return null;
        }
            
        Color secondColor= colorsList.OrderBy(x => Random.value).First();
        colorsList.Remove(firstColor);
        
        return new[] {firstColor, secondColor};
    }
    
    private void StackPlacedCallback(GridCell obj)
    {
        stackCounter++;
        if (stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }
    }
}
