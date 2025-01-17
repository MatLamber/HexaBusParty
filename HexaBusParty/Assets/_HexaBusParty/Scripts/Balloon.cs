using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveY(gameObject, transform.position.y + .5f, Random.Range(0.5f,1f)).setLoopPingPong(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
