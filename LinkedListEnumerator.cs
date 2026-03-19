using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedListEnumerator<T> : IEnumerator<T>
{
    private LinkedListNode<T>? _eerste;
    private LinkedListNode<T>? _huidig;

    public LinkedListEnumerator(LinkedListNode<T>? eerste)
    {
        _eerste = eerste;
        _huidig = null;
    }

    public T Current => _huidig!.Waarde;

    object IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        if (_huidig == null)
        {
            _huidig = _eerste;
        }
        else
        {
            _huidig = _huidig.Volgende;
        }

        return _huidig != null;
    }

    public void Reset()
    {
        _huidig = null;
    }

    public void Dispose()
    {

    }
}