using System;
using System.Collections;
using System.Collections.Generic;

public class BSTEnumerator<T> : IEnumerator<T>
{
    private readonly BSTNode<T>? _root;
    private readonly Func<T, T, int> _compare;
    private BSTNode<T>? _current;
    private bool _started;

    public BSTEnumerator(BSTNode<T>? root, Func<T, T, int> compare)
    {
        _root = root;
        _compare = compare;
        _current = null;
        _started = false;
    }

    public T Current
    {
        get
        {
            if (_current == null)
                throw new InvalidOperationException();

            return _current.Value;
        }
    }

    object IEnumerator.Current => Current!;

    public bool MoveNext()
    {
        if (!_started)
        {
            _current = GetLeftMost(_root);
            _started = true;
            return _current != null;
        }

        if (_current == null)
            return false;

        _current = GetSuccessor(_root, _current.Value);
        return _current != null;
    }

    public void Reset()
    {
        _current = null;
        _started = false;
    }

    public void Dispose()
    {
    }

    private BSTNode<T>? GetLeftMost(BSTNode<T>? node)
    {
        if (node == null)
            return null;

        BSTNode<T> current = node;
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }

    private BSTNode<T>? GetSuccessor(BSTNode<T>? root, T value)
    {
        BSTNode<T>? successor = null;
        BSTNode<T>? current = root;

        while (current != null)
        {
            int result = _compare(value, current.Value);

            if (result < 0)
            {
                successor = current;
                current = current.Left;
            }
            else if (result > 0)
            {
                current = current.Right;
            }
            else
            {
                if (current.Right != null)
                    return GetLeftMost(current.Right);

                break;
            }
        }

        return successor;
    }
}