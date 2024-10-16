using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float doorOpenSpeed;
    public int doorOpenTime;
    private bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Operate()
    {
        if (!doorOpen)
        {
            Debug.Log("Door opening...");
            doorOpen = true;
            for (int i = 0; i < doorOpenSpeed; i++)
            {
                transform.Translate(Vector3.up * 3.5f / doorOpenSpeed);
                yield return new WaitForSeconds(1 / 2 * doorOpenSpeed);
            }

            yield return new WaitForSeconds(doorOpenTime);
            
            Debug.Log("Door closing...");
            for (int i = 0; i < doorOpenSpeed; i++)
            {
                transform.Translate(Vector3.down * 3.5f / doorOpenSpeed);
                yield return new WaitForSeconds(1 / 2 * doorOpenSpeed);
            }
            doorOpen = false;
        }
    }
}
