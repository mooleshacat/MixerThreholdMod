using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Lighting;
using ScheduleOne.Misc;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF9 RID: 3065
	public class GrowLight : ProceduralGridItem
	{
		// Token: 0x06005235 RID: 21045 RVA: 0x0015B470 File Offset: 0x00159670
		public override void InitializeProceduralGridItem(ItemInstance instance, int _rotation, List<CoordinateProceduralTilePair> _footprintTileMatches, string GUID)
		{
			base.InitializeProceduralGridItem(instance, _rotation, _footprintTileMatches, GUID);
			if (!this.isGhost)
			{
				this.SetIsOn(true);
				foreach (CoordinateProceduralTilePair coordinateProceduralTilePair in base.SyncAccessor_footprintTileMatches)
				{
					if (coordinateProceduralTilePair.tile.MatchedFootprintTile != null)
					{
						coordinateProceduralTilePair.tile.MatchedFootprintTile.MatchedStandardTile.LightExposureNode.AddSource(this.usableLightSource, 1f);
					}
				}
			}
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x0015B510 File Offset: 0x00159710
		public void SetIsOn(bool isOn)
		{
			this.usableLightSource.isEmitting = isOn;
			this.Light.isOn = isOn;
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x0015B52C File Offset: 0x0015972C
		public override void DestroyItem(bool callOnServer = true)
		{
			foreach (CoordinateProceduralTilePair coordinateProceduralTilePair in base.SyncAccessor_footprintTileMatches)
			{
				if (coordinateProceduralTilePair.tile.MatchedFootprintTile != null)
				{
					coordinateProceduralTilePair.tile.MatchedFootprintTile.MatchedStandardTile.LightExposureNode.RemoveSource(this.usableLightSource);
				}
			}
			base.DestroyItem(callOnServer);
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x0015B5BC File Offset: 0x001597BC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x0015B5D5 File Offset: 0x001597D5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.GrowLightAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x0015B5EE File Offset: 0x001597EE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x0015B5FC File Offset: 0x001597FC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003D9D RID: 15773
		[Header("References")]
		public ToggleableLight Light;

		// Token: 0x04003D9E RID: 15774
		public UsableLightSource usableLightSource;

		// Token: 0x04003D9F RID: 15775
		private bool dll_Excuted;

		// Token: 0x04003DA0 RID: 15776
		private bool dll_Excuted;
	}
}
