using System;
using System.Collections;

public class BSTCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private BSTNode<T>? _root;
    private readonly Func<T, T, int> _treeCompare;
    private int _count;
    private ArrayCollection<T>? _sortedView;

    public int Count => _count;
    public bool Dirty { get; set; }

    public BSTCollection(Func<T, T, int> compare)
    {
        _root = null;
        _treeCompare = compare;
        _count = 0;
        Dirty = false;
        _sortedView = null;
    }

    public void Add(T item)
    {
        _root = AddRecursive(_root, item);
        _count++;
        Dirty = true;
        _sortedView = null;
    }

    private BSTNode<T> AddRecursive(BSTNode<T>? node, T item)
    {
        if (node == null)
            return new BSTNode<T>(item);

        int result = _treeCompare(item, node.Value);

        if (result < 0)
            node.Left = AddRecursive(node.Left, item);
        else
            node.Right = AddRecursive(node.Right, item);

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
            _sortedView = null;
        }
    }

    private BSTNode<T>? RemoveRecursive(BSTNode<T>? node, T item, out bool removed)
    {
        removed = false;

        if (node == null)
            return null;

        int result = _treeCompare(item, node.Value);

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

        bool ignored;
        node.Right = RemoveRecursive(node.Right, successor.Value, out ignored);
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
        if (!Equals(leftResult, default(T)))
            return leftResult;

        return FindByRecursive(node.Right, key, comparer);
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        BSTCollection<T> result = new BSTCollection<T>(_treeCompare);
        FilterRecursive(_root, predicate, result);
        return result;
    }

    private void FilterRecursive(BSTNode<T>? node, Func<T, bool> predicate, BSTCollection<T> result)
    {
        if (node == null)
            return;

        FilterRecursive(node.Left, predicate, result);

        if (predicate(node.Value))
            result.Add(node.Value);

        FilterRecursive(node.Right, predicate, result);
    }

    public void Sort(Comparison<T> comparison)
    {
        ArrayCollection<T> items = new ArrayCollection<T>(_count > 0 ? _count : 4);
        FillCollectionInOrder(_root, items);
        items.Sort(comparison);
        _sortedView = items;
        Dirty = true;
    }

    private void FillCollectionInOrder(BSTNode<T>? node, ArrayCollection<T> items)
    {
        if (node == null)
            return;

        FillCollectionInOrder(node.Left, items);
        items.Add(node.Value);
        FillCollectionInOrder(node.Right, items);
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
        if (_sortedView != null)
            return _sortedView.GetIterator();

        return new BSTIterator<T>(_root, _treeCompare);
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (_sortedView != null)
            return _sortedView.GetEnumerator();

        return new BSTEnumerator<T>(_root, _treeCompare);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
