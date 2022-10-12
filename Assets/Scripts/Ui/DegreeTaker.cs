using TMPro;
using UnityEngine;

public class DegreeTaker : MonoBehaviour
{
    [SerializeField] PlanetData _planetData;
    TMP_Text _textObj;
    [SerializeField] AstrologicalIdentity astrologicalIdentity;

    private void Awake()
    {
        // connect to event manager
        EventManager.Instance.OnRecalculationOfGeoData += UpdateNumber;
        EventManager.Instance.OnChartMode += UpdateNumber;
         
        // get text component
        _textObj = GetComponent<TMP_Text>();
    }

    // update values for degrees and minutes
    // called whenever there are changes in calculations
    // OR when chart mode is activated
    void UpdateNumber()
    {
        if(ModesMenu.chartModeOn)
        {
            _textObj.text = ToDoubleDigit(_planetData.LongMinSec[0].ToString()) + "°\n" + ToDoubleDigit(_planetData.LongMinSec[1].ToString()) + "'";
            _textObj.color = astrologicalIdentity.listOfPlanets[_planetData.planetID].color;
        }
            
    }
     
    // change format of single digit numbers to double digit
    string ToDoubleDigit (string number)
    {
        string ddnumber;

        if (int.Parse(number) < 10)
            ddnumber = "0" + number;
        else
            ddnumber = number;
        

        return ddnumber;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnRecalculationOfGeoData -= UpdateNumber;
        EventManager.Instance.OnChartMode -= UpdateNumber;
    }

}
