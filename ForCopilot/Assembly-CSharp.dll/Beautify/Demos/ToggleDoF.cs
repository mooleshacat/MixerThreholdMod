using System;
using Beautify.Universal;
using UnityEngine;

namespace Beautify.Demos
{
	// Token: 0x020001FB RID: 507
	public class ToggleDoF : MonoBehaviour
	{
		// Token: 0x06000B38 RID: 2872 RVA: 0x000311E0 File Offset: 0x0002F3E0
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				bool value = BeautifySettings.settings.depthOfField.value;
				BeautifySettings.settings.depthOfField.Override(!value);
			}
		}
	}
}
