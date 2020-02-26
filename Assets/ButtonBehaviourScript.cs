using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviourScript : MonoBehaviour
{
    public string label;
    void Start()
    {
        var t = this.GetComponentInChildren<TextMesh>();
        t.text = label;
    }
}
