using System;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x020008B9 RID: 2233
	[ExecuteInEditMode]
	public class WheelRotator : MonoBehaviour
	{
		// Token: 0x06003C5F RID: 15455 RVA: 0x000FE711 File Offset: 0x000FC911
		private void Start()
		{
			if (this.Controller != null)
			{
				this.Controller.AudioSource.time = UnityEngine.Random.Range(0f, this.Controller.AudioSource.clip.length);
			}
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x000FE750 File Offset: 0x000FC950
		private void LateUpdate()
		{
			Vector3 position = base.transform.position;
			float num = Vector3.Distance(position, this.lastFramePosition);
			if (num > 0f)
			{
				float num2 = num / (6.2831855f * this.Radius) * 360f;
				this.Wheel.Rotate(this.RotationAxis, num2 * (this.Flip ? -1f : 1f));
				float num3 = num2 / Time.deltaTime;
				if (this.Controller != null)
				{
					this.Controller.VolumeMultiplier = num3 / this.AudioVolumeDivisor;
				}
			}
			this.lastFramePosition = position;
		}

		// Token: 0x04002B25 RID: 11045
		public float Radius = 0.5f;

		// Token: 0x04002B26 RID: 11046
		public Transform Wheel;

		// Token: 0x04002B27 RID: 11047
		public bool Flip;

		// Token: 0x04002B28 RID: 11048
		public AudioSourceController Controller;

		// Token: 0x04002B29 RID: 11049
		public float AudioVolumeDivisor = 90f;

		// Token: 0x04002B2A RID: 11050
		public Vector3 RotationAxis = Vector3.up;

		// Token: 0x04002B2B RID: 11051
		[SerializeField]
		private Vector3 lastFramePosition = Vector3.zero;
	}
}
