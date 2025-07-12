using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000013 RID: 19
public class SensitivitySetter : MonoBehaviour
{
	// Token: 0x06000066 RID: 102 RVA: 0x00004964 File Offset: 0x00002B64
	private void Awake()
	{
		base.GetComponent<Slider>().onValueChanged.AddListener(delegate(float x)
		{
			Singleton<Settings>.Instance.LookSensitivity = x;
		});
	}
}
