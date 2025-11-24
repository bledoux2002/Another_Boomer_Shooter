using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private MonoBehaviour target;
    private bool _pressed;
    
    // Start is called before the first frame update
    void Start()
    {
        _pressed = false;
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public IEnumerator Interact()
    {
        // Debug.Log("Button pressed");
        if (!_pressed)
        {
            _pressed = true;
            GetComponent<MeshRenderer>().material.color = Color.green;
            var interactable = target as IInteractable;
            StartCoroutine(interactable?.Interact());

            for (int i = 0; i < 50; i++) //i use 50 since it matches with default fixed update timing
            {
                transform.Translate(Vector3.forward * 0.1f / 50f);
                yield return new WaitForSeconds(0.01f);
            }

            for (int i = 0; i < 50; i++)
            {
                transform.Translate(Vector3.back * 0.1f / 50f);
                yield return new WaitForSeconds(0.01f);
            }

            _pressed = false;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
    
    
}
