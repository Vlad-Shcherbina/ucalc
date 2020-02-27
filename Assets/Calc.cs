using UnityEngine;

public class Calc
{
    long value = 0;

    public string display
    {
        get
        {
            return value.ToString();
        }
    }

    public void receive(char button)
    {
        /*if ('0' <= button && button <= '9')
        {
            value *= 10;
            value += button - '0';
        }*/
        switch (button)
        {
            case var c when ('0' <= c && c <= '9'):
                value *= 10;
                value += c - '0';
                break;
            default:
                Debug.Assert(false, button);
                break;
        }
    }

    public void receive(string buttons)
    {
        foreach (char button in buttons)
        {
            receive(button);
        }
    }
}
