using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallHex : MonoBehaviour
{
    Hexagon hex;
    // Start is called before the first frame update
    void Start()
    {
        hex = gameObject.GetComponentInParent<Hexagon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        hex.myMouseDown();
    }


}
