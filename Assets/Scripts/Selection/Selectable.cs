using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Selectable")]
public class Selectable : MonoBehaviour
{
    [DisplayProperty]
    public string Name { get => gameObject.name; set => gameObject.name = value; }


    
}




public struct PropertyBinding
{
    public object Value {
        get => getter();
        set => setter(Value);
    }

    private readonly Action<object> setter;
    private readonly Func<object> getter;

    public PropertyBinding(Action<object> setter, Func<object> getter)
    {
        this.setter = setter;
        this.getter = getter;
    }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class DisplayPropertyAttribute : Attribute
{ }