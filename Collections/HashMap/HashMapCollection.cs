using System;
using System.Collections;
using System.Collections.Generic;

public class HashMapCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private List<T>[] _buckets;
    private readonly Func<T, int> _hashFunc;
    private int _count;

    public int Count => _count;
    public bool Dirty { get; set; }

    public HashMapCollection(Func<T, int> hashFunc, int capacity = 16)
    {
        _hashFunc = hashFunc;
        _buckets = new List<T>[capacity];

        for (int i = 0; i < capacity; i++)
        {
            _buckets[i] = new List<T>();
        }

        _count = 0;
        Dirty = false;
    }

    private int GetIndex(T item)
    {
        int hash = _hashFunc(item);
        return Math.Abs(hash) % _buckets.Length;
    }

    public void Add(T item)
    {
        int index = GetIndex(item);
        _buckets[index].Add(item);
        _count++;
        Dirty = true;
    }

    public void Remove(T item)
    {
        int index = GetIndex(item);

        if (_buckets[index].Remove(item))
        {
            _count--;
            Dirty = true;
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        foreach (var bucket in _buckets)
        {
            foreach (var item in bucket)
            {
                if (comparer(item, key))
                    return item;
            }
        }

        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new HashMapCollection<T>(_hashFunc);

        foreach (var bucket in _buckets)
        {
            foreach (var item in bucket)
            {
                if (predicate(item))
                    result.Add(item);
            }
        }

        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        List<T> allItems = new List<T>();

        foreach (var bucket in _buckets)
        {
            allItems.AddRange(bucket);
        }

        allItems.Sort(comparison);

        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i].Clear();
        }

        _count = 0;

        foreach (var item in allItems)
        {
            Add(item);
        }

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R result = default!;

        foreach (var bucket in _buckets)
        {
            foreach (var item in bucket)
            {
                result = accumulator(result, item);
            }
        }

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        foreach (var bucket in _buckets)
        {
            foreach (var item in bucket)
            {
                result = accumulator(result, item);
            }
        }

        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new HashMapIterator<T>(_buckets);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new HashMapEnumerator<T>(_buckets);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}