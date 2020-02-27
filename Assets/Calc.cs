using UnityEngine;

enum Op
{
    Plus,
    Minus,
}

enum State
{
    Input,
    AfterEq,
    AfterOp,
    AfterOpInput,
    AfterOpEq,
    AfterOpEqInput,
    Overflow,
}

public class Calc
{
    const int MAX_LENGTH = 8;
    const long MAX_VALUE = 99999999;
    const long MIN_VALUE = -9999999;

    State state = State.Input;

    long? value = 0;
    Op? op = null;
    long? memory = null;

    void CheckInvariants()
    {
        switch (state)
        {
            case State.Input:
            case State.AfterEq:
                Debug.Assert(value.HasValue);
                Debug.Assert(!op.HasValue);
                Debug.Assert(!memory.HasValue);
                break;
            case State.AfterOp:
                Debug.Assert(value.HasValue);
                Debug.Assert(op.HasValue);
                Debug.Assert(!memory.HasValue);
                break;
            case State.AfterOpInput:
            case State.AfterOpEq:
            case State.AfterOpEqInput:
                Debug.Assert(value.HasValue);
                Debug.Assert(op.HasValue);
                Debug.Assert(memory.HasValue);
                break;
            case State.Overflow:
                Debug.Assert(!value.HasValue);
                Debug.Assert(!op.HasValue);
                Debug.Assert(!memory.HasValue);
                break;
            default:
                Debug.Assert(false, state);
                break;
        }
        Debug.Assert(Display.Length <= MAX_LENGTH);
    }

    public string Display
    {
        get
        {
            if (state == State.Overflow)
            {
                return "overflow";
            }
            return value.ToString();
        }
    }

    void HandleOverflow()
    {
        Debug.Assert(state != State.Overflow);
        if (value < MIN_VALUE || value > MAX_VALUE)
        {
            state = State.Overflow;
            value = null;
            op = null;
            memory = null;
        }
    }

    void ReceiveDigit(int d)
    {
        switch (state)
        {
            case State.Input:
            case State.AfterOpInput:
            case State.AfterOpEqInput:
                Debug.Assert(value >= 0);
                if (value * 10 <= MAX_VALUE)
                {
                    value *= 10;
                    value += d;
                }
                break;
            case State.AfterOp:
                state = State.AfterOpInput;
                memory = value;
                value = d;
                break;
            case State.AfterOpEq:
                state = State.AfterOpEqInput;
                value = d;
                break;
            case State.AfterEq:
                state = State.Input;
                value = d;
                break;
            case State.Overflow:
                break;
            default:
                Debug.Assert(false, state);
                break;
        }
    }

    void ReceiveBackspace()
    {
        switch (state)
        {
            case State.Input:
            case State.AfterOpInput:
            case State.AfterOpEqInput:
                Debug.Assert(value >= 0);
                value /= 10;
                break;
        }
    }

    private long ApplyOp(long a, Op op, long b)
    {
        switch (op)
        {
            case Op.Plus:
                return a + b;
            case Op.Minus:
                return a - b;
            default:
                Debug.Assert(false, op);
                return -1;
        }
    }

    void ReceiveOp(Op op)
    {
        switch (state)
        {
            case State.Input:
            case State.AfterEq:
            case State.AfterOp:
                state = State.AfterOp;
                this.op = op;
                break;
            case State.AfterOpInput:
                state = State.AfterOp;
                value = ApplyOp(memory.Value, this.op.Value, value.Value);
                memory = null;
                this.op = op;
                HandleOverflow();
                break;
            case State.AfterOpEqInput:
                state = State.AfterOp;
                this.op = op;
                memory = null;
                break;
            case State.AfterOpEq:
                state = State.AfterOp;
                this.op = op;
                this.memory = null;
                break;
            case State.Overflow:
                break;
            default:
                Debug.Assert(false, state);
                break;
        }
    }

    void ReceiveEq()
    {
        switch (state)
        {
            case State.Input:
            case State.AfterEq:
                state = State.AfterEq;
                break;
            case State.AfterOp:
                state = State.AfterOpEq;
                memory = value;
                value = ApplyOp(value.Value, op.Value, value.Value);
                HandleOverflow();
                break;
            case State.AfterOpInput:
                state = State.AfterOpEq;
                var result = ApplyOp(memory.Value, op.Value, value.Value);
                memory = value;
                value = result;
                HandleOverflow();
                break;
            case State.AfterOpEq:
            case State.AfterOpEqInput:
                state = State.AfterOpEq;
                value = ApplyOp(value.Value, op.Value, memory.Value);
                HandleOverflow();
                break;
            case State.Overflow:
                break;
            default:
                Debug.Assert(false, state);
                break;
        }
    }

    public void Receive(char button)
    {
        CheckInvariants();
        switch (button)
        {
            case var c when ('0' <= c && c <= '9'):
                ReceiveDigit(c - '0');
                break;
            case '<':
                ReceiveBackspace();
                break;
            case 'C':
                state = State.Input;
                value = 0;
                op = null;
                memory = null;
                break;
            case '+':
                ReceiveOp(Op.Plus);
                break;
            case '-':
                ReceiveOp(Op.Minus);
                break;
            case '=':
                ReceiveEq();
                break;
            default:
                Debug.Assert(false, button);
                break;
        }
        CheckInvariants();
    }

    public void Receive(string buttons)
    {
        foreach (char button in buttons)
        {
            Receive(button);
        }
    }
}
