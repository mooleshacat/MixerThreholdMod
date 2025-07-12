using System;
using UnityEngine;
using UnityEngine.UI;

namespace VolumetricFogAndMist2.Demos
{
	// Token: 0x0200016B RID: 363
	public class DemoSceneControls : MonoBehaviour
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x0001F67D File Offset: 0x0001D87D
		private void Start()
		{
			this.SetProfile(this.index);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001F68C File Offset: 0x0001D88C
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				this.index++;
				if (this.index >= this.profiles.Length)
				{
					this.index = 0;
				}
				this.SetProfile(this.index);
			}
			if (Input.GetKeyDown(KeyCode.T))
			{
				this.fogVolume.gameObject.SetActive(!this.fogVolume.gameObject.activeSelf);
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001F700 File Offset: 0x0001D900
		private void SetProfile(int profileIndex)
		{
			if (profileIndex < 2)
			{
				this.fogVolume.transform.position = Vector3.up * 25f;
			}
			else
			{
				this.fogVolume.transform.position = Vector3.zero;
			}
			this.fogVolume.profile = this.profiles[profileIndex];
			this.presetNameDisplay.text = "Current fog preset: " + this.profiles[profileIndex].name;
			this.fogVolume.UpdateMaterialPropertiesNow(false, false);
		}

		// Token: 0x040007B2 RID: 1970
		public VolumetricFogProfile[] profiles;

		// Token: 0x040007B3 RID: 1971
		public VolumetricFog fogVolume;

		// Token: 0x040007B4 RID: 1972
		public Text presetNameDisplay;

		// Token: 0x040007B5 RID: 1973
		private int index;
	}
}
