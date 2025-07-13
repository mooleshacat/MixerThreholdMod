using System;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000015 RID: 21
public class UIScaleSetter : MonoBehaviour
{
	// Token: 0x0600006B RID: 107 RVA: 0x000049AE File Offset: 0x00002BAE
	private void Awake()
	{
		base.GetComponent<Slider>().onValueChanged.AddListener(delegate(float x)
		{
			CanvasScaler.SetScaleFactor(x);
		});
	}
}
