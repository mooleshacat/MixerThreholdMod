using System;
using System.Collections.Generic;
using System.Linq;

namespace AmplifyColor
{
	// Token: 0x02000CA7 RID: 3239
	[Serializable]
	public class VolumeEffectContainer
	{
		// Token: 0x06005AEA RID: 23274 RVA: 0x0017F50D File Offset: 0x0017D70D
		public VolumeEffectContainer()
		{
			this.volumes = new List<VolumeEffect>();
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x0017F520 File Offset: 0x0017D720
		public void AddColorEffect(AmplifyColorEffect colorEffect)
		{
			VolumeEffect volumeEffect;
			if ((volumeEffect = this.FindVolumeEffect(colorEffect)) != null)
			{
				volumeEffect.UpdateVolume();
				return;
			}
			volumeEffect = new VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			volumeEffect.UpdateVolume();
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x0017F558 File Offset: 0x0017D758
		public VolumeEffect AddJustColorEffect(AmplifyColorEffect colorEffect)
		{
			VolumeEffect volumeEffect = new VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			return volumeEffect;
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x0017F57C File Offset: 0x0017D77C
		public VolumeEffect FindVolumeEffect(AmplifyColorEffect colorEffect)
		{
			for (int i = 0; i < this.volumes.Count; i++)
			{
				if (this.volumes[i].gameObject == colorEffect)
				{
					return this.volumes[i];
				}
			}
			for (int j = 0; j < this.volumes.Count; j++)
			{
				if (this.volumes[j].gameObject != null && this.volumes[j].gameObject.SharedInstanceID == colorEffect.SharedInstanceID)
				{
					return this.volumes[j];
				}
			}
			return null;
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x0017F625 File Offset: 0x0017D825
		public void RemoveVolumeEffect(VolumeEffect volume)
		{
			this.volumes.Remove(volume);
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x0017F634 File Offset: 0x0017D834
		public AmplifyColorEffect[] GetStoredEffects()
		{
			return (from r in this.volumes
			select r.gameObject).ToArray<AmplifyColorEffect>();
		}

		// Token: 0x040042C0 RID: 17088
		public List<VolumeEffect> volumes;
	}
}
