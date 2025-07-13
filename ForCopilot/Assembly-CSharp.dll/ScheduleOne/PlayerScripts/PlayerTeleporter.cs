using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x0200063B RID: 1595
	public class PlayerTeleporter : MonoBehaviour
	{
		// Token: 0x06002942 RID: 10562 RVA: 0x000AA294 File Offset: 0x000A8494
		public void Teleport(Transform destination)
		{
			PlayerSingleton<PlayerMovement>.Instance.Teleport(destination.position);
			Player.Local.transform.rotation = destination.rotation;
			Player.Local.transform.eulerAngles = new Vector3(0f, Player.Local.transform.eulerAngles.y, 0f);
		}
	}
}
