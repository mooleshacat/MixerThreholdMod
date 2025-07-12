using System;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000011 RID: 17
public class InvertMouseSetter : MonoBehaviour
{
	// Token: 0x06000061 RID: 97 RVA: 0x00004912 File Offset: 0x00002B12
	private void Awake()
	{
		base.GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool x)
		{
			Singleton<Settings>.Instance.InvertMouse = x;
		});
	}
}
