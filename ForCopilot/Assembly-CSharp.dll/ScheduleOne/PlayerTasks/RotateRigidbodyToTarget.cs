using System;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000356 RID: 854
	public class RotateRigidbodyToTarget : MonoBehaviour
	{
		// Token: 0x06001328 RID: 4904 RVA: 0x00053208 File Offset: 0x00051408
		public void FixedUpdate()
		{
			this.Bitch.localRotation = Quaternion.Euler(this.TargetRotation);
			Quaternion rotation = this.Bitch.rotation;
			Quaternion quaternion = rotation * Quaternion.Inverse(base.transform.rotation);
			Vector3 a = Vector3.Normalize(new Vector3(quaternion.x, quaternion.y, quaternion.z)) * this.RotationForce;
			float d = Mathf.Clamp01(Quaternion.Angle(base.transform.rotation, rotation) / 90f);
			this.Rigidbody.AddTorque(a * d, 5);
		}

		// Token: 0x04001256 RID: 4694
		public Rigidbody Rigidbody;

		// Token: 0x04001257 RID: 4695
		public Vector3 TargetRotation;

		// Token: 0x04001258 RID: 4696
		public float RotationForce = 1f;

		// Token: 0x04001259 RID: 4697
		public Transform Bitch;
	}
}
