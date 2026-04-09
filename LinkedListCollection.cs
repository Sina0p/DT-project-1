using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedListCollection<T> : IMyCollection<T>, IEnumerable<T>
{
    private LinkedListNode<T>? _eerste;
    private int _aantal;

    public int Count => _aantal;
    public bool Dirty { get; set; }

    public LinkedListCollection()
    {
        _eerste = null;
        _aantal = 0;
        Dirty = false;
    }

    public void Add(T item)
    {
        var nieuwKnooppunt = new LinkedListNode<T>(item);

        if (_eerste == null)
        {
            _eerste = nieuwKnooppunt;
        }
        else
        {
            var huidig = _eerste;

            while (huidig.Volgende != null)
            {
                huidig = huidig.Volgende;
            }

            huidig.Volgende = nieuwKnooppunt;
        }

        _aantal++;
        Dirty = true;
    }

    public void Remove(T item)
    {
        if (_eerste == null) return;

        if (Equals(_eerste.Waarde, item))
        {
            _eerste = _eerste.Volgende;
            _aantal--;
            Dirty = true;
            return;
        }

        var huidig = _eerste;

        while (huidig.Volgende != null)
        {
            if (Equals(huidig.Volgende.Waarde, item))
            {
                huidig.Volgende = huidig.Volgende.Volgende;
                _aantal--;
                Dirty = true;
                return;
            }

            huidig = huidig.Volgende;
        }
    }

    public T FindBy<K>(K sleutel, Func<T, K, bool> vergelijker)
    {
        var huidig = _eerste;

        while (huidig != null)
        {
            if (vergelijker(huidig.Waarde, sleutel))
                return huidig.Waarde;

            huidig = huidig.Volgende;
        }

        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> voorwaarde)
    {
        var resultaat = new LinkedListCollection<T>();
        var huidig = _eerste;

        while (huidig != null)
        {
            if (voorwaarde(huidig.Waarde))
            {
                resultaat.Add(huidig.Waarde);
            }

            huidig = huidig.Volgende;
        }

        return resultaat;
    }

    public void Sort(Comparison<T> vergelijking)
    {
        if (_eerste == null) return;

        bool gewisseld;

        do
        {
            gewisseld = false;
            var huidig = _eerste;

            while (huidig.Volgende != null)
            {
                if (vergelijking(huidig.Waarde, huidig.Volgende.Waarde) > 0)
                {
                    T tijdelijk = huidig.Waarde;
                    huidig.Waarde = huidig.Volgende.Waarde;
                    huidig.Volgende.Waarde = tijdelijk;

                    gewisseld = true;
                }

                huidig = huidig.Volgende;
            }

        } while (gewisseld);

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R resultaat = default!;
        var huidig = _eerste;

        while (huidig != null)
        {
            resultaat = accumulator(resultaat, huidig.Waarde);
            huidig = huidig.Volgende;
        }

        return resultaat;
    }

    public R Reduce<R>(R beginwaarde, Func<R, T, R> accumulator)
    {
        R resultaat = beginwaarde;
        var huidig = _eerste;

        while (huidig != null)
        {
            resultaat = accumulator(resultaat, huidig.Waarde);
            huidig = huidig.Volgende;
        }

        return resultaat;
    }

    public IMyIterator<T> GetIterator()
    {
        return new LinkedListIterator<T>(_eerste);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new LinkedListEnumerator<T>(_eerste);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}