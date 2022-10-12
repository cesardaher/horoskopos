using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class TutorialStagesHolder : ScriptableObject
{
    public List<TutorialStage> tutorialStages = new List<TutorialStage>();
    [SerializeField] List<TutorialStage> unusedTutorialStages = new List<TutorialStage>();

    [System.Serializable]
    public class TutorialStage
    {
        public int stage;
        public string name;
        [TextArea(10, 15)]
        public string[] infoTexts;
    }

    private void OnValidate()
    {
        for(int i = 0; i < tutorialStages.Count; i++)
        {
            tutorialStages[i].stage = i;
        }
    }

}
