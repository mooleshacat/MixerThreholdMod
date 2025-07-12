using System;
using FishNet.Connection;
using FishNet.Object;
using ScheduleOne.Property;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x020005B6 RID: 1462
	public interface IConfigurable
	{
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060023F6 RID: 9206
		EntityConfiguration Configuration { get; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060023F7 RID: 9207
		ConfigurationReplicator ConfigReplicator { get; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060023F8 RID: 9208
		EConfigurableType ConfigurableType { get; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060023F9 RID: 9209
		// (set) Token: 0x060023FA RID: 9210
		WorldspaceUIElement WorldspaceUI { get; set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060023FB RID: 9211
		// (set) Token: 0x060023FC RID: 9212
		NetworkObject CurrentPlayerConfigurer { get; set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x00094321 File Offset: 0x00092521
		bool IsBeingConfiguredByOtherPlayer
		{
			get
			{
				return this.CurrentPlayerConfigurer != null && !this.CurrentPlayerConfigurer.IsOwner;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060023FE RID: 9214
		Sprite TypeIcon { get; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060023FF RID: 9215
		Transform Transform { get; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002400 RID: 9216
		Transform UIPoint { get; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x00094341 File Offset: 0x00092541
		bool IsDestroyed
		{
			get
			{
				return this == null || this.Transform == null;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002402 RID: 9218
		bool CanBeSelected { get; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002403 RID: 9219
		Property ParentProperty { get; }

		// Token: 0x06002404 RID: 9220
		WorldspaceUIElement CreateWorldspaceUI();

		// Token: 0x06002405 RID: 9221
		void DestroyWorldspaceUI();

		// Token: 0x06002406 RID: 9222
		void ShowOutline(Color color);

		// Token: 0x06002407 RID: 9223
		void HideOutline();

		// Token: 0x06002408 RID: 9224 RVA: 0x00094354 File Offset: 0x00092554
		void Selected()
		{
			this.Configuration.Selected();
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x00094361 File Offset: 0x00092561
		void Deselected()
		{
			this.Configuration.Deselected();
		}

		// Token: 0x0600240A RID: 9226
		void SetConfigurer(NetworkObject player);

		// Token: 0x0600240B RID: 9227
		void SendConfigurationToClient(NetworkConnection conn);
	}
}
