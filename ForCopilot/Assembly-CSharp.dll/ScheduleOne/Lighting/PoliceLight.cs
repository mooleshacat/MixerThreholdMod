using System;
using System.Collections;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x020005E1 RID: 1505
	public class PoliceLight : MonoBehaviour
	{
		// Token: 0x060024EB RID: 9451 RVA: 0x00096517 File Offset: 0x00094717
		public void SetIsOn(bool isOn)
		{
			this.IsOn = isOn;
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x00096520 File Offset: 0x00094720
		private void FixedUpdate()
		{
			if (this.IsOn)
			{
				if (!this.Siren.isPlaying)
				{
					this.Siren.Play();
				}
				if (this.cycleRoutine == null)
				{
					this.cycleRoutine = base.StartCoroutine(this.CycleCoroutine());
					return;
				}
			}
			else if (this.Siren.isPlaying)
			{
				this.Siren.Stop();
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x00096580 File Offset: 0x00094780
		protected IEnumerator CycleCoroutine()
		{
			foreach (OptimizedLight optimizedLight in this.RedLights)
			{
				optimizedLight._Light.intensity = 0f;
				optimizedLight.Enabled = true;
			}
			foreach (OptimizedLight optimizedLight2 in this.BlueLights)
			{
				optimizedLight2._Light.intensity = 0f;
				optimizedLight2.Enabled = true;
			}
			float time = 0f;
			MeshRenderer[] array2;
			while (this.IsOn)
			{
				time += Time.deltaTime;
				float time2 = time / this.CycleDuration % 1f;
				float num = this.RedBrightnessCurve.Evaluate(time2);
				float num2 = this.BlueBrightnessCurve.Evaluate(time2);
				OptimizedLight[] array = this.RedLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]._Light.intensity = num * this.LightBrightness;
				}
				array2 = this.RedMeshes;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].material = ((num > 0f) ? this.RedOnMat : this.RedOffMat);
				}
				array = this.BlueLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]._Light.intensity = num2 * this.LightBrightness;
				}
				array2 = this.BlueMeshes;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].material = ((num2 > 0f) ? this.BlueOnMat : this.BlueOffMat);
				}
				yield return new WaitForEndOfFrame();
			}
			foreach (OptimizedLight optimizedLight3 in this.RedLights)
			{
				optimizedLight3._Light.intensity = 0f;
				optimizedLight3.Enabled = false;
			}
			array2 = this.RedMeshes;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].material = this.RedOffMat;
			}
			foreach (OptimizedLight optimizedLight4 in this.BlueLights)
			{
				optimizedLight4._Light.intensity = 0f;
				optimizedLight4.Enabled = false;
			}
			array2 = this.BlueMeshes;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].material = this.BlueOffMat;
			}
			this.cycleRoutine = null;
			yield break;
		}

		// Token: 0x04001B48 RID: 6984
		public bool IsOn;

		// Token: 0x04001B49 RID: 6985
		[Header("References")]
		public MeshRenderer[] RedMeshes;

		// Token: 0x04001B4A RID: 6986
		public MeshRenderer[] BlueMeshes;

		// Token: 0x04001B4B RID: 6987
		public OptimizedLight[] RedLights;

		// Token: 0x04001B4C RID: 6988
		public OptimizedLight[] BlueLights;

		// Token: 0x04001B4D RID: 6989
		public AudioSourceController Siren;

		// Token: 0x04001B4E RID: 6990
		[Header("Settings")]
		public float CycleDuration = 0.5f;

		// Token: 0x04001B4F RID: 6991
		public Material RedOffMat;

		// Token: 0x04001B50 RID: 6992
		public Material RedOnMat;

		// Token: 0x04001B51 RID: 6993
		public Material BlueOffMat;

		// Token: 0x04001B52 RID: 6994
		public Material BlueOnMat;

		// Token: 0x04001B53 RID: 6995
		public AnimationCurve RedBrightnessCurve;

		// Token: 0x04001B54 RID: 6996
		public AnimationCurve BlueBrightnessCurve;

		// Token: 0x04001B55 RID: 6997
		public float LightBrightness = 5f;

		// Token: 0x04001B56 RID: 6998
		private Coroutine cycleRoutine;
	}
}
