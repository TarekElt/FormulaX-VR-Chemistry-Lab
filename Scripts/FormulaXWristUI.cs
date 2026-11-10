
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Hands;

public class FormulaXWristUI : MonoBehaviour
{
    [Header("Left Wrist Tracking")]
    public Transform xrOrigin;
    public Vector3 positionOffset = new Vector3(0f, 0.03f, 0f);
    public Vector3 rotationOffset = new Vector3(90f, 0f, 0f);

    [Header("Instruction Graphics")]
    public Image instructionImage;
    public Sprite chemicalA;
    public Sprite chemicalB;
    public Sprite mixing;
    public Sprite heating;

    private XRHandSubsystem hands;
    private readonly List<XRHandSubsystem> foundHands =
        new List<XRHandSubsystem>();

    private Canvas wristCanvas;

    void Awake()
    {
        wristCanvas = GetComponent<Canvas>();
    }

    void LateUpdate()
    {
        if (hands == null || !hands.running)
        {
            foundHands.Clear();
            SubsystemManager.GetSubsystems(foundHands);
            hands = null;

            foreach (var subsystem in foundHands)
            {
                if (subsystem.running)
                {
                    hands = subsystem;
                    break;
                }
            }
        }

        if (hands == null || xrOrigin == null)
        {
            if (wristCanvas != null)
                wristCanvas.enabled = false;
            return;
        }

        XRHand leftHand = hands.leftHand;

        if (!leftHand.isTracked ||
            !leftHand.GetJoint(XRHandJointID.Wrist)
                .TryGetPose(out Pose wristPose))
        {
            if (wristCanvas != null)
                wristCanvas.enabled = false;
            return;
        }

        if (wristCanvas != null)
            wristCanvas.enabled = true;

        Quaternion wristRotation =
            xrOrigin.rotation * wristPose.rotation;

        transform.position =
            xrOrigin.TransformPoint(wristPose.position) +
            wristRotation * positionOffset;

        transform.rotation =
            wristRotation * Quaternion.Euler(rotationOffset);
    }

    public void ShowChemicalA() => Show(chemicalA);
    public void ShowChemicalB() => Show(chemicalB);
    public void ShowMixing() => Show(mixing);
    public void ShowHeating() => Show(heating);

    private void Show(Sprite image)
    {
        if (instructionImage != null && image != null)
            instructionImage.sprite = image;
    }
}
