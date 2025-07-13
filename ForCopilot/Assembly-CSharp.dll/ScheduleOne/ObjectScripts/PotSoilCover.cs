using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C04 RID: 3076
	public class PotSoilCover : MonoBehaviour
	{
		// Token: 0x060052FA RID: 21242 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0015EB5B File Offset: 0x0015CD5B
		private void OnEnable()
		{
			base.StartCoroutine(this.CheckQueue());
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0015EB6A File Offset: 0x0015CD6A
		public void ConfigureAppearance(Color col, float transparency)
		{
			this.MeshRenderer.material.SetColor("_MainColor", col);
			this.MeshRenderer.material.SetFloat("_Transparency", transparency);
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x0015EB98 File Offset: 0x0015CD98
		public void Reset()
		{
			this.Blank();
			this.CurrentCoverage = 0.215f;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0015EBAB File Offset: 0x0015CDAB
		public void QueuePour(Vector3 worldSpacePosition)
		{
			this.queued = true;
			this.queuedWorldPos = worldSpacePosition;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x0015EBBB File Offset: 0x0015CDBB
		public float GetNormalizedProgress()
		{
			return (this.CurrentCoverage - 0.215f) / 0.735f;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x0015EBCF File Offset: 0x0015CDCF
		private IEnumerator CheckQueue()
		{
			while (base.gameObject != null)
			{
				if (this.queued)
				{
					this.queued = false;
					this.DelayedApplyPour(this.queuedWorldPos);
				}
				yield return new WaitForSeconds(0.041666668f);
			}
			yield break;
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x0015EBE0 File Offset: 0x0015CDE0
		private void Blank()
		{
			Texture2D texture2D = new Texture2D(128, 128);
			Color[] array = new Color[16384];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.black;
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			this.MeshRenderer.material.mainTexture = texture2D;
			this.mainTex = texture2D;
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x0015EC48 File Offset: 0x0015CE48
		private void DelayedApplyPour(Vector3 worldSpace)
		{
			PotSoilCover.<>c__DisplayClass27_0 CS$<>8__locals1 = new PotSoilCover.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.worldSpace = worldSpace;
			base.StartCoroutine(CS$<>8__locals1.<DelayedApplyPour>g__Routine|0());
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x0015EC78 File Offset: 0x0015CE78
		private void ApplyPour(Vector3 worldSpace)
		{
			this.relative = base.transform.InverseTransformPoint(worldSpace);
			this.vector2 = new Vector2(this.relative.x, this.relative.z);
			if (this.vector2.magnitude > this.Radius)
			{
				return;
			}
			this.normalizedOffset = new Vector2(this.vector2.x / this.Radius, this.vector2.y / this.Radius);
			this.originPixel = new Vector2(64f * (1f + this.normalizedOffset.x), 64f * (1f + this.normalizedOffset.y));
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					int num = (int)this.originPixel.x - 32 + i;
					int num2 = (int)this.originPixel.y - 32 + j;
					if (num >= 0 && num < 128 && num2 >= 0 && num2 < 128)
					{
						Color pixel = this.mainTex.GetPixel(num, num2);
						pixel.r += this.GetPourMaskValue(i, j);
						pixel.g = pixel.r;
						pixel.b = pixel.r;
						pixel.a = 1f;
						this.mainTex.SetPixel(num, num2, pixel);
					}
				}
			}
			this.mainTex.Apply();
			float currentCoverage = this.CurrentCoverage;
			float coverage = this.GetCoverage();
			this.CurrentCoverage = coverage;
			if (coverage >= 0.95f && currentCoverage < 0.95f && this.onSufficientCoverage != null)
			{
				this.onSufficientCoverage.Invoke();
			}
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x0015EE40 File Offset: 0x0015D040
		private float GetPourMaskValue(int x, int y)
		{
			return this.PourMask.GetPixel(x, y).grayscale;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x0015EE64 File Offset: 0x0015D064
		private float GetCoverage()
		{
			int num = 16384;
			int num2 = 0;
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					if (this.mainTex.GetPixel(i, j).r > 0.5f)
					{
						num2++;
					}
				}
			}
			return Mathf.Clamp01((float)num2 / (float)num + 0.215f);
		}

		// Token: 0x04003E0A RID: 15882
		public const int TEXTURE_SIZE = 128;

		// Token: 0x04003E0B RID: 15883
		public const int POUR_RADIUS = 32;

		// Token: 0x04003E0C RID: 15884
		public const int UPDATES_PER_SECOND = 24;

		// Token: 0x04003E0D RID: 15885
		public const float COVERAGE_THRESHOLD = 0.5f;

		// Token: 0x04003E0E RID: 15886
		public const float BASE_COVERAGE = 0.215f;

		// Token: 0x04003E0F RID: 15887
		public const float SUCCESS_COVERAGE_THRESHOLD = 0.95f;

		// Token: 0x04003E10 RID: 15888
		public const float DELAY = 0.35f;

		// Token: 0x04003E11 RID: 15889
		public float CurrentCoverage;

		// Token: 0x04003E12 RID: 15890
		[Header("Settings")]
		public float Radius;

		// Token: 0x04003E13 RID: 15891
		[Header("References")]
		public MeshRenderer MeshRenderer;

		// Token: 0x04003E14 RID: 15892
		public Texture2D PourMask;

		// Token: 0x04003E15 RID: 15893
		public UnityEvent onSufficientCoverage;

		// Token: 0x04003E16 RID: 15894
		private bool queued;

		// Token: 0x04003E17 RID: 15895
		private Vector3 queuedWorldPos = Vector3.zero;

		// Token: 0x04003E18 RID: 15896
		private Texture2D mainTex;

		// Token: 0x04003E19 RID: 15897
		private Vector3 relative;

		// Token: 0x04003E1A RID: 15898
		private Vector2 vector2;

		// Token: 0x04003E1B RID: 15899
		private Vector2 normalizedOffset;

		// Token: 0x04003E1C RID: 15900
		private Vector2 originPixel;
	}
}
