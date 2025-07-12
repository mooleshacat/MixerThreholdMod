using System;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005CB RID: 1483
	public class TransitRouteMaterial : MonoBehaviour
	{
		// Token: 0x06002484 RID: 9348 RVA: 0x000956DF File Offset: 0x000938DF
		private void Awake()
		{
			Material material = base.GetComponent<MeshRenderer>().material;
			material.SetInt("unity_GUIZTestMode", 8);
			material.renderQueue = 3000;
		}
	}
}
