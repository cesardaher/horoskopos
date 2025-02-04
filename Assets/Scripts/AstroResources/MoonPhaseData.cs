using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] [Serializable]
public class MoonPhaseData : ScriptableObject
{
    double newMoonMin = 0;
    double newMoonMax = 0.01;
    double fullMoonMin = 0.99;
    double fullMoonMax = 1;

    double phaseIncrement = 0.07; // 0.98 / 14 phases

    public List<MoonPhase> listOfMoonPhases;

    [System.Serializable]
    public class MoonPhase
    {
        [SerializeField] int id;
        [SerializeField] bool isWaxing;
        [SerializeField] double minRange;
        [SerializeField] double maxRange;
        [SerializeField] Texture2D texture;
        [SerializeField] Sprite sprite;

        public double MinRange { get { return minRange; } set { minRange = value; } }
        public double MaxRange { get { return maxRange; } set { maxRange = value; } }
        public bool IsWaxing { get { return isWaxing; } set { isWaxing = value; } }
        public int Id { get { return id; } set { id = value; } }
        public Texture2D Texture { get{return texture; } set{ texture = value; }}
        public Sprite Sprite { get { return sprite; } set { sprite = value; } }

    }

    public int[] SearchRange(double phase)
    {
        int[] doublePhase = new int[2];

        if (phase < 0) return null;
        if (phase > 1) return null;

        phase -= newMoonMax;
        phase /= phaseIncrement;

        int indexIncrement = (int)phase;

        doublePhase[0] = indexIncrement + 1; // waxing
        doublePhase[1] = (listOfMoonPhases.Count - 1) - 1 - indexIncrement; // waning

        return doublePhase;
        
    }

    private void OnValidate()
    {
        // THIS CODE IS FOR SETTING UP THE OBJECT, EXECUTE IF IT NEEDS TO BE RESET
        //DefineRanges();
        //DefineWaxingWaning();
        //SetIds();

        void DefineRanges()
        {

            for (int i = 0; i < listOfMoonPhases.Count; i++)
            {
                if (i == 0)
                {
                    listOfMoonPhases[i].MinRange = newMoonMin;
                    listOfMoonPhases[i].MaxRange = newMoonMax;
                    continue;

                }

                if (i == 15)
                {
                    listOfMoonPhases[i].MinRange = fullMoonMin;
                    listOfMoonPhases[i].MaxRange = fullMoonMax;
                    continue;
                }

                if (i == 16)
                {
                    listOfMoonPhases[i].MinRange = fullMoonMin;
                    listOfMoonPhases[i].MaxRange = fullMoonMax;
                    continue;
                }

                if (i == listOfMoonPhases.Count - 1)
                {
                    listOfMoonPhases[i].MinRange = newMoonMin;
                    listOfMoonPhases[i].MaxRange = newMoonMax;
                    continue;
                }

                if (i < 15)
                {
                    listOfMoonPhases[i].MinRange = listOfMoonPhases[0].MaxRange + ((i - 1) * phaseIncrement);
                    listOfMoonPhases[i].MaxRange = listOfMoonPhases[i].MinRange + phaseIncrement;
                    continue;
                }

                if (i > 16)
                {
                    int increment = i - 16;
                    listOfMoonPhases[i].MinRange = listOfMoonPhases[15 - increment].MinRange;
                    listOfMoonPhases[i].MaxRange = listOfMoonPhases[15 - increment].MaxRange;
                    continue;
                }

            }
        }

        void DefineWaxingWaning()
        {
            for (int i = 0; i < listOfMoonPhases.Count; i++)
            {
                int waxingMax = (listOfMoonPhases.Count / 2) + 1;

                if (i < waxingMax)
                {
                    listOfMoonPhases[i].IsWaxing = true;
                }
            }
        }

        void SetIds()
        {
            for (int i = 0; i < listOfMoonPhases.Count; i++)
            {
                listOfMoonPhases[i].Id = i;
            }
        }

    }
    
}
