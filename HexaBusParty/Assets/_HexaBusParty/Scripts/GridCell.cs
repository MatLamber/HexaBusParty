using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Hexagon hexagon;

    [Header("Data")]
    [OnValueChanged(nameof(GenerateInitialHexagons))]
    [SerializeField] private Color[] colors;
    public HexStack Stack { get; private set; }
    
    public bool IsOccupied { get => Stack != null; private set {} }


    private void Start()
    {
        Stack = transform.childCount > 1 ? transform.GetChild(1).GetComponent<HexStack>() : null;
        if (Stack != null)
            Stack.Initialize(this);
    }

    public void AssignStack(HexStack stack)
    {
        this.Stack = stack;
    }

    private void GenerateInitialHexagons()
    {
        while (transform.childCount > 1)
        {
            Transform t = transform.GetChild(1);
            DestroyImmediate(t.gameObject);
        }
        Stack = new GameObject("Stack").AddComponent<HexStack>();
        Stack.transform.SetParent(transform);
        Stack.transform.localPosition = Vector3.up * .2f;
        for (int i = 0; i < colors.Length; i++)
        {
            Vector3 spawnPositions  =  Stack.transform.TransformPoint(Vector3.up * i * .2f);
            Hexagon hexagonInstance = Instantiate(hexagon, spawnPositions, Quaternion.identity);
            hexagonInstance.Color = colors[i];
            hexagonInstance.Configure(Stack);
            Stack.Add(hexagonInstance);
        }
        
    }

    public void ResetStack()
    {
        if (Stack != null)
            Stack = null;
    }
}
