using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Vector3Spring
{
    public float Frequency {
        get {
            return Mathf.Sqrt(_stiffness) / 2.0f / Mathf.PI;
        }
        set {
            _stiffness = Mathf.Pow(value * 2.0f * Mathf.PI, 2.0f);
        }
    }
    public float Drag = 0.5f;
    [SerializeField]
    private float _stiffness = 1.0f;
    
    private Vector3 _value;
    private Vector3 _velocity;
    private Vector3 _equilibrium;

    public Vector3 Value {
        get {
            float stiffness = Mathf.Pow(2 * Mathf.PI * Frequency, 2.0f);
            _velocity = _velocity * (1 - Drag * Time.deltaTime) + (_equilibrium - _value) * stiffness * Time.deltaTime;
            _value += _velocity * Time.deltaTime;
            return _value;
        }
        set {
            _equilibrium = value;
        }
    }

    public Vector3Spring(Vector3 equilibrium, float frequency, float drag) {
        Frequency = frequency;
        Drag = drag;
        _equilibrium = equilibrium;
        _value = equilibrium;
        _velocity = Vector3.zero;
    }

    public Vector3 GetEquilibrium() {
        return _equilibrium;
    }

    public void SetValue(Vector3 value) {
        _value = value;
    }

    public void SetValue(Vector3 value, Vector3 velocity) {
        _value = value;
        _velocity = velocity;
    }
}
