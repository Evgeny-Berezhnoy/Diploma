using System;

public class OpenQueue<T>
{
    #region Constants

    private const int DEFAULT_CAPACITY = 8;

    #endregion

    #region Fields

    private T[] _elements;
    private int _lastElementIndex;
    private int _lastElementIndexOperational;
    private int _index = -1;

    private T _element;
    private T _defaultValue;
    private ValueComparer<T> _valueComparer;

    #endregion

    #region Properties

    public int Count
    {
        get { return _lastElementIndex + 1; }
    }

    public T this[int index]
    {
        get
        {
            if(index < 0 || index > _lastElementIndex)
            {
                return _defaultValue;
            };

            return _elements[index];
        }
    }

    #endregion

    #region Constructors

    public OpenQueue(int capacity = DEFAULT_CAPACITY)
    {
        _elements           = new T[capacity];
        _lastElementIndex   = -1;
        _defaultValue       = default;
        _valueComparer      = new ValueComparer<T>();
    }

    #endregion

    #region Methods

    public T Find(Predicate<T> criteriaMatch)
    {
        for (_index = 0; _index < Count; _index++)
        {
            _element = _elements[_index];

            if (criteriaMatch(_element))
            {
                return _element;
            };
        };

        return _defaultValue;
    }

    public void Enqueue(T item)
    {
        if(_elements.Length < Count + 1)
        {
            T[] elements = new T[_elements.Length * 2];

            for (_index = 0; _index < _elements.Length; _index++)
            {
                elements[_index + 1] = _elements[_index];
            };

            _elements = elements;
        }
        else
        {
            for(_index = Count; _index > 0; _index--)
            {
                _elements[_index] = _elements[_index - 1];
            };
        };

        _elements[0] = item;

        _lastElementIndex++;
    }

    public T Dequeue()
    {
        if(_lastElementIndex == -1)
        {
            return _defaultValue;
        };

        _element = _elements[_lastElementIndex];

        Constrict(_lastElementIndex);

        return _element;
    }

    public T Dequeue(T item)
    {
        for(_index = 0; _index < Count; _index++)
        {
            if (_elements[_index].Equals(item))
            {
                _element = _elements[_index];

                Constrict(_index);

                return _element;
            };
        };

        return _defaultValue;
    }

    public T Reenqueue()
    {
        _element = Dequeue();

        if (_valueComparer.AreEqual(_element, _defaultValue))
        {
            Enqueue(_element);
        };

        return _element;
    }

    public T Reenqueue(T item)
    {
        _element = Dequeue(item);

        if (_valueComparer.AreEqual(_element, _defaultValue))
        {
            Enqueue(_element);
        };

        return _element;
    }

    private void Constrict(int indexFrom)
    {
        _elements[indexFrom] = _defaultValue;

        _lastElementIndexOperational = -1;

        for (_index = 0; _index < Count; _index++)
        {
            if (!_valueComparer.AreEqual(_elements[_index], _defaultValue))
            {
                _lastElementIndexOperational++;

                if (_lastElementIndexOperational != _index)
                {
                    _elements[_lastElementIndexOperational] = _elements[_index];

                    _elements[_index] = _defaultValue;
                };
            };
        };

        _lastElementIndex = _lastElementIndexOperational;
    }

    #endregion
}