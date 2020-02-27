using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBehaviourScript : MonoBehaviour
{
    public GameObject display;
    public float buttonTolerance;

    ButtonBehaviourScript pressedButton = null;
    Calc calc = new Calc();

    void Start()
    {
        display.GetComponent<TextMesh>().text = calc.display;
    }

    void Update()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Assert(pressedButton == null);
            ButtonBehaviourScript hitButton = null;
            if (Physics.Raycast(ray, out var hit))
            {
                hitButton = hit.collider.GetComponentInParent<ButtonBehaviourScript>();
            }
            if (hitButton)
            {
                pressedButton = hitButton;
                pressedButton.pressed = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Generally if the mouse is moved away on release,
            // we don't want to register a hit.
            // But we want to register a hit when the button is pressed at the very edge,
            // even if the ray doesn't hit depressed button.
            // Hence spherecast with tolerance.
            ButtonBehaviourScript hitButton = null;
            if (Physics.SphereCast(ray, buttonTolerance, out var hit))
            {
                hitButton = hit.collider.GetComponentInParent<ButtonBehaviourScript>();
            }
            if (pressedButton)
            {
                if (hitButton == pressedButton)
                {
                    Debug.Assert(pressedButton.label.Length == 1);
                    calc.receive(pressedButton.label[0]);
                    display.GetComponent<TextMesh>().text = calc.display;
                }
                pressedButton.pressed = false;
                pressedButton = null;
            }
        }
    }
}
