using System.Collections;

public class ArrayCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private T[] _items;
    public int Count { get; private set; }
    public bool Dirty { get; set; }

    public ArrayCollection(int capacity = 4)
    {
        if (capacity < 1)
            capacity = 4;

        _items = new T[capacity];
        Count = 0;
        Dirty = false;
    }

    public void Add(T item)
    {
        if (Count == _items.Length)
        {
            Resize();
        }

        _items[Count] = item;
        Count++;
        Dirty = true;
    }

    public void Remove(T item)
    {
        int index = -1;

        for (int i = 0; i < Count; i++)
        {
            if (Equals(_items[i], item))
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            return;

        for (int i = index; i < Count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }

        _items[Count - 1] = default!;
        Count--;
        Dirty = true;
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < Count; i++)
        {
            if (comparer(_items[i], key))
            {
                return _items[i];
            }
        }

        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        ArrayCollection<T> result = new ArrayCollection<T>();

        for (int i = 0; i < Count; i++)
        {
            if (predicate(_items[i]))
            {
                result.Add(_items[i]);
            }
        }

        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        for (int i = 1; i < Count; i++)
        {
            T current = _items[i];
            int j = i - 1;

            while (j >= 0 && comparison(_items[j], current) > 0)
            {
                _items[j + 1] = _items[j];
                j--;
            }

            _items[j + 1] = current;
        }

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R result = default!;

        for (int i = 0; i < Count; i++)
        {
            result = accumulator(result, _items[i]);
        }

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        for (int i = 0; i < Count; i++)
        {
            result = accumulator(result, _items[i]);
        }

        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new ArrayIterator<T>(_items, Count);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new ArrayEnumerator<T>(_items, Count);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Resize()
    {
        int newCapacity = _items.Length * 2;
        T[] newItems = new T[newCapacity];

        for (int i = 0; i < Count; i++)
        {
            newItems[i] = _items[i];
        }

        _items = newItems;
    }
}