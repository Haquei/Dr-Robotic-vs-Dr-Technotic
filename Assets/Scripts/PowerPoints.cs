using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerPoints : MonoBehaviour
{
    public TMP_Text power_point_value_text;
    private float power_point_value;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            power_point_value += 50f;
            power_point_value_text.text = power_point_value.ToString() + "PP";
        }
    }
}
