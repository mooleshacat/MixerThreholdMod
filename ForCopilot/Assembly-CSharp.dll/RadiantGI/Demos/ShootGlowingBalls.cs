using System;
using UnityEngine;

namespace RadiantGI.Demos
{
	// Token: 0x02000176 RID: 374
	public class ShootGlowingBalls : MonoBehaviour
	{
		// Token: 0x0600072F RID: 1839 RVA: 0x00020518 File Offset: 0x0001E718
		private void Start()
		{
			for (int i = 0; i < this.count; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.glowingBall, this.center.position + Vector3.right * (float)UnityEngine.Random.Range(-4, 4) + Vector3.up * (5f + (float)i), Quaternion.identity);
				Color color = UnityEngine.Random.ColorHSV();
				float value = UnityEngine.Random.value;
				if (value < 0.33f)
				{
					color.r *= 0.2f;
				}
				else if (value < 0.66f)
				{
					color.g *= 0.2f;
				}
				else
				{
					color.b *= 0.2f;
				}
				Renderer component = gameObject.GetComponent<Renderer>();
				component.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.65f, 1f);
				component.material.color = color;
				component.material.SetColor("_EmissionColor", color * 2f);
			}
		}

		// Token: 0x040007E7 RID: 2023
		public int count;

		// Token: 0x040007E8 RID: 2024
		public Transform center;

		// Token: 0x040007E9 RID: 2025
		public GameObject glowingBall;
	}
}
