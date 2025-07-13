using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB_Samples
{
	// Token: 0x02000169 RID: 361
	public class Rotater : MonoBehaviour
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0001F4C0 File Offset: 0x0001D6C0
		private void Update()
		{
			Vector3 vector = base.transform.rotation.eulerAngles;
			vector += this.EulerSpeed * Time.deltaTime;
			base.transform.rotation = Quaternion.Euler(vector);
		}

		// Token: 0x040007AB RID: 1963
		[FormerlySerializedAs("m_EulerSpeed")]
		public Vector3 EulerSpeed = Vector3.zero;
	}
}
