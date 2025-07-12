using System;
using UnityEngine;
using UnityEngine.UI;

namespace Funly.SkyStudio
{
	// Token: 0x020001DC RID: 476
	[RequireComponent(typeof(RawImage))]
	public class LoadOverheadDepthTexture : MonoBehaviour
	{
		// Token: 0x06000A75 RID: 2677 RVA: 0x0002E4C3 File Offset: 0x0002C6C3
		private void Start()
		{
			this.m_RainCamera = UnityEngine.Object.FindObjectOfType<WeatherDepthCamera>();
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x04000B59 RID: 2905
		private WeatherDepthCamera m_RainCamera;
	}
}
