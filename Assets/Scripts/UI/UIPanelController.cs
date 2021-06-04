using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[AddComponentMenu("Simulation/UI/UI Panel Controller")]
public class UIPanelController : MonoBehaviour
{

    [SerializeField]
    private GameObject titleTextPrefab;
    [SerializeField]
    private GameObject bodyTextPrefab;
    [SerializeField]
    private GameObject dropDownPrefab;


    [SerializeField]
    private GameObject selectedPropertyParent;

    [SerializeField]
    private List<Behaviour> objectsToHide;

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
        foreach (Behaviour go in objectsToHide) go.enabled = isOpen;
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
                else if(property.PropertyType.IsEnum)
                {
                    elements.Add(CreateDropdownElement(property, c));
                }
                else
                {
                    elements.Add(CreateNewBodyElement(property, c));
                }
                
            }
        }
    }

    #region Generic UI propertys
    private GameObject CreateNewBodyElement(PropertyInfo property, object dataSource)
    {
        GameObject go = Instantiate(bodyTextPrefab, selectedPropertyParent.transform);
        return SetupElement(go, property, dataSource);
    }

    private GameObject CreateNewTitleElement(PropertyInfo property, object dataSource)
    {
        GameObject go = Instantiate(titleTextPrefab, selectedPropertyParent.transform);
        return SetupElement(go, property, dataSource);
    }

    private GameObject CreateDropdownElement(PropertyInfo property, object dataSource)
    {
        if (property.CanWrite && property.GetSetMethod(true).IsPublic)
        {
            GameObject go = Instantiate(dropDownPrefab, selectedPropertyParent.transform);
            if (go.TryGetComponentInChildren(out TMP_Text label))
            {
                label.text = FormatLabelString(property.Name);
            }

            if (go.TryGetComponentInChildren(out TMP_Dropdown dropDown))
            {

                AddCallback(dropDown, (Enum)property.GetValue(dataSource));
                //_ = (property.GetValue(dataSource)) switch
                //{
                //    TileType v => AddCallback(dropDown, v),
                //    ResourceType v => AddCallback(dropDown, v),
                //    BuildingType v => AddCallback(dropDown, v),
                //    AgentType v => AddCallback(dropDown, v),
                //    _ => throw new NotImplementedException(""),
                //};
            }
            return go;
        }
        else
        {
            return CreateNewBodyElement(property, dataSource);
        }

        Enum AddCallback<T>(TMP_Dropdown dropDown, T value) where T : Enum
        {
            int i = 0;
            int s = 0;
            var options = new List<TMP_Dropdown.OptionData>();
            foreach (string option in Enum.GetNames(value.GetType()))
            {
                options.Add(new TMP_Dropdown.OptionData(option));

                if(option == value.ToString())
                {
                    s = i;
                }
                i++;
            }
            dropDown.AddOptions(options);
            dropDown.value = s;

            dropDown.onValueChanged.AddListener(e => {
                object v = Enum.Parse(value.GetType(), options[e].text);
                property.SetValue(dataSource, v);
                });

            return value;
        }
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


            _ = (property.GetValue(dataSource)) switch
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
            Debug.LogError($"{typeof(UIPanelController)} could not find {typeof(TMP_InputField)} {typeof(Component)} in {nameof(bodyTextPrefab)}");
        }

        return elementGO;

        E AddCallbackString<E, T>(E element, T value, Convert<string, T> toV, bool readOnly = false) where E : TMP_InputField => AddCallback(element, value, toV, ToString, readOnly);

        E AddCallback<E, T>(E element, T value, Convert<string, T> toV, Convert<T, string> toTarget, bool readOnly = false) where E : TMP_InputField
        {
            element.readOnly = readOnly || element.readOnly;
            element.onValueChanged.AddListener(e =>
            {
#pragma warning disable CS0642 // Possible mistaken empty statement
                if (toV(e, out T value)) ;
                //else if (toV(e.previousValue, out value)) ;
                else value = default;
#pragma warning restore CS0642

                //if(property.CanWrite) property.SetValue(dataSource, value);
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
