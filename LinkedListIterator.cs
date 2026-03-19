using System;

public class LinkedListIterator<T> : IMyIterator<T>
{
    private LinkedListNode<T>? _start;
    private LinkedListNode<T>? _huidig;

    public LinkedListIterator(LinkedListNode<T>? eerste)
    {
        _start = eerste;
        _huidig = eerste;
    }

    public bool HasNext()
    {
        return _huidig != null;
    }

    public T Next()
    {
        if (_huidig == null)
            throw new InvalidOperationException("Geen elementen meer.");

        var waarde = _huidig.Waarde;
        _huidig = _huidig.Volgende;

        return waarde;
    }

    public void Reset()
    {
        _huidig = _start;
    }
}