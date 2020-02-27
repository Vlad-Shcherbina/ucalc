using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviourScript : MonoBehaviour
{
    public string label;
    public Vector3 pressOffset;
    public bool pressed = false;

    Vector3 initialPos;

    void Start()
    {
        GetComponentInChildren<TextMesh>().text = label;
        initialPos = GetComponent<Transform>().position;
    }

    private void Update()
    {
        GetComponent<Transform>().position =
            pressed ? initialPos + pressOffset : initialPos;
    }
}
