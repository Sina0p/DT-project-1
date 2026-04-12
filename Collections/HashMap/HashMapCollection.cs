using System;
using System.Collections;
using System.Collections.Generic;

public class HashMapCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private ArrayCollection<T>[] _buckets;
    private readonly Func<T, int> _hashFunc;
    private int _count;

    public int Count => _count;
    public bool Dirty { get; set; }

    public HashMapCollection(Func<T, int> hashFunc, int capacity = 16)
    {
        if (capacity < 1)
            capacity = 16;

        _hashFunc = hashFunc;
        _buckets = new ArrayCollection<T>[capacity];

        for (int i = 0; i < capacity; i++)
        {
            _buckets[i] = new ArrayCollection<T>();
        }

        _count = 0;
        Dirty = false;
    }

    private int GetIndex(T item)
    {
        int hash = _hashFunc(item);

        if (hash == int.MinValue)
            hash = 0;

        hash = Math.Abs(hash);
        return hash % _buckets.Length;
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
        int before = _buckets[index].Count;

        _buckets[index].Remove(item);

        if (_buckets[index].Count < before)
        {
            _count--;
            Dirty = true;
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < _buckets.Length; i++)
        {
            T found = _buckets[i].FindBy(key, comparer);

            if (!Equals(found, default(T)))
                return found;
        }

        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        HashMapCollection<T> result = new HashMapCollection<T>(_hashFunc, _buckets.Length);

        for (int i = 0; i < _buckets.Length; i++)
        {
            foreach (T item in _buckets[i])
            {
                if (predicate(item))
                    result.Add(item);
            }
        }

        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        if (_count <= 1)
            return;

        T[] items = new T[_count];
        int index = 0;

        for (int i = 0; i < _buckets.Length; i++)
        {
            foreach (T item in _buckets[i])
            {
                items[index++] = item;
            }
        }

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

        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i] = new ArrayCollection<T>();
        }

        _count = 0;

        for (int i = 0; i < items.Length; i++)
        {
            Add(items[i]);
        }

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R result = default!;

        for (int i = 0; i < _buckets.Length; i++)
        {
            foreach (T item in _buckets[i])
            {
                result = accumulator(result, item);
            }
        }

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        for (int i = 0; i < _buckets.Length; i++)
        {
            foreach (T item in _buckets[i])
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