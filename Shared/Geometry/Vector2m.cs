using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GraphicsEngine;
using Microsoft.SolverFoundation.Common;
using Shared.Additional;

public class Vector2m
{
    public Rational X { get; private set; }
    public Rational Y { get; private set; }

    internal  DynamicProperties DynamicProperties = new DynamicProperties();

    public Vector2m(Rational x, Rational y)
    {
        X = x;
        Y = y;
    }

    public Vector2m(Vector2m v)
    {
        X = v.X;
        Y = v.Y;
    }

    public override bool Equals(object obj)
    {
        var other = obj as Vector2m;

        if (other == null)
        {
            return false;
        }

        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return this.X.GetHashCode() ^ this.Y.GetHashCode();
    }

    public static Vector2m operator -(Vector2m a, Vector2m b)
    {
        return a.Minus(b);
    }

    public Vector2m Minus(Vector2m a)
    {
        return new Vector2m(this.X - a.X, this.Y - a.Y);
    }

    public static Vector2m operator +(Vector2m a, Vector2m b)
    {
        return a.Plus(b);
    }

    public Vector2m Plus(Vector2m a)
    {
        return new Vector2m(this.X + a.X, this.Y + a.Y);
    }

    public Vector2m Times(Rational r)
    {
        return new Vector2m(X * r, Y * r);
    }

    public Rational Dot(Vector2m a)
    {
        return X * a.X + Y * a.Y;
    }

    public override string ToString()
    {
        return "Vector:" + " " + X.ToDouble() + " " + Y.ToDouble() + " ";
    }
}