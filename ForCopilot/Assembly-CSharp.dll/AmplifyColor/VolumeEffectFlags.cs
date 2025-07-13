using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000CAE RID: 3246
	[Serializable]
	public class VolumeEffectFlags
	{
		// Token: 0x06005B03 RID: 23299 RVA: 0x0017F990 File Offset: 0x0017DB90
		public VolumeEffectFlags()
		{
			this.components = new List<VolumeEffectComponentFlags>();
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x0017F9A4 File Offset: 0x0017DBA4
		public void AddComponent(Component c)
		{
			VolumeEffectComponentFlags volumeEffectComponentFlags;
			if ((volumeEffectComponentFlags = this.components.Find(delegate(VolumeEffectComponentFlags s)
			{
				string componentName = s.componentName;
				Type type = c.GetType();
				return componentName == (((type != null) ? type.ToString() : null) ?? "");
			})) != null)
			{
				volumeEffectComponentFlags.UpdateComponentFlags(c);
				return;
			}
			this.components.Add(new VolumeEffectComponentFlags(c));
		}

		// Token: 0x06005B05 RID: 23301 RVA: 0x0017F9FC File Offset: 0x0017DBFC
		public void UpdateFlags(VolumeEffect effectVol)
		{
			using (List<VolumeEffectComponent>.Enumerator enumerator = effectVol.components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectComponent comp = enumerator.Current;
					VolumeEffectComponentFlags volumeEffectComponentFlags;
					if ((volumeEffectComponentFlags = this.components.Find((VolumeEffectComponentFlags s) => s.componentName == comp.componentName)) == null)
					{
						this.components.Add(new VolumeEffectComponentFlags(comp));
					}
					else
					{
						volumeEffectComponentFlags.UpdateComponentFlags(comp);
					}
				}
			}
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x0017FA94 File Offset: 0x0017DC94
		public static void UpdateCamFlags(AmplifyColorEffect[] effects, AmplifyColorVolumeBase[] volumes)
		{
			foreach (AmplifyColorEffect amplifyColorEffect in effects)
			{
				amplifyColorEffect.EffectFlags = new VolumeEffectFlags();
				for (int j = 0; j < volumes.Length; j++)
				{
					VolumeEffect volumeEffect = volumes[j].EffectContainer.FindVolumeEffect(amplifyColorEffect);
					if (volumeEffect != null)
					{
						amplifyColorEffect.EffectFlags.UpdateFlags(volumeEffect);
					}
				}
			}
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x0017FAF8 File Offset: 0x0017DCF8
		public VolumeEffect GenerateEffectData(AmplifyColorEffect go)
		{
			VolumeEffect volumeEffect = new VolumeEffect(go);
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in this.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = go.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						volumeEffect.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
			return volumeEffect;
		}

		// Token: 0x06005B08 RID: 23304 RVA: 0x0017FB74 File Offset: 0x0017DD74
		public VolumeEffectComponentFlags FindComponentFlags(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x0017FBC0 File Offset: 0x0017DDC0
		public string[] GetComponentNames()
		{
			return (from r in this.components
			where r.blendFlag
			select r.componentName).ToArray<string>();
		}

		// Token: 0x040042CE RID: 17102
		public List<VolumeEffectComponentFlags> components;
	}
}
