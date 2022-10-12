using TMPro;
using UnityEngine;

public class MidSign : Point3D
{
    [SerializeField] AstrologicalIdentity astrologicalIdentity;
    public string signName;
    public int signID;
    public int elementNumber
    {
        get
        {
            if (signID < 5) return signID;
            if (signID < 9) return signID - 4;
            return signID - 8;
        }
    }
    [SerializeField] TextMeshPro symbolText;

    //assign sprite from spritelist using indices
    public void assignSprite(int i)
    {
        symbolText.text = astrologicalIdentity.listOfSigns[i].symbol;
        //spriteRenderer.sprite = spriteList.signSprite[i];
    }
}
