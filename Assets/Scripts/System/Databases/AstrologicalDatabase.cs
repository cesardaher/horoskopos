using System.Collections.Generic;

public static class AstrologicalDatabase
{
    static AstrologicalDatabase()
    {
        AssignSigns();
        AssignHouseSystems();
    }

    // vars and properties
    static public Dictionary<int, string> signsByName = new Dictionary<int, string>();
    static public Dictionary<int, int> signsByModality = new Dictionary<int, int>();
    static public Dictionary<int, int> signsByElement = new Dictionary<int, int>();
    static public Dictionary<int, int> signsBySeason = new Dictionary<int, int>();

    static public Dictionary<int, string> modalityID = new Dictionary<int, string>();
    static public Dictionary<int, string> elementID = new Dictionary<int, string>();
    static public Dictionary<int, string> seasonID = new Dictionary<int, string>();
    static public Dictionary<char, string> houseSystemsByChar = new Dictionary<char, string>();
    static public Dictionary<string, char> houseSystemsByName = new Dictionary<string, char>();




    static void AssignSigns()
    {
        AssignSignsByName();
        AssignSignsByModality();
        AssignSignsByElement();

        AssignElementsAndModalities();
        AssignSignsBySeason();
    }

    static void AssignSignsByName()
    {
        signsByName.Add(1, "Aries");
        signsByName.Add(2, "Taurus");
        signsByName.Add(3, "Gemini");
        signsByName.Add(4, "Cancer");
        signsByName.Add(5, "Leo");
        signsByName.Add(6, "Virgo");
        signsByName.Add(7, "Libra");
        signsByName.Add(8, "Scorpio");
        signsByName.Add(9, "Sagittarius");
        signsByName.Add(10, "Capricorn");
        signsByName.Add(11, "Aquarius");
        signsByName.Add(12, "Pisces");
    }

    static void AssignSignsByModality()
    {
        signsByModality.Add(1, 1);
        signsByModality.Add(2, 2);
        signsByModality.Add(3, 3);
        signsByModality.Add(4, 1);
        signsByModality.Add(5, 2);
        signsByModality.Add(6, 3);
        signsByModality.Add(7, 1);
        signsByModality.Add(8, 2);
        signsByModality.Add(9, 3);
        signsByModality.Add(10, 1);
        signsByModality.Add(11, 2);
        signsByModality.Add(12, 3);
    }
    
    static void AssignSignsByElement()
    {
        signsByElement.Add(1, 1);
        signsByElement.Add(2, 2);
        signsByElement.Add(3, 3);
        signsByElement.Add(4, 4);
        signsByElement.Add(5, 1);
        signsByElement.Add(6, 2);
        signsByElement.Add(7, 3);
        signsByElement.Add(8, 4);
        signsByElement.Add(9, 1);
        signsByElement.Add(10, 2);
        signsByElement.Add(11, 3);
        signsByElement.Add(12, 4);
    }

    static void AssignSignsBySeason()
    {
        signsBySeason.Add(1, 1);
        signsBySeason.Add(2, 1);
        signsBySeason.Add(3, 1);
        signsBySeason.Add(4, 2);
        signsBySeason.Add(5, 2);
        signsBySeason.Add(6, 2);
        signsBySeason.Add(7, 3);
        signsBySeason.Add(8, 3);
        signsBySeason.Add(9, 3);
        signsBySeason.Add(10, 4);
        signsBySeason.Add(11, 4);
        signsBySeason.Add(12, 4);
    }

    static void AssignElementsAndModalities()
    {
        modalityID.Add(1, "Cardinal");
        modalityID.Add(2, "Fixed");
        modalityID.Add(3, "Mutable");

        elementID.Add(1, "Fire");
        elementID.Add(2, "Earth");
        elementID.Add(3, "Air");
        elementID.Add(4, "Water");

        seasonID.Add(1, "Spring");
        seasonID.Add(2, "Summer");
        seasonID.Add(3, "Autumn");
        seasonID.Add(4, "Winter");
    }


    static void AssignHouseSystems()
    {
        houseSystemsByChar.Add('P', "Placidus");
        houseSystemsByChar.Add('K', "Koch");
        houseSystemsByChar.Add('O', "Porphyrius");
        houseSystemsByChar.Add('R', "Regiomontanus");
        houseSystemsByChar.Add('C', "Campanus");
        houseSystemsByChar.Add('E', "Equal");
        houseSystemsByChar.Add('W', "Whole sign");

        foreach(KeyValuePair<char, string> entry in houseSystemsByChar)
        {
            houseSystemsByName.Add(entry.Value, entry.Key);
        }
    }


}
