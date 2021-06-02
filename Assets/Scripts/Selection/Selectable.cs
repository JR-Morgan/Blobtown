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

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class DisplayPropertyAttribute : Attribute
{
}