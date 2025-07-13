using System;
using ScheduleOne.Audio;
using ScheduleOne.AvatarFramework;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200094C RID: 2380
	[Serializable]
	public class WeedInstance : ProductItemInstance
	{
		// Token: 0x06004043 RID: 16451 RVA: 0x00108557 File Offset: 0x00106757
		public WeedInstance()
		{
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x0010855F File Offset: 0x0010675F
		public WeedInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition packaging = null) : base(definition, quantity, quality, packaging)
		{
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0010FA28 File Offset: 0x0010DC28
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new WeedInstance(base.Definition, quantity, this.Quality, base.AppliedPackaging);
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0010FA5C File Offset: 0x0010DC5C
		public override void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			base.SetupPackagingVisuals(visuals);
			if (visuals == null)
			{
				Console.LogError("WeedInstance: visuals is null!", null);
				return;
			}
			WeedDefinition weedDefinition = base.Definition as WeedDefinition;
			if (weedDefinition == null)
			{
				string str = "WeedInstance: definition is null! Type: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair in visuals.weedVisuals.MainMeshes)
			{
				Material[] materials = meshIndexPair.Mesh.materials;
				materials[meshIndexPair.MaterialIndex] = weedDefinition.MainMat;
				meshIndexPair.Mesh.materials = materials;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair2 in visuals.weedVisuals.SecondaryMeshes)
			{
				Material[] materials2 = meshIndexPair2.Mesh.materials;
				materials2[meshIndexPair2.MaterialIndex] = weedDefinition.SecondaryMat;
				meshIndexPair2.Mesh.materials = materials2;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair3 in visuals.weedVisuals.LeafMeshes)
			{
				Material[] materials3 = meshIndexPair3.Mesh.materials;
				materials3[meshIndexPair3.MaterialIndex] = weedDefinition.LeafMat;
				meshIndexPair3.Mesh.materials = materials3;
			}
			foreach (FilledPackagingVisuals.MeshIndexPair meshIndexPair4 in visuals.weedVisuals.StemMeshes)
			{
				Material[] materials4 = meshIndexPair4.Mesh.materials;
				materials4[meshIndexPair4.MaterialIndex] = weedDefinition.StemMat;
				meshIndexPair4.Mesh.materials = materials4;
			}
			visuals.weedVisuals.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0010FBFB File Offset: 0x0010DDFB
		public override ItemData GetItemData()
		{
			return new WeedData(base.Definition.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0010FC2C File Offset: 0x0010DE2C
		public override void ApplyEffectsToNPC(NPC npc)
		{
			npc.Avatar.Eyes.OverrideEyeballTint(new Color32(byte.MaxValue, 170, 170, byte.MaxValue));
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.3f,
				topLidOpen = 0.3f
			});
			npc.Avatar.Eyes.ForceBlink();
			base.ApplyEffectsToNPC(npc);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0010FCAF File Offset: 0x0010DEAF
		public override void ClearEffectsFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetEyeballTint();
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ForceBlink();
			base.ClearEffectsFromNPC(npc);
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0010FCE8 File Offset: 0x0010DEE8
		public override void ApplyEffectsToPlayer(Player player)
		{
			player.Avatar.Eyes.OverrideEyeballTint(new Color32(byte.MaxValue, 170, 170, byte.MaxValue));
			player.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.3f,
				topLidOpen = 0.3f
			});
			if (player.IsOwner)
			{
				Singleton<PostProcessingManager>.Instance.ChromaticAberrationController.AddOverride(0.2f, 5, "weed");
				Singleton<PostProcessingManager>.Instance.SaturationController.AddOverride(70f, 5, "weed");
				Singleton<PostProcessingManager>.Instance.BloomController.AddOverride(3f, 5, "weed");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
			}
			base.ApplyEffectsToPlayer(player);
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0010FDD4 File Offset: 0x0010DFD4
		public override void ClearEffectsFromPlayer(Player Player)
		{
			Player.Avatar.Eyes.ResetEyeballTint();
			Player.Avatar.Eyes.ResetEyeLids();
			Player.Avatar.Eyes.ForceBlink();
			if (Player.IsOwner)
			{
				Singleton<PostProcessingManager>.Instance.ChromaticAberrationController.RemoveOverride("weed");
				Singleton<PostProcessingManager>.Instance.SaturationController.RemoveOverride("weed");
				Singleton<PostProcessingManager>.Instance.BloomController.RemoveOverride("weed");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
			}
			base.ClearEffectsFromPlayer(Player);
		}
	}
}
