using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously rotate object
        transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
        
        // Object floats up and down

    }
}
