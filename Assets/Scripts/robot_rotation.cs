using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class robot_rotation : MonoBehaviour
{
    public GameObject[] arms = { };

    public Slider[] sliders = { };

    public Button grabButton;
    public TMP_Text buttonText;
    bool grabbing_or_not = false;
    private GameObject grabbingArm;
    private Collider cube;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < arms.Length; ++i)
        {
            float rotation = ((i == 0) ? arms[i].transform.rotation.eulerAngles.y : arms[i].transform.rotation.eulerAngles.z);
            float percentage = RotToPercentage(rotation);
            sliders[i].value = percentage;

            //sliders[i].onValueChanged.AddListener((float value) => SetRotation(i, value));
        }

        SetButtonText("GRAB");
        grabbing_or_not = false;

        grabButton.onClick.AddListener(Grab);
    }

    private float RotToPercentage(float rotation)
    {
        float percentage = rotation / 360f;
        return percentage;
    }

    private float PercentageToRot(float percentage)
    {
        float rotation = percentage * 360f;
        return rotation;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < arms.Length; ++i)
        {
            float rotation = PercentageToRot(sliders[i].value);
            arms[i].transform.rotation = Quaternion.Euler(arms[i].transform.rotation.eulerAngles.x, (i == 0) ? rotation : arms[i].transform.rotation.eulerAngles.y, (i != 0) ? rotation : arms[i].transform.rotation.eulerAngles.z);
        }
    }

    public void SetRotation(int arm_index, float alpha)
    {
        if (arm_index < 0 || arm_index >= arms.Length)
            return;

        float rotation = PercentageToRot(alpha);
        arms[arm_index].transform.rotation = Quaternion.Euler(0, (arm_index == 0) ? rotation : 0, (arm_index != 0) ? rotation : 0);
    }

    public void Grab()
    {
        if (cube == null)
            return;
        grabbing_or_not = !grabbing_or_not;
        if (grabbing_or_not)
        {
            LockCube();
            SetButtonText("RELEASE");
        }
        else
        {
            UnLockCube();
            SetButtonText("GRAB");
        }
    }

    private void SetButtonText(string text)
    {
        buttonText.text = text;
    }

    private void LockCube()
    {
        if (cube == null)
            return;
        cube.transform.SetParent(grabbingArm.transform, true);
        cube.attachedRigidbody.isKinematic = true;
    }

    private void UnLockCube()
    {
        if (cube == null)
            return;

        cube.transform.SetParent(null, true);
        cube.attachedRigidbody.isKinematic = false;
    }

    public void CanGrab(Collider cube, GameObject parent)
    {
        this.grabbingArm = parent;
        this.cube = cube;
    }

    public void CantGrab()
    {
        this.cube = null;
    }
}
