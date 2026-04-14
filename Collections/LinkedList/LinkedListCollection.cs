using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedListCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private LinkedListNode<T>? _first;
    private int _count;

    public int Count => _count;
    public bool Dirty { get; set; }

    public LinkedListCollection()
    {
        _first = null;
        _count = 0;
        Dirty = false;
    }

    public void Add(T item)
    {
        var newNode = new LinkedListNode<T>(item);

        if (_first == null)
        {
            _first = newNode;
        }
        else
        {
            var current = _first;

            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = newNode;
        }

        _count++;
        Dirty = true;
    }

    public void Remove(T item)
    {
        if (_first == null) return;

        if (Equals(_first.Value, item))
        {
            _first = _first.Next;
            _count--;
            Dirty = true;
            return;
        }

        var current = _first;

        while (current.Next != null)
        {
            if (Equals(current.Next.Value, item))
            {
                current.Next = current.Next.Next;
                _count--;
                Dirty = true;
                return;
            }

            current = current.Next;
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        var current = _first;

        while (current != null)
        {
            if (comparer(current.Value, key))
                return current.Value;

            current = current.Next;
        }

        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> condition)
    {
        var result = new LinkedListCollection<T>();
        var current = _first;

        while (current != null)
        {
            if (condition(current.Value))
            {
                result.Add(current.Value);
            }

            current = current.Next;
        }

        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        if (_first == null) return;

        bool swapped;

        do
        {
            swapped = false;
            var current = _first;

            while (current.Next != null)
            {
                if (comparison(current.Value, current.Next.Value) > 0)
                {
                    T temp = current.Value;
                    current.Value = current.Next.Value;
                    current.Next.Value = temp;

                    swapped = true;
                }

                current = current.Next;
            }

        } while (swapped);

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R result = default!;
        var current = _first;

        while (current != null)
        {
            result = accumulator(result, current.Value);
            current = current.Next;
        }

        return result;
    }

    public R Reduce<R>(R initialValue, Func<R, T, R> accumulator)
    {
        R result = initialValue;
        var current = _first;

        while (current != null)
        {
            result = accumulator(result, current.Value);
            current = current.Next;
        }

        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new LinkedListIterator<T>(_first);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new LinkedListEnumerator<T>(_first);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}