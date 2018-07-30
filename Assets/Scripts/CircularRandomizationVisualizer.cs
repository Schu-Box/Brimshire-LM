using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularRandomizationVisualizer : MonoBehaviour {

	public Image failureImg;
	public Image successImg;
	public Image successBorder;
	public GameObject arrowObj;

	public void SetVisualizer(Color succ, Color fail, float succFill) {
		successImg.color = succ;
		failureImg.color = fail;
		successImg.fillAmount = succFill;
		successBorder.fillAmount = succFill + 0.002f;

		arrowObj.transform.eulerAngles = Vector3.zero;
	}
}
