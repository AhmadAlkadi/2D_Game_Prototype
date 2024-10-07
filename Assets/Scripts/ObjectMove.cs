using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var renderer = GetComponent<Renderer>();
        renderer.material.SetColor("_Color", Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
