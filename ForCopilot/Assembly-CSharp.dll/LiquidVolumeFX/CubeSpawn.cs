using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000179 RID: 377
	public class CubeSpawn : MonoBehaviour
	{
		// Token: 0x0600073C RID: 1852 RVA: 0x00020898 File Offset: 0x0001EA98
		private void Start()
		{
			for (int i = 1; i <= this.instances; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
				gameObject.GetComponent<CubeSpawn>().enabled = false;
				gameObject.name = "Cube" + i.ToString();
				float f = (float)i / (float)this.instances * 3.1415927f * 2f * this.laps;
				float num = (float)i * this.expansion;
				float x = Mathf.Cos(f) * (this.radius + num);
				float z = Mathf.Sin(f) * (this.radius + num);
				Vector3 b = UnityEngine.Random.insideUnitSphere * this.jitter;
				gameObject.transform.position = base.transform.position + new Vector3(x, 0f, z) + b;
				gameObject.transform.localScale *= 1f - UnityEngine.Random.value * this.jitter;
			}
		}

		// Token: 0x040007F1 RID: 2033
		public int instances = 150;

		// Token: 0x040007F2 RID: 2034
		public float radius = 2f;

		// Token: 0x040007F3 RID: 2035
		public float jitter = 0.5f;

		// Token: 0x040007F4 RID: 2036
		public float expansion = 0.04f;

		// Token: 0x040007F5 RID: 2037
		public float laps = 2f;
	}
}
