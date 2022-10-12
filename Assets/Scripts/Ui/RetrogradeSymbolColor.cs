using UnityEngine;
using TMPro;

public class RetrogradeSymbolColor : MonoBehaviour
{
    [SerializeField] AstrologicalIdentity astrologicalIdentity;
    [SerializeField] int planetId;
    [SerializeField] TMP_Text textMesh;

    private void Start()
    {
        textMesh.color = astrologicalIdentity.listOfPlanets[planetId].color;
    }
}
