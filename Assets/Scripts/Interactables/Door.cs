using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // Animation Controls
    [SerializeField] private float openSpeed;
    [SerializeField] private bool stayOpen;
    [SerializeField] private float closeDelay;
    private bool _open;
    public bool Locked { get; private set; }
    public KeyType KeyColor { get; private set; }

    // GameObjects
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bot;

    void Start()
    {
        _open = false;
    }

    public int Interact()
    {
        if (Locked) return 1;   

        if (!_open && !Locked)
        {
            StartCoroutine(Open());
        }
        return 0;
    }

    public int Unlock(KeyType key)
    {
        if (key == KeyColor)
        {
            Locked = false;
            return 0;
        } else return 1;
    }

    private IEnumerator Open()
    {
        // Debug.Log("Door opening...");
        _open = true;
        for (int i = 0; i < 50; i++)
        {
            top.transform.Translate(Vector3.up * 2.49f / 50f);
            bot.transform.Translate(Vector3.down * 0.99f /  50f);
            yield return new WaitForSeconds(openSpeed / 50f);
        }

        if (!stayOpen)
        {
            yield return new WaitForSeconds(closeDelay);
            
            // Debug.Log("Door closing...");
            for (int i = 0; i < 50; i++)
            {
                top.transform.Translate(Vector3.down * 2.49f / 50f);
                bot.transform.Translate(Vector3.up * 0.99f / 50f);
                yield return new WaitForSeconds(openSpeed / 50f);
            }
            _open = false;
        }
    }
}
