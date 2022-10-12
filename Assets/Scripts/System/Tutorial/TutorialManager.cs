using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AstroResources;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public int counter;
    static public bool isInTutorial = false;

    [SerializeField] TutorialStagesHolder tutorialStagesHolder;
    public Dictionary<int, string> tutorialStages = new Dictionary<int, string>();

    [Header("Buttons")]
    [SerializeField] GameObject modesMenu;
    [SerializeField] GameObject openChartButton;
    [SerializeField] GameObject dataPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject viewPanel;
    [SerializeField] GameObject followSunPanel;
    [SerializeField] Button skyViewButton;
    [SerializeField] Button chartViewButton;

    [Header("Tutorial Box")]
    [SerializeField] GameObject tutorialBox;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Button continueButton;
    [SerializeField] Button backButton;

    [Header("Misc")]
    [SerializeField] SeasonDrawer seasonDrawer;
    [SerializeField] UnityTemplateProjects.CameraController cameraController;

    [Header("Story Triggers")]
    public bool findSun;
    public bool findMoon;
    public bool openChart;
    public bool viewSky;
    public bool viewChartMode;

    private void Awake()
    {
        InitializeTutorialPoints();

        if (Instance is null)
            Instance = this;
        else Debug.LogWarning("More than one TutorialManager. Delete this.");

        DontDestroyOnLoad(gameObject);
        // add tutorial to game's events
        EventManager.Instance.OnTutorialAllowContinue += AllowContinue;
        EventManager.Instance.OnTutorialBacktrack += PreviousStage;
        EventManager.Instance.OnTutorialContinue += NextStage;
        EventManager.Instance.OnTitleScreenReturn += EndTutorial; 
    }

    private void Start()
    {
        //tutorialDataManager = FindObjectOfType<TutorialDataManager>();
        // turn off tutorial by default
        tutorialBox.SetActive(false);

        // TODO: might be redundant, possibly remove
        ToggleTutorialState(isInTutorial);

        CheckIfTutorialStarts();
    }

    void InitializeTutorialPoints()
    {
        for(int i = 0; i < tutorialStagesHolder.tutorialStages.Count; i++)
        {
            tutorialStages.Add(i, tutorialStagesHolder.tutorialStages[i].name);
        }
    }

    void ToggleTutorialState(bool val)
    {
        isInTutorial = val;

        // apply it to other relevant scripts
        EventManager.Instance.ToggleTutorial(val);
    }

    void CheckIfTutorialStarts()
    {
        // turn tutorial on if that is the case
        if (isInTutorial)
        {
            counter = 0;
            
            openChartButton.SetActive(false);

            skyViewButton.gameObject.SetActive(false);
            chartViewButton.gameObject.SetActive(false);
            openChartButton.SetActive(false);

            dataPanel.SetActive(false);
            settingsPanel.SetActive(false);
            viewPanel.SetActive(false);

            StartTutorialStage();

        }
    }

    void AllowContinue()
    {
        continueButton.interactable = true;
    }

    void NextStage()
    {
        counter += 1;
        StartTutorialStage();
    }

    void PreviousStage()
    {
        counter -= 1;
        StartTutorialStage();
    }

    void StartTutorialStage()
    {
        if (counter >= tutorialStages.Count)
        {
            EndTutorial();
            return;
        }

        Invoke(tutorialStages[counter], 0);
    }

    void Instructions()
    {
        // open tutorial box
        tutorialBox.SetActive(true);
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        
        // turn off back button
        backButton.interactable = false;
        // turn on continue button
        continueButton.interactable = true;

        Target.inTutorial = true;
        SignTarget.inTutorial = true;
    }

    // find the sun
    // continue when sun is found
    void FindSun()
    {

        // apply text
        tutorialText.text = GetProperTutorialText(tutorialStagesHolder.tutorialStages[counter]);
        // turn off continue button
        continueButton.interactable = false;
        // turn on back button
        backButton.interactable = true;
        //continueButton.gameObject.SetActive(false);

        findSun = true;
    }

    // sun explanation
    // continue on button
    void SunExplanation()
    {
        // apply text
        tutorialText.text = string.Format(GetProperTutorialText(tutorialStagesHolder.tutorialStages[counter]),
            GetHorizontalPosition()[0],
            GetHorizontalPosition()[1],
            PlanetData.PlanetDataList[0].Sign,
            GetModalityPart(),
            GetElementSeason());

        // turn on button
        backButton.interactable = true;
        continueButton.interactable = true;
        //continueButton.gameObject.SetActive(true);

        string[] GetHorizontalPosition()
        {
            string[] horizonWords = new string[2];

            if(PlanetData.PlanetDataList[0].realPlanet.TrAlt > 0)
            {
                horizonWords[0] = "above";
                horizonWords[1] = "day";

                return horizonWords;
            }

            horizonWords[0] = "below";
            horizonWords[1] = "night";

            return horizonWords;
        }

        string GetModalityPart()
        {
            int signId = PlanetData.PlanetDataList[0].SignID;
            int modeId = AstrologicalDatabase.signsByModality[signId];

            switch (modeId)
            {
                case (int)MODALITY_NAME.Cardinal:
                    return "beginning";
                case (int)MODALITY_NAME.Fixed:
                    return "middle";
                case (int)MODALITY_NAME.Mutable:
                    return "end";
                default:
                    Debug.LogError("Invalid modality ID for Sign");
                    return "middle";
            }
        }

        string GetElementSeason()
        {
            int signId = PlanetData.PlanetDataList[0].SignID;
            int seasonId = AstrologicalDatabase.signsBySeason[signId];
            string seasonName = AstrologicalDatabase.seasonID[seasonId];

            return seasonName;
        }
    }

    // find moon
    // continue on moon click
    void FindMoon()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn off button
        //continueButton.gameObject.SetActive(false);
        continueButton.interactable = false;

        findMoon = true;
    }

    // moon explanation
    // continue on button
    void MoonExplanation()
    {
        // apply text
        tutorialText.text = string.Format(GetProperTutorialText(tutorialStagesHolder.tutorialStages[counter]),
            PhaseKeywords()[0],
            PhaseKeywords()[1],
            PlanetData.PlanetDataList[1].Sign,
            GetModalityPart(),
            GetElementSeason());

        // turn on button
        continueButton.interactable = true;
        //continueButton.gameObject.SetActive(true);

        string[] PhaseKeywords()
        {
            string[] keywords = new string[2];

            if(PlanetData.PlanetDataList[1].PhaseState > 1)
            {
                keywords[0] = "away from";
                keywords[1] = "waxing";
            }
            else
            {
                keywords[0] = "towards";
                keywords[1] = "waning";
            }

            if (FindConjunction())
            {
                keywords[1] += ". It is also a new moon.";
                return keywords;
            }

            if (FindOpposition())
            {
                keywords[0] += ". It is also a full moon.";
                return keywords;
            }

            return keywords;

            bool FindConjunction()
            {
                double longSun = PlanetData.PlanetDataList[0].Longitude;
                double longMoon = PlanetData.PlanetDataList[1].Longitude;

                if(Math.Abs(Get360Distance(longSun, longMoon)) < 12)
                {
                    return true;
                }

                return false;
            }

            bool FindOpposition()
            {
                double longSun = PlanetData.PlanetDataList[0].Longitude;
                double longMoon = PlanetData.PlanetDataList[1].Longitude;
                double absDist = Math.Abs(Get360Distance(longSun, longMoon));

                if (absDist > 168 && absDist < 192)
                {
                    return true;
                }

                return false;
            }


            double Get360Distance(double x, double y)
            {
                double diff = x - y;

                if (Math.Abs(diff) > 180)
                {
                    diff -= 360;
                }
                return diff;
            }
        }

        string GetModalityPart()
        {
            int signId = PlanetData.PlanetDataList[1].SignID;
            int modeId = AstrologicalDatabase.signsByModality[signId];

            switch (modeId)
            {
                case (int)MODALITY_NAME.Cardinal:
                    return "beginning";
                case (int)MODALITY_NAME.Fixed:
                    return "middle";
                case (int)MODALITY_NAME.Mutable:
                    return "end";
                default:
                    Debug.LogError("Invalid modality ID for Sign");
                    return "middle";
            }
            
        }

        string GetElementSeason()
        {
            int signId = PlanetData.PlanetDataList[1].SignID;
            int seasonId = AstrologicalDatabase.signsBySeason[signId];
            string seasonName = AstrologicalDatabase.seasonID[seasonId];

            return seasonName;
        }
    }

    // planets explanation
    // continue on button
    void PlanetsExplanation()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn on button
        continueButton.interactable = true;
        //continueButton.gameObject.SetActive(true);

        // BACK
        // turn on chart button
        openChartButton.SetActive(false);
    }

    // chart prompt
    // continue on chart button
    void OpenChart()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn off button
        continueButton.interactable = false;
        //continueButton.gameObject.SetActive(false);
        // turn on chart button
        openChartButton.SetActive(true);
        openChart = true;
    }

    // chart explanation
    // continue on button
    void ExplainChart()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn on button
        continueButton.interactable = true;
        //continueButton.gameObject.SetActive(true);

        // BACK
        // turn on modes toggler
        chartViewButton.gameObject.SetActive(false);
        skyViewButton.gameObject.SetActive(false);
    }

    // views prompt
    // continue on Sky view
    void ChartViewPrompt()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn off button

        if (ModesMenu.chartModeOn) continueButton.interactable = true;
        else continueButton.interactable = false;
        //continueButton.gameObject.SetActive(false);
        // turn on modes toggler
        chartViewButton.gameObject.SetActive(true);
        skyViewButton.gameObject.SetActive(true);

        viewChartMode = true;

        // deactivate ecliptic glow
        EventManager.Instance.ToggleEclipticGlow(false);
    }

    // ecliptic explanation
    // continue on button
    void EclipticExplanation()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];

        // turn on button
        continueButton.interactable = true;

        // activate ecliptic glow
        EventManager.Instance.ToggleEclipticGlow(true);

        // turn off season drawer for back button
        seasonDrawer.gameObject.SetActive(false);

        // BACK
        // revert zodiac color
        EventManager.Instance.RevertZodiacColor();
    }

    // zodiac explanation
    // continue on button
    void ZodiacExplanation()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];

        // activate sign symbols glow
        EventManager.Instance.DivideZodiacBySeason();

        // turn on button
        continueButton.interactable = true;

        // draw seasons labels
        seasonDrawer.gameObject.SetActive(true);
        seasonDrawer.DrawSeasonsLabel();
    }

    // zodiac explanation
    // continue on button
    void ZodiacDivision()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];

        // activate sign symbols glow
        EventManager.Instance.ToggleSignSymbolsGlow(true);

        // activate sign symbol colors
        EventManager.Instance.DivideZodiacByElement();

        // turn on button
        continueButton.interactable = true;

        // BACK
        // turn off season labels
        seasonDrawer.gameObject.SetActive(false);
    }

    // constellation prompts
    // continue on button
    void ConstellationPrompt()
    {
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn off button
        continueButton.interactable = false;
        //continueButton.gameObject.SetActive(false);
        // turn on sky view button
        skyViewButton.interactable = true;
        // allow continuing if already on chartmode
        if (!ModesMenu.chartModeOn) continueButton.interactable = true;
        // deactivate ecliptic and sing symbols glow
        EventManager.Instance.ToggleSignSymbolsGlow(false);
        EventManager.Instance.RevertZodiacColor();
        EventManager.Instance.ToggleEclipticGlow(false);

        viewSky = true;

        //BACK
        //turn on constellations
        EventManager.Instance.UseConstellations(false);
    }

    // constellation and zodiacs explanation
    // continue on button
    void ConstellationAndZodiacExplanation()
    {
        // look at midheaven
        cameraController.TargetAngle(1);
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn on button
        continueButton.interactable = true;
        // turn off chart view button
        chartViewButton.interactable = false;
        //turn on constellations
        EventManager.Instance.UseConstellations(true);
    }

    // constellation and zodiacs explanation
    // continue on button
    void ConstellationAndZodiacExplanation2()
    {
        // turn on button
        continueButton.interactable = true;

        int nextSignId = PlanetData.PlanetDataList[0].SignID - 1;
        if (nextSignId == -1) nextSignId = 12;
        string nextSignName = AstrologicalDatabase.signsByName[nextSignId];
        // apply text
        tutorialText.text = string.Format(tutorialStagesHolder.tutorialStages[counter].infoTexts[0],
            PlanetData.PlanetDataList[0].Sign,
            nextSignName);

        // BACK
        // turn on data panel
        dataPanel.SetActive(false);
        // turn on data panel
        settingsPanel.SetActive(false);
        // turn on view options panel
        viewPanel.SetActive(false);
    }

    // constellation and zodiacs explanation
    // continue on button
    void ConstellationAndZodiacExplanation3()
    {
        // turn on button
        continueButton.interactable = true;
        // apply text
        tutorialText.text = tutorialStagesHolder.tutorialStages[counter].infoTexts[0];
        // turn on chart view button
        chartViewButton.interactable = true;
        //end tutorial
        ToggleTutorialState(false);
        //turn off constellations
        EventManager.Instance.UseConstellations(false);

    }

    void EndTutorial()
    {
        // reset tutorial counter and state
        counter = 0;

        // close tutorial box
        tutorialBox.SetActive(false);

        // destroy object and clear instance
        Destroy(Instance.gameObject);
        Instance = null;
    }

    string GetProperTutorialText(TutorialStagesHolder.TutorialStage tutorialStage)
    {
        // use current time text
        if (!TutorialDataManager.isUsingBirthChart)
            return tutorialStage.infoTexts[2];
        // use default-time text
        if (TutorialDataManager.isBirthTimeUnknown)
            return tutorialStage.infoTexts[1];

        // use full birth chart text
        return tutorialStage.infoTexts[0];
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnTutorialAllowContinue -= AllowContinue;
        EventManager.Instance.OnTutorialContinue -= NextStage;
        EventManager.Instance.OnTitleScreenReturn -= EndTutorial;
        EventManager.Instance.OnTutorialBacktrack -= PreviousStage;
    }
}
