using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBehaviourScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var obj = hit.collider.gameObject;
            Debug.Log(obj.GetComponentInParent<ButtonBehaviourScript>().label);
        }
    }
}
