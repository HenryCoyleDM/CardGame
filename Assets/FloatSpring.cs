using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class FloatSpring
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

    private float _stiffness = 1.0f;
    
    private float _value;
    private float _velocity;
    private float _equilibrium;

    public float Value {
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

    public FloatSpring(float equilibrium, float frequency, float drag) {
        Frequency = frequency;
        Drag = drag;
        _equilibrium = equilibrium;
        _value = equilibrium;
        _velocity = 0.0f;
    }

    public float GetEquilibrium() {
        return _equilibrium;
    }

    public void SetValue(float value) {
        _value = value;
    }

    public void SetValue(float value, float velocity) {
        _value = value;
        _velocity = velocity;
    }
}
