using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorOpenTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Open()
    {
        Debug.Log("Door opening...");
        yield return new WaitForSeconds(doorOpenTime);
        Debug.Log("Door closing...");
    }
}
