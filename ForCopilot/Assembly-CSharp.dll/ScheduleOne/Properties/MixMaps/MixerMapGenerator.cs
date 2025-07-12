using System;
using System.Linq;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200033D RID: 829
	public class MixerMapGenerator : MonoBehaviour
	{
		// Token: 0x0600123A RID: 4666 RVA: 0x0004EF34 File Offset: 0x0004D134
		private void OnValidate()
		{
			this.BasePlateMesh.localScale = Vector3.one * this.MapRadius * 2f * 0.01f;
			base.gameObject.name = this.MapName;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0004EF84 File Offset: 0x0004D184
		[Button]
		public void CreateEffectPrefabs()
		{
			foreach (Property property in Resources.LoadAll<Property>("Properties"))
			{
				if (this.GetEffect(property) == null)
				{
					Effect effect = UnityEngine.Object.Instantiate<Effect>(this.EffectPrefab, base.transform);
					effect.Property = property;
					effect.Radius = 0.5f;
					effect.transform.position = new Vector3(UnityEngine.Random.Range(-this.MapRadius, this.MapRadius), 0.1f, UnityEngine.Random.Range(-this.MapRadius, this.MapRadius));
				}
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0004F018 File Offset: 0x0004D218
		[Button]
		public Effect GetEffect(Property property)
		{
			return base.GetComponentsInChildren<Effect>().FirstOrDefault((Effect effect) => effect.Property == property);
		}

		// Token: 0x04001192 RID: 4498
		public float MapRadius = 5f;

		// Token: 0x04001193 RID: 4499
		public string MapName = "New Map";

		// Token: 0x04001194 RID: 4500
		public Transform BasePlateMesh;

		// Token: 0x04001195 RID: 4501
		public Effect EffectPrefab;
	}
}
