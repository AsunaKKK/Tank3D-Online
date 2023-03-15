using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChangColor : MonoBehaviour
{
    public MeshRenderer d;
    // Start is called before the first frame update
    void Start()
    {
        d.material.color = Random.ColorHSV();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
