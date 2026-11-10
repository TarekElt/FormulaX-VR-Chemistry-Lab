
using UnityEngine;
using System.Collections;

public class LabManager : MonoBehaviour
{
    public enum LabStage
    {
        Introduction,
        FormulaSelection,
        ChemicalA,
        ChemicalB,
        Mixing,
        Heating,
        Completed
    }

    [Header("Experiment State")]
    public LabStage currentStage = LabStage.Introduction;

    [Header("Mixing Beaker")]
    [SerializeField] private Renderer mixingLiquidRenderer;

    [Header("Liquid Colors")]
    [SerializeField] private Color chemicalAColor = Color.blue;
    [SerializeField] private Color combinedColor = new Color(0.5f, 0f, 0.8f);
    [SerializeField] private Color mixedColor = new Color(0.8f, 0.3f, 1f);
    [SerializeField] private Color completedColor = Color.green;

    [Header("Reaction Particles")]
    public ParticleSystem mixingReactionParticles;
    public ParticleSystem heatingReactionParticles;

    [Header("Heating Reaction")]
    public float conclusionDelay = 3f;

    [Header("Wrist Instructions")]
    public FormulaXWristUI wristUI;

    private Material liquidMaterial;
    private string colorProperty;
    private Color initialColor = Color.white;

    private bool heatingReactionStarted = false;

    void Awake()
    {
        if (mixingLiquidRenderer != null)
        {
            liquidMaterial = mixingLiquidRenderer.material;

            if (liquidMaterial.HasProperty("_BaseColor"))
                colorProperty = "_BaseColor";
            else if (liquidMaterial.HasProperty("_Color"))
                colorProperty = "_Color";

            if (colorProperty != null)
                initialColor = liquidMaterial.GetColor(colorProperty);
            else
                Debug.LogWarning(
                    "Mixing liquid shader has no supported color property."
                );
        }

        ResetExperiment();
    }

    private void ChangeLiquidColor(Color newColor)
    {
        if (liquidMaterial == null || colorProperty == null)
            return;

        // Preserve the liquid's original transparency.
        newColor.a = initialColor.a;

        liquidMaterial.SetColor(colorProperty, newColor);
    }

    private void AdvanceTo(LabStage nextStage)
    {
        currentStage = nextStage;

        Debug.Log(
            "FORMULA X - Current step: " + currentStage
        );
    }

    // =====================================
    // INTRODUCTION
    // =====================================

    [ContextMenu("Test/1 - Start Experiment")]
    public void StartExperiment()
    {
        if (currentStage != LabStage.Introduction)
            return;

        Debug.Log("Welcome to Formula X!");

        AdvanceTo(LabStage.FormulaSelection);
    }

    // =====================================
    // FORMULA SELECTION
    // =====================================

    [ContextMenu("Test/2 - Select Formula")]
    public void SelectFormula()
    {
        if (currentStage != LabStage.FormulaSelection)
            return;

        Debug.Log("Formula X selected. Add Chemical A.");

        AdvanceTo(LabStage.ChemicalA);

        wristUI?.ShowChemicalA();
    }

    // =====================================
    // CHEMICAL A
    // =====================================

    [ContextMenu("Test/3 - Add Chemical A")]
    public void AddChemicalA()
    {
        if (currentStage != LabStage.ChemicalA)
            return;

        ChangeLiquidColor(chemicalAColor);

        Debug.Log("Chemical A added!");

        AdvanceTo(LabStage.ChemicalB);

        wristUI?.ShowChemicalB();
    }

    // =====================================
    // CHEMICAL B
    // =====================================

    [ContextMenu("Test/4 - Add Chemical B")]
    public void AddChemicalB()
    {
        if (currentStage != LabStage.ChemicalB)
            return;

        ChangeLiquidColor(combinedColor);

        Debug.Log("Chemical B added!");

        AdvanceTo(LabStage.Mixing);

        wristUI?.ShowMixing();
    }

    // =====================================
    // MIXING REACTION
    // =====================================

    [ContextMenu("Test/5 - Activate Mixer")]
    public void ActivateMixer()
    {
        if (currentStage != LabStage.Mixing)
            return;

        // Change liquid to bright purple.
        ChangeLiquidColor(mixedColor);

        Debug.Log("Mixing complete!");

        // Advance to the heating stage.
        AdvanceTo(LabStage.Heating);

        // Update wrist instruction.
        wristUI?.ShowHeating();

        // Play purple mixing particles.
        if (mixingReactionParticles != null)
        {
            mixingReactionParticles.Play();
        }
    }

    // =====================================
    // HEATING REACTION
    // =====================================

    [ContextMenu("Test/6 - Activate Heater")]
    public void ActivateHeater()
    {
        if (currentStage != LabStage.Heating)
            return;

        // Prevent multiple heating activations.
        if (heatingReactionStarted)
            return;

        heatingReactionStarted = true;

        Debug.Log("Heating reaction activated!");

        // Immediately turn the liquid green.
        ChangeLiquidColor(completedColor);

        // Play the dramatic green particle effect.
        if (heatingReactionParticles != null)
        {
            heatingReactionParticles.Play();
        }

        // Wait before showing the conclusion.
        StartCoroutine(DelayedConclusion());
    }

    // =====================================
    // DELAYED CONCLUSION
    // =====================================

    private IEnumerator DelayedConclusion()
    {
        Debug.Log("Waiting for final reaction...");

        // Give players time to observe the reaction.
        yield return new WaitForSeconds(conclusionDelay);

        // Only finish if we're still heating.
        if (currentStage != LabStage.Heating)
            yield break;

        // Advance to the completion screen.
        AdvanceTo(LabStage.Completed);

        Debug.Log(
            "EXPERIMENT COMPLETE! Formula X created!"
        );
    }

    // =====================================
    // RESET EXPERIMENT
    // =====================================

    [ContextMenu("Test/Reset Experiment")]
    public void ResetExperiment()
    {
        // Cancel any pending conclusion countdown.
        StopAllCoroutines();

        // Allow heating to be activated again.
        heatingReactionStarted = false;

        // Restore introduction stage.
        currentStage = LabStage.Introduction;

        // Restore original liquid color.
        ChangeLiquidColor(initialColor);

        // Clear purple particles.
        if (mixingReactionParticles != null)
        {
            mixingReactionParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        // Clear green particles.
        if (heatingReactionParticles != null)
        {
            heatingReactionParticles.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );
        }

        Debug.Log("Formula X experiment reset.");
    }
}
