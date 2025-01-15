using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [Header("Elements")] [SerializeField] private new Renderer renderer;
    [SerializeField] private new Collider collider;
    public HexStack HexStack { get; private set; }
    [Header("Settings")] 
    [SerializeField] private bool unstackable;
    
    
    public bool Unstackable => unstackable;
    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }


    public void Configure(HexStack hexStack)
    {
        HexStack = hexStack;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void MoveToLocal(Vector3 targetLocalPosition)
    {
        LeanTween.moveLocal(gameObject, targetLocalPosition, .2f).setEase(LeanTweenType.easeInOutSine)
            .setDelay(transform.GetSiblingIndex() * .1f);

        Vector3 direction = (targetLocalPosition - transform.localPosition).normalized.With(y: 0);
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        LeanTween.rotateAroundLocal(gameObject, rotationAxis, 180, .2f).setEase(LeanTweenType.easeInOutSine)
            .setDelay(transform.GetSiblingIndex() * .1f);
    }

    public void Vanish(float delay)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setDelay(delay).setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => Destroy(gameObject));
    }
}