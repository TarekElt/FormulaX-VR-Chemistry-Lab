
using UnityEngine;
using TMPro;

public class LabUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private LabManager labManager;

    [Header("UI Panels")]
    [SerializeField] private GameObject introductionPanel;
    [SerializeField] private GameObject formulaPanel;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject conclusionPanel;

    [Header("Instruction Text")]
    [SerializeField] private TMP_Text instructionText;

    [Header("Bottles (For Restart)")]
    [SerializeField] private Transform chemicalA;
    [SerializeField] private Transform chemicalB;

    private Vector3 aStartPosition;
    private Vector3 bStartPosition;
    private Quaternion aStartRotation;
    private Quaternion bStartRotation;

    private LabManager.LabStage displayedStage;
    private bool initialized;

    private void Awake()
    {
        if (chemicalA != null)
        {
            aStartPosition = chemicalA.position;
            aStartRotation = chemicalA.rotation;
        }

        if (chemicalB != null)
        {
            bStartPosition = chemicalB.position;
            bStartRotation = chemicalB.rotation;
        }
    }

    private void Start()
    {
        RefreshUI();
    }

    private void Update()
    {
        if (labManager == null) return;

        if (!initialized ||
            labManager.currentStage != displayedStage)
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (labManager == null) return;

        var stage = labManager.currentStage;

        introductionPanel.SetActive(
            stage == LabManager.LabStage.Introduction);

        formulaPanel.SetActive(
            stage == LabManager.LabStage.FormulaSelection);

        conclusionPanel.SetActive(
            stage == LabManager.LabStage.Completed);

        bool showInstructions =
            stage == LabManager.LabStage.ChemicalA ||
            stage == LabManager.LabStage.ChemicalB ||
            stage == LabManager.LabStage.Mixing ||
            stage == LabManager.LabStage.Heating;

        instructionPanel.SetActive(showInstructions);

        switch (stage)
        {
            case LabManager.LabStage.ChemicalA:
                instructionText.text =
                    "STEP 1: ADD CHEMICAL A\n" +
                    "Place the BLUE bottle in the mixing zone.";
                break;

            case LabManager.LabStage.ChemicalB:
                instructionText.text =
                    "STEP 2: ADD CHEMICAL B\n" +
                    "Place the RED bottle in the mixing zone.";
                break;

            case LabManager.LabStage.Mixing:
                instructionText.text =
                    "STEP 3: MIX CHEMICALS\n" +
                    "Press the GREEN mixing button.";
                break;

            case LabManager.LabStage.Heating:
                instructionText.text =
                    "STEP 4: HEAT MIXTURE\n" +
                    "Press the RED heating button.";
                break;
        }

        displayedStage = stage;
        initialized = true;
    }

    [ContextMenu("Test/Start Button")]
    public void StartButtonPressed()
    {
        labManager.StartExperiment();
        RefreshUI();
    }

    [ContextMenu("Test/Select Formula")]
    public void SelectFormulaPressed()
    {
        labManager.SelectFormula();
        RefreshUI();
    }

    [ContextMenu("Test/Restart")]
    public void RestartPressed()
    {
        RestoreBottle(
            chemicalA, aStartPosition, aStartRotation);

        RestoreBottle(
            chemicalB, bStartPosition, bStartRotation);

        Physics.SyncTransforms();

        labManager.ResetExperiment();
        RefreshUI();
    }

    private void RestoreBottle(
        Transform bottle, Vector3 position,
        Quaternion rotation)
    {
        if (bottle == null) return;

        Rigidbody rb = bottle.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        bottle.SetPositionAndRotation(position, rotation);
    }
}
