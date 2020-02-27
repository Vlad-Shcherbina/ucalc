using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBehaviourScript : MonoBehaviour
{
    public GameObject display;

    ButtonBehaviourScript pressedButton = null;

    void Start()
    {
        Debug.Log(new Calc());
    }

    void Update()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        ButtonBehaviourScript hitButton = null;
        if (Physics.Raycast(ray, out hit))
        {
            hitButton = hit.collider.GetComponentInParent<ButtonBehaviourScript>();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Assert(pressedButton == null);
            if (hitButton)
            {
                pressedButton = hitButton;
                pressedButton.pressed = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (pressedButton)
            {
                if (hitButton == pressedButton)
                {
                    display.GetComponent<TextMesh>().text = pressedButton.label;
                }
                pressedButton.pressed = false;
                pressedButton = null;
            }
        }
    }
}
