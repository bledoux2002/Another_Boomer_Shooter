using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // Animation Controls
    [SerializeField] private float openSpeed;
    [SerializeField] private float closeDelay;
    private bool _open = false;
    public bool _locked { get; private set; }

    // GameObjects
    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bot;

    public int Interact(List<KeyTypes>)
    {
        if (_locked)
        {
            return 1;
        }
        if (!_open && !_locked)
        {
            StartCoroutine(Open());
        }

        return 0;
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
