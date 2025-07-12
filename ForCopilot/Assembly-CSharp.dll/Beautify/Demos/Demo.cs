using System;
using Beautify.Universal;
using UnityEngine;
using UnityEngine.UI;

namespace Beautify.Demos
{
	// Token: 0x020001FA RID: 506
	public class Demo : MonoBehaviour
	{
		// Token: 0x06000B34 RID: 2868 RVA: 0x00030F53 File Offset: 0x0002F153
		private void Start()
		{
			this.UpdateText();
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00030F5C File Offset: 0x0002F15C
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.J))
			{
				BeautifySettings.settings.bloomIntensity.value += 0.1f;
			}
			if (Input.GetKeyDown(KeyCode.T) || Input.GetMouseButtonDown(0))
			{
				BeautifySettings.settings.disabled.value = !BeautifySettings.settings.disabled.value;
				this.UpdateText();
			}
			if (Input.GetKeyDown(KeyCode.B))
			{
				BeautifySettings.Blink(0.2f, 1f);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				BeautifySettings.settings.compareMode.value = !BeautifySettings.settings.compareMode.value;
			}
			if (Input.GetKeyDown(KeyCode.N))
			{
				BeautifySettings.settings.nightVision.Override(!BeautifySettings.settings.nightVision.value);
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				if (BeautifySettings.settings.blurIntensity.overrideState)
				{
					BeautifySettings.settings.blurIntensity.overrideState = false;
				}
				else
				{
					BeautifySettings.settings.blurIntensity.Override(4f);
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				BeautifySettings.settings.brightness.Override(0.1f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				BeautifySettings.settings.brightness.Override(0.5f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				BeautifySettings.settings.brightness.overrideState = false;
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				BeautifySettings.settings.outline.Override(true);
				BeautifySettings.settings.outlineColor.Override(Color.cyan);
				BeautifySettings.settings.outlineCustomize.Override(true);
				BeautifySettings.settings.outlineSpread.Override(1.5f);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				BeautifySettings.settings.outline.overrideState = false;
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				BeautifySettings.settings.lut.Override(true);
				BeautifySettings.settings.lutIntensity.Override(1f);
				BeautifySettings.settings.lutTexture.Override(this.lutTexture);
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				BeautifySettings.settings.lut.Override(false);
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0003118C File Offset: 0x0002F38C
		private void UpdateText()
		{
			if (BeautifySettings.settings.disabled.value)
			{
				GameObject.Find("Beautify").GetComponent<Text>().text = "Beautify OFF";
				return;
			}
			GameObject.Find("Beautify").GetComponent<Text>().text = "Beautify ON";
		}

		// Token: 0x04000BDD RID: 3037
		public Texture lutTexture;
	}
}
