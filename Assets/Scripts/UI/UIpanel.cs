using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[AddComponentMenu("Simulation/UI/UI Panel")]
public class UIPanel : MonoBehaviour
{

    [SerializeField]
    private GameObject titleTextPrefab;
    [SerializeField]
    private GameObject bodyTextPrefab;

    [SerializeField]
    private GameObject selectedPropertyParent;

    [SerializeField]
    private List<GameObject> objectsToHide;

    private List<GameObject> elements;

    private void Start()
    {
        elements = new List<GameObject>();
        InputManager.Instance.OnSelectableChange.AddListener(s =>
        {
            SetOpen(true);
            GenerateProperties(s);
        });
        SetOpen(false);
    }

    public void TogglePanel() => SetOpen(!this.gameObject.activeSelf);
    public void SetOpen(bool isOpen)
    {
        foreach (GameObject go in objectsToHide) go.SetActive(isOpen);
        this.gameObject.SetActive(isOpen);
    }

    private void GenerateProperties(Selectable s)
    {
        foreach (GameObject g in elements) Destroy(g);
        elements.Clear();

        Component[] components = s.GetComponents<Component>();

        foreach (Component c in components)
        {
            var properties = c.GetType().GetProperties().Where(p => p.IsDefined(typeof(DisplayPropertyAttribute)));
            foreach (PropertyInfo property in properties)
            {
                if(property.Name == "Name")
                {
                    var e = CreateNewTitleElement(property, c);
                    elements.Add(e);
                    e.transform.SetAsFirstSibling();
                }
                else
                {
                    elements.Add(CreateNewBodyElement(property, c));
                }
                
            }
        }
    }

    #region Generic UI propertys
    private GameObject CreateNewBodyElement(PropertyInfo property, object datasource)
    {
        GameObject go = Instantiate(bodyTextPrefab, selectedPropertyParent.transform);
        return SetupElement(go, property, datasource);
    }

    private GameObject CreateNewTitleElement(PropertyInfo property, object datasource)
    {
        GameObject go = Instantiate(titleTextPrefab, selectedPropertyParent.transform);
        return SetupElement(go, property, datasource);
    }


    private GameObject SetupElement(GameObject elementGO, PropertyInfo property, object dataSource)
    {
        
        if(elementGO.TryGetComponentInChildren(out TMP_InputField element))
        {
            element.readOnly = !property.CanWrite;

            if(elementGO.TryGetComponentInChildren(out TMP_Text label))
            {
                label.text = FormatLabelString(property.Name);
            }


            object viewModel = dataSource; //Box

            _ = (property.GetValue(viewModel)) switch
            {
                bool v => AddCallbackString(element, v, bool.TryParse, true),
                int v => AddCallbackString(element, v, int.TryParse),
                string v => AddCallbackString(element, v, ToSelf),
                float v => AddCallbackString(element, v, float.TryParse),
                double v => AddCallbackString(element, v, double.TryParse),
                TileType v => AddCallback(element, v, Enum.TryParse, ToString, true),
                ResourceType v => AddCallback(element, v, Enum.TryParse, ToString, true),
                BuildingType v => AddCallback(element, v, Enum.TryParse, ToString, true),
                _ => null,
            };
        }
        else
        {
            Debug.LogError($"{typeof(UIPanel)} could not find {typeof(TMP_InputField)} {typeof(Component)} in {nameof(bodyTextPrefab)}");
        }

        return elementGO;

        E AddCallbackString<E, T>(E element, T value, Convert<string, T> toV, bool readOnly = false) where E : TMP_InputField => AddCallback(element, value, toV, ToString, readOnly);

        E AddCallback<E, T>(E element, T value, Convert<string, T> toV, Convert<T, string> toTarget, bool readOnly = false) where E : TMP_InputField
        {
            element.readOnly = readOnly && element.readOnly;
            element.onValueChanged.AddListener(e =>
            {
#pragma warning disable CS0642 // Possible mistaken empty statement
                if (toV(e, out T value)) ;
                //else if (toV(e.previousValue, out value)) ;
                else value = default;
#pragma warning restore CS0642

                property.SetValue(dataSource, value);
                SetValue(element, value, toTarget);

            });

            SetValue(element, value, toTarget);

            return element;

            static void SetValue(E element, T value, Convert<T, string> toTarget)
            {
                if (toTarget(value, out string newValue))
                {
                    element.text = newValue;
                }
                else
                {
                    element.text = default;
                }
            }
        }


        static bool ToString<T>(T input, out string result)
        {
            result = input.ToString();
            return true;
        }

        static bool ToSelf<T>(T input, out T result)
        {
            result = input;
            return true;
        }

    }

    private delegate bool Convert<A, B>(A input, out B result);
    private static string FormatLabelString(string label)
    {
        TextInfo t = CultureInfo.CurrentCulture.TextInfo;
        return t.ToTitleCase(Regex.Replace(label, "([A-Z])", " $1").Trim());
    }

    #endregion
}
