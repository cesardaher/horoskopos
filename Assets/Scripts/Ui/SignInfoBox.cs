using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignInfoBox : InfoBox
{
    public static List<SignInfoBox> signInfoBoxes = new List<SignInfoBox>();

    [SerializeField] AstrologicalIdentity _astroIdentity;

    [Header("UI Objects")]
    [SerializeField] TextMeshProUGUI _signName;
    [SerializeField] TextMeshProUGUI _signSymbol;
    [SerializeField] TextMeshProUGUI _cuspsValue;
    [SerializeField] List<TextMeshProUGUI> _rulerComponents = new List<TextMeshProUGUI>();
    [SerializeField] List<GameObject> currentPlanetObjs = new List<GameObject>();

    List<CurrentPlanetTextGroup> planetTextGroups = new List<CurrentPlanetTextGroup>();

    override public int AstroID
    {
        get { return astroID; }
        set
        {
            astroID = value;
            EventManager.Instance.ApplySignIdentity(astroID, this);
            ChangeValues();
        }
    }

    new private void Awake()
    {
        base.Awake();

        SetupPlanetTextGroups();
        ClosePlanetGroups();

        EventManager.Instance.OnRecalculationOfGeoData += ChangeValues;
    }

    void ClosePlanetGroups()
    {
        foreach(GameObject obj in currentPlanetObjs)
        {
            obj.SetActive(false);
        }
    }

    void ChangeValues()
    {

        // change cusps value
        List<int> cusps = GetCuspList(AstroID); 
        _cuspsValue.text = CuspListToString(cusps);

        PlanetListToSymbols();

        SetRuler();

        void SetRuler()
        {
            int rulerId = _astroIdentity.listOfSigns[AstroID].rulerId;
            string rulerSymbol = _astroIdentity.listOfPlanets[rulerId].symbol;
            _rulerComponents[0].text = rulerSymbol;

            int rulerSignId = PlanetData.PlanetDataList[rulerId].SignID;

            string rulerLongitude = DegreesStringFormat(PlanetData.PlanetDataList[rulerId].LongMinSec);
            _rulerComponents[1].text = rulerLongitude;
            _rulerComponents[1].color = _astroIdentity.listOfSigns[rulerSignId].secondaryColor;

            string rulerSign = _astroIdentity.listOfSigns[rulerSignId].symbol;
            _rulerComponents[2].text = rulerSign;
            _rulerComponents[2].color = _astroIdentity.listOfSigns[rulerSignId].secondaryColor;
        }

        void PlanetListToSymbols()
        {

            List<int> planetInSignList = new List<int>();

            for (int p = 0; p < PlanetData.PlanetDataList.Count; p++)
            {
                if (p > 11) continue;

                // check within signs list (signId) for that particular planet (i)
                if (ZodiacData.instance.signDataList[AstroID].planets[p] == 1)
                {
                    planetInSignList.Add(p);
                }
            }

            for(int t = 0; t < planetTextGroups.Count; t++)
            {
                // apply name and symbol to planet boxes with color
                if(t < planetInSignList.Count)
                {
                    if (!planetTextGroups[t].parent.activeSelf) planetTextGroups[t].parent.SetActive(true);
                    planetTextGroups[t].planetName.text = _astroIdentity.listOfPlanets[planetInSignList[t]].name;
                    planetTextGroups[t].planetName.color = _astroIdentity.listOfPlanets[planetInSignList[t]].secondaryColor;

                    planetTextGroups[t].planetSymbol.text = _astroIdentity.listOfPlanets[planetInSignList[t]].symbol;
                    planetTextGroups[t].planetSymbol.color = _astroIdentity.listOfPlanets[planetInSignList[t]].secondaryColor;
                    continue;
                }

                // close irrelevant boxes
                if (planetTextGroups[t].parent.activeSelf) planetTextGroups[t].parent.SetActive(false);

            }
        }

        
        string CuspListToString(List<int> cuspList)
        {
            string cuspNames = "";
           /* if (cuspList.Count == 0) return "";
            if (cuspList.Count == 1) return HouseInRomanNumerals(cuspList[0]);
            if (cuspList.Count == 2) return HouseInRomanNumerals(cuspList[0]) + ", " + HouseInRomanNumerals(cuspList[1]);
            if (cuspList.Count == 3) return HouseInRomanNumerals(cuspList[0]) + ", " + HouseInRomanNumerals(cuspList[1]) + ", " + HouseInRomanNumerals(cuspList[3]);*/

            for(int i = 0; i < cuspList.Count; i++)
            {
                cuspNames += HouseInRomanNumerals(cuspList[i]);
                
                if (i < cuspList.Count - 1) cuspNames += ", ";
            } 

            return cuspNames;
        }

        List<int> GetCuspList(int id)
        {
            List<int> cusps = new List<int>();

            for (int i = 1; i < HouseData.instance.houseDataList.Count; i++)
            {
                if (HouseData.instance.houseDataList[i].signID == id)
                    cusps.Add(i);
            }

            return cusps;
        }
    }

    [System.Serializable]
    class CurrentPlanetTextGroup
    {
        public GameObject parent;
        public TextMeshProUGUI planetName;
        public TextMeshProUGUI planetSymbol;

        public CurrentPlanetTextGroup(GameObject par, TextMeshProUGUI name, TextMeshProUGUI symbol)
        {
            parent = par;
            planetName = name;
            planetSymbol = symbol;
        }
    }

    void SetupPlanetTextGroups()
    {
        foreach (GameObject obj in currentPlanetObjs)
        {
            TextMeshProUGUI name = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI symbol = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            planetTextGroups.Add(new CurrentPlanetTextGroup(obj, name, symbol));
        }
    }

    string ToDoubleDigit(int val)
    {
        string finalVal = val.ToString();

        if (val < 10)
        {
            finalVal = "0" + finalVal;
            return finalVal;
        }

        return finalVal;
    }

    string StringToDoubleDigit(string val)
    {
        int intVal = int.Parse(val);

        if (intVal < 10)
        {
            val = 0 + val;
            return val;
        }

        return val;

    }

    string DegreesStringFormat(int[] deg)
    {
        string minus = "-";
        string val = "";

        if (deg[0] < 0 || deg[1] < 0 || deg[2] < 0) val = minus;

        val += ToDoubleDigit(Math.Abs(deg[0])) + "°" + ToDoubleDigit(Math.Abs(deg[1])) + "\'" + ToDoubleDigit(Math.Abs(deg[2])) + "\"";

        return val;
    }

    string HouseInRomanNumerals(int val)
    {
        string finalVal;

        if (val == 1) return finalVal = "I";
        if (val == 4) return finalVal = "IV";
        if (val == 7) return finalVal = "VII";
        if (val == 10) return finalVal = "X";

        return finalVal = val.ToString();
    }

    private void OnDestroy()
    {
        InfoBoxSpawner.signInfoBoxes.Remove(this);
        EventManager.Instance.OnRecalculationOfGeoData -= ChangeValues;
    }

}
