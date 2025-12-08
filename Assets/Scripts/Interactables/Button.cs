using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private IInteractable target;
    private bool _pressed;
    private MeshRenderer _mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        _pressed = false;
        _mesh = GetComponent<MeshRenderer>();
        _mesh.material.color = Color.red;
    }

    public int Interact()
    {
        // Debug.Log("Button pressed");
        if (!_pressed)
        {
            _pressed = true;
            int result = target.Interact();
            if (result != 0)
            {
                Debug.Log($"Issue with interactable {target}, error code {result}");
            }
            StartCoroutine(Press());
        }

        return 0;
    }

    private IEnumerator Press()
    {        
            _mesh.material.color = Color.green;
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
            _mesh.material.color = Color.red;
            
            _pressed = false;
    } 
    
    
}
