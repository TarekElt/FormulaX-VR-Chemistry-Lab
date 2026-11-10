
using UnityEngine;

public class MixingZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LabManager labManager;
    [SerializeField] private GameObject chemicalA;
    [SerializeField] private GameObject chemicalB;

    private void OnTriggerEnter(Collider other)
    {
        if (labManager == null) return;

        // Check if the incoming collider belongs to Chemical A.
        if (BelongsTo(other, chemicalA))
        {
            if (labManager.currentStage ==
                LabManager.LabStage.ChemicalA)
            {
                labManager.AddChemicalA();
                Debug.Log("Mixing Zone: Chemical A detected!");
            }
        }

        // Check if the incoming collider belongs to Chemical B.
        else if (BelongsTo(other, chemicalB))
        {
            if (labManager.currentStage ==
                LabManager.LabStage.ChemicalB)
            {
                labManager.AddChemicalB();
                Debug.Log("Mixing Zone: Chemical B detected!");
            }
        }
    }

    private bool BelongsTo(Collider other, GameObject bottle)
    {
        if (bottle == null) return false;

        return other.transform == bottle.transform ||
               other.transform.IsChildOf(bottle.transform);
    }
}
