using System;
using System.Collections;

public class BSTCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private BSTNode<T>? _root;
    private readonly Func<T, T, int> _compare;
    private int _count;

    public int Count => _count;
    public bool Dirty { get; set; }

    public BSTCollection(Func<T, T, int> compare)
    {
        _root = null;
        _compare = compare;
        _count = 0;
        Dirty = false;
    }

    public void Add(T item)
    {
        _root = AddRecursive(_root, item);
        _count++;
        Dirty = true;
    }

    private BSTNode<T> AddRecursive(BSTNode<T>? node, T item)
    {
        if (node == null)
            return new BSTNode<T>(item);

        int result = _compare(item, node.Value);

        if (result < 0)
        {
            node.Left = AddRecursive(node.Left, item);
        }
        else
        {
            node.Right = AddRecursive(node.Right, item);
        }

        return node;
    }

    public void Remove(T item)
    {
        bool removed;
        _root = RemoveRecursive(_root, item, out removed);

        if (removed)
        {
            _count--;
            Dirty = true;
        }
    }

    private BSTNode<T>? RemoveRecursive(BSTNode<T>? node, T item, out bool removed)
    {
        removed = false;

        if (node == null)
            return null;

        int result = _compare(item, node.Value);

        if (result < 0)
        {
            node.Left = RemoveRecursive(node.Left, item, out removed);
            return node;
        }

        if (result > 0)
        {
            node.Right = RemoveRecursive(node.Right, item, out removed);
            return node;
        }

        removed = true;

        if (node.Left == null && node.Right == null)
            return null;

        if (node.Left == null)
            return node.Right;

        if (node.Right == null)
            return node.Left;

        BSTNode<T> successor = GetMinNode(node.Right);
        node.Value = successor.Value;

        bool dummy;
        node.Right = RemoveRecursive(node.Right, successor.Value, out dummy);
        return node;
    }

    private BSTNode<T> GetMinNode(BSTNode<T> node)
    {
        BSTNode<T> current = node;

        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        return FindByRecursive(_root, key, comparer);
    }

    private T FindByRecursive<K>(BSTNode<T>? node, K key, Func<T, K, bool> comparer)
    {
        if (node == null)
            return default!;

        if (comparer(node.Value, key))
            return node.Value;

        T leftResult = FindByRecursive(node.Left, key, comparer);
        if (leftResult != null && !leftResult.Equals(default(T)))
            return leftResult;

        return FindByRecursive(node.Right, key, comparer);
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new BSTCollection<T>(_compare);
        FilterRecursive(_root, predicate, result);
        return result;
    }

    private void FilterRecursive(BSTNode<T>? node, Func<T, bool> predicate, BSTCollection<T> result)
    {
        if (node == null)
            return;

        FilterRecursive(node.Left, predicate, result);

        if (predicate(node.Value))
        {
            result.Add(node.Value);
        }

        FilterRecursive(node.Right, predicate, result);
    }

    public void Sort(Comparison<T> comparison)
    {
        T[] items = new T[_count];
        int index = 0;

        FillArrayInOrder(_root, items, ref index);

        for (int i = 1; i < items.Length; i++)
        {
            T current = items[i];
            int j = i - 1;

            while (j >= 0 && comparison(items[j], current) > 0)
            {
                items[j + 1] = items[j];
                j--;
            }

            items[j + 1] = current;
        }

        _root = null;
        _count = 0;

        for (int i = 0; i < items.Length; i++)
        {
            Add(items[i]);
        }

        Dirty = true;
    }

    private void FillArrayInOrder(BSTNode<T>? node, T[] items, ref int index)
    {
        if (node == null)
            return;

        FillArrayInOrder(node.Left, items, ref index);
        items[index++] = node.Value;
        FillArrayInOrder(node.Right, items, ref index);
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R result = default!;
        ReduceRecursive(_root, accumulator, ref result);
        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        ReduceRecursive(_root, accumulator, ref result);
        return result;
    }

    private void ReduceRecursive<R>(BSTNode<T>? node, Func<R, T, R> accumulator, ref R result)
    {
        if (node == null)
            return;

        ReduceRecursive(node.Left, accumulator, ref result);
        result = accumulator(result, node.Value);
        ReduceRecursive(node.Right, accumulator, ref result);
    }

    public IMyIterator<T> GetIterator()
    {
        return new BSTIterator<T>(_root, _compare);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new BSTEnumerator<T>(_root, _compare);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}