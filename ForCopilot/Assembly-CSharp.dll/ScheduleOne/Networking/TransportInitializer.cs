using System;
using FishNet.Managing.Transporting;
using FishNet.Transporting.Multipass;
using FishNet.Transporting.Yak;
using UnityEngine;

namespace ScheduleOne.Networking
{
	// Token: 0x0200057A RID: 1402
	public class TransportInitializer : MonoBehaviour
	{
		// Token: 0x060021CF RID: 8655 RVA: 0x0008B6E6 File Offset: 0x000898E6
		public void Awake()
		{
			base.GetComponent<TransportManager>().GetTransport<Multipass>().SetClientTransport<Yak>();
		}
	}
}
