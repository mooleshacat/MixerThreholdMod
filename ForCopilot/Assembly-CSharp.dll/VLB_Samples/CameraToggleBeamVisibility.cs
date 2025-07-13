using System;
using UnityEngine;
using VLB;

namespace VLB_Samples
{
	// Token: 0x02000164 RID: 356
	[RequireComponent(typeof(Camera))]
	public class CameraToggleBeamVisibility : MonoBehaviour
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x0001EDE0 File Offset: 0x0001CFE0
		private void Update()
		{
			if (Input.GetKeyDown(this.m_KeyCode))
			{
				Camera component = base.GetComponent<Camera>();
				int geometryLayerID = Config.Instance.geometryLayerID;
				int num = 1 << geometryLayerID;
				if ((component.cullingMask & num) == num)
				{
					component.cullingMask &= ~num;
					return;
				}
				component.cullingMask |= num;
			}
		}

		// Token: 0x04000799 RID: 1945
		[SerializeField]
		private KeyCode m_KeyCode = KeyCode.Space;
	}
}
