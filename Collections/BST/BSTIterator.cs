using System;

public class BSTIterator<T> : IMyIterator<T>
{
    private readonly BSTNode<T>? _root;
    private readonly Func<T, T, int> _compare;
    private BSTNode<T>? _current;

    public BSTIterator(BSTNode<T>? root, Func<T, T, int> compare)
    {
        _root = root;
        _compare = compare;
        _current = GetLeftMost(_root);
    }

    public bool HasNext()
    {
        return _current != null;
    }

    public T Next()
    {
        if (_current == null)
            throw new InvalidOperationException("No more elements.");

        T value = _current.Value;
        _current = GetSuccessor(_root, _current.Value);
        return value;
    }

    public void Reset()
    {
        _current = GetLeftMost(_root);
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