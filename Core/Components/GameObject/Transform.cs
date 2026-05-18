using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class Transform
{
    public Transform()
    {
        Position = Vector3.Zero;
        Rotation = Vector3.Zero;
        Scale = Vector3.One;
        Origin = Vector3.Zero;
    }

    private Transform _parent;
    public Vector3 Origin { get; set; }

    private readonly List<Transform> _childrens = [];

    private Matrix _absolute, _invertAbsolute, _local;

    private Vector3 _localRotation, _absoluteRotation;

    private Vector3 _localScale, _absoluteScale, _localPosition, _absolutePosition;

    private bool _needsAbsoluteUpdate = true, _needsLocalUpdate = true;

    public GameObject Gameobject {get; set;}
    public Transform Parent
    {
        get => _parent;
        set
        {
            if (_parent != value)
            {
                if (_parent != null)
                    {
                        _parent._childrens.Remove(this);
                        _parent.Gameobject.RemoveChild();
                    }

                _parent = value;

                if (_parent != null)
                    {
                        _parent._childrens.Add(this);
                        _parent.Gameobject.AddChild();
                    }

                SetNeedsAbsoluteUpdate();
            }
        }
    }
    
    public IReadOnlyList<Transform> Childrens => _childrens;

    public Vector3 AbsoluteRotation => UpdateAbsoluteAndGet(ref _absoluteRotation);

    public Vector3 AbsoluteScale => UpdateAbsoluteAndGet(ref _absoluteScale);

    public Vector3 AbsolutePosition => UpdateAbsoluteAndGet(ref _absolutePosition);

    public Vector3 Rotation
    {
        get => _localRotation;
        set
        {
            if (_localRotation != value)
            {
                _localRotation = value;
                SetNeedsLocalUpdate();
            }
        }
    }

    public Vector3 Position
    {
        get
        {
            return _localPosition;
        }
        set
        {
            if (_localPosition != value)
            {
                _localPosition = value;
                SetNeedsLocalUpdate();
            }
        }
    }

    
    public Vector3 Scale
    {
        get => _localScale;
        set
        {
            if (_localScale != value)
            {
                _localScale = value;
                SetNeedsLocalUpdate();
            }
        }
    }
    
    public Matrix Local => UpdateLocalAndGet(ref _absolute);

    public Matrix Absolute => UpdateAbsoluteAndGet(ref _absolute);

    public Matrix InvertAbsolute => UpdateAbsoluteAndGet(ref _invertAbsolute);

    public void ToLocalPosition(ref Vector3 absolute, out Vector3 local) => 
        Vector3.Transform(ref absolute, ref _invertAbsolute, out local);

    public void ToAbsolutePosition(ref Vector3 local, out Vector3 absolute) => 
        Vector3.Transform(ref local, ref _absolute, out absolute);

    public Vector3 ToLocalPosition(Vector3 absolute)
    {
        ToLocalPosition(ref absolute, out Vector3 result);
        return result;
    }

    public Vector3 ToAbsolutePosition(Vector3 local)
    {
        ToAbsolutePosition(ref local, out Vector3 result);
        return result;
    }

    private void SetNeedsLocalUpdate()
    {
        _needsLocalUpdate = true;
        SetNeedsAbsoluteUpdate();
    }

    private void SetNeedsAbsoluteUpdate()
    {
        _needsAbsoluteUpdate = true;

        foreach (Transform child in _childrens)
            child.SetNeedsAbsoluteUpdate();
    }

    private void UpdateLocal()
    {
        Matrix scale = Matrix.CreateScale(Scale.X, Scale.Y, Scale.Z);
        Matrix rotation = Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z); 
        Matrix translation = Matrix.CreateTranslation(Position.X, Position.Y, Position.Z);
        _local = scale * rotation * translation;

        _needsLocalUpdate = false;
    }

    private void UpdateAbsolute()
    {
        if (Parent == null)
        {
            _absolute = _local;
            _absoluteScale = _localScale;
            _absoluteRotation = _localRotation;
            _absolutePosition = _localPosition;
        }
        else
        {
            Matrix parentAbsolute = Parent.Absolute;
            Matrix.Multiply(ref _local, ref parentAbsolute, out _absolute);
            _absoluteScale = Parent.AbsoluteScale * Scale;
            _absoluteRotation = Parent.AbsoluteRotation + Rotation;
            _absolutePosition = Vector3.Zero;
            ToAbsolutePosition(ref _absolutePosition, out _absolutePosition);
        }

        Matrix.Invert(ref _absolute, out _invertAbsolute);

        _needsAbsoluteUpdate = false;
    }

    private T UpdateLocalAndGet<T>(ref T field)
    {
        if (_needsLocalUpdate)
            UpdateLocal();

        return field;
    }

    private T UpdateAbsoluteAndGet<T>(ref T field)
    {
        if (_needsLocalUpdate)
            UpdateLocal();

        if (_needsAbsoluteUpdate)
            UpdateAbsolute();

        return field;
    }

    public override string ToString() => $"Position: X:{Position.X} Y:{Position.Y} Z:{Position.Z} AbsolutePosition: X:{AbsolutePosition.X} Y:{AbsolutePosition.Y} Z:{AbsolutePosition.Z} Rotation: X:{Rotation.X} Y:{Rotation.Y} Z:{Rotation.Z} Scale: X:{Scale.X} Y:{Scale.Y} Z:{Scale.Z}";
}
