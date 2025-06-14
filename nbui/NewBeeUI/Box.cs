using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI;

public struct Box<T>
{
    private bool Unboxed = false;

    public T? Value { get; private set; }
    public Func<T?>? ValueFactory { get; private set; }

    public bool IsEmpty => Value == null && ValueFactory == null;

    public Box(T? value)
    {
        Value = value;
    }

    public Box(Func<T?> factory)
    {
        ValueFactory = factory;
    }

    public static implicit operator Box<T>(T? value)
    {
        return new Box<T>(value);
    }

    public static implicit operator Box<T>(Func<T?> value)
    {
        return new Box<T>(value);
    }

    public T? Unbox()
    {
        return Unbox(this);
    }

    public static T? Unbox(Box<T> box)
    {
        if(box.Unboxed) return box.Value;
        box.Unboxed = true;
        if (box.IsEmpty) return default;
        else if (box.Value != null) return box.Value;
        else if (box.ValueFactory != null)
        {
            box.Value = box.ValueFactory();
        }
        return default;
    }
}