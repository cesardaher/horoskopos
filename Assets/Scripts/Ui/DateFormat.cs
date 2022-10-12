using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateFormat : MonoBehaviour
{
    TMP_Dropdown dropdown;
    public TextMeshProUGUI textBox;
    public int minVal = 1;
    public int maxVal;

    private void Awake()
    {
        StartDropDown();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void StartDropDown()
    {
        dropdown = transform.GetComponent<TMP_Dropdown>();

        dropdown.options.Clear();

        List<string> valueList = new List<string>();
        valueList.Add(gameObject.name);

        valueList = PopulateDropDown(valueList);

        // add items to dropdown menu
        foreach (string item in valueList)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() { text = item });
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });

    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;

        textBox.text = dropdown.options[index].text;
    }

    List<string> PopulateDropDown(List<string> list)
    {
        if(gameObject.name == "Day")
        {
            maxVal = 31; // TODO: variate according to month

            for(int i = minVal; i <= maxVal; i++)
            {
                list.Add(i.ToString());
            }

            return list;
        }

        if (gameObject.name == "Month")
        {
            maxVal = 12;

            for (int i = minVal; i <= maxVal; i++)
            {
                list.Add(i.ToString());
            }

            return list;
        }

        if (gameObject.name == "Year")
        {
            maxVal = 2021; // TODO: expand year list

            int tempVal = maxVal;

            for (int i = minVal; i <= maxVal; i++)
            {
                list.Add(maxVal.ToString());

                maxVal -= 1;
            }

            return list;
        }

        if (gameObject.name == "Hour")
        {
            minVal = 00;
            maxVal = 23;

            for (int i = minVal; i <= maxVal; i++)
            {
                list.Add(i.ToString());
            }

            return list;
        }

        if (gameObject.name == "Minute")
        {
            minVal = 00;
            maxVal = 59;

            for (int i = minVal; i <= maxVal; i++)
            {
                list.Add(i.ToString());
            }

            return list;
        }

        return list;
    }


}
