using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroResources
{

    public enum SIGN
    {
        Null,
        Aries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius,
        Capricorn,
        Aquarius,
        Pisces
    }

    public enum MODALITY_SIGN
    {
        Aries = 1,
        Taurus = 2,
        Gemini = 3,
        Cancer = 1,
        Leo = 2,
        Virgo = 3,
        Libra = 1,
        Scorpio = 2,
        Sagittarius = 3,
        Capricorn = 1,
        Aquarius = 2,
        Pisces = 3,
    }

    public enum MODALITY_NAME
    {

        Null,
        Cardinal,
        Fixed,
        Mutable
    }

    public enum ELEMENT_NAME
    {
        Null,
        Fire,
        Earth,
        Air,
        Water
    }

    public enum ELEMENT_SIGN
    {
        Aries = 1 ,
        Taurus = 2,
        Gemini = 3,
        Cancer = 4,
        Leo = 1,
        Virgo = 2,
        Libra = 3,
        Scorpio = 4,
        Sagittarius = 1,
        Capricorn = 2,
        Aquarius = 3,
        Pisces = 4
    }

    public enum SEASON_SIGN
    {
        Aries = 1,
        Taurus = 1,
        Gemini = 1,
        Cancer = 2,
        Leo = 2,
        Virgo = 2,
        Libra = 3,
        Scorpio = 3,
        Sagittarius = 3,
        Capricorn = 4,
        Aquarius = 4,
        Pisces = 4
    }

    public enum SEASON
    {
        Null,
        Spring,
        Summer,
        Autumn,
        Winter
    }
}