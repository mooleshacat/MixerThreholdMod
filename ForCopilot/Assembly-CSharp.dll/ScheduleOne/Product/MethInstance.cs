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
	// Token: 0x02000924 RID: 2340
	[Serializable]
	public class MethInstance : ProductItemInstance
	{
		// Token: 0x06003F00 RID: 16128 RVA: 0x00108557 File Offset: 0x00106757
		public MethInstance()
		{
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0010855F File Offset: 0x0010675F
		public MethInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition packaging = null) : base(definition, quantity, quality, packaging)
		{
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x00108C74 File Offset: 0x00106E74
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new MethInstance(base.Definition, quantity, this.Quality, base.AppliedPackaging);
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00108CA8 File Offset: 0x00106EA8
		public override void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			base.SetupPackagingVisuals(visuals);
			if (visuals == null)
			{
				Console.LogError("MethInstance: visuals is null!", null);
				return;
			}
			MethDefinition methDefinition = base.Definition as MethDefinition;
			if (methDefinition == null)
			{
				string str = "MethInstance: definition is null! Type: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return;
			}
			MeshRenderer[] crystalMeshes = visuals.methVisuals.CrystalMeshes;
			for (int i = 0; i < crystalMeshes.Length; i++)
			{
				crystalMeshes[i].material = methDefinition.CrystalMaterial;
			}
			visuals.methVisuals.Container.gameObject.SetActive(true);
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x00108D47 File Offset: 0x00106F47
		public override ItemData GetItemData()
		{
			return new MethData(base.Definition.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x00108D78 File Offset: 0x00106F78
		public override void ApplyEffectsToNPC(NPC npc)
		{
			Console.Log("Applying meth effects to NPC: " + npc.fullName, null);
			npc.Avatar.EmotionManager.AddEmotionOverride("Meth", this.Name, 0f, 0);
			npc.Avatar.Eyes.OverrideEyeballTint(new Color32(165, 112, 86, byte.MaxValue));
			npc.Avatar.Eyes.OverrideEyeLids(new Eye.EyeLidConfiguration
			{
				bottomLidOpen = 0.5f,
				topLidOpen = 0.1f
			});
			npc.Avatar.Eyes.SetPupilDilation(0.1f, false);
			npc.Avatar.Eyes.ForceBlink();
			npc.OverrideAggression(1f);
			base.ApplyEffectsToNPC(npc);
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00108E50 File Offset: 0x00107050
		public override void ClearEffectsFromNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			npc.Avatar.Eyes.ResetEyeballTint();
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ResetPupilDilation();
			npc.Avatar.Eyes.ForceBlink();
			npc.ResetAggression();
			base.ClearEffectsFromNPC(npc);
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00108EC0 File Offset: 0x001070C0
		public override void ApplyEffectsToPlayer(Player player)
		{
			player.Avatar.EmotionManager.AddEmotionOverride("Meth", this.Name, 0f, 0);
			player.Avatar.Eyes.OverrideEyeballTint(new Color32(165, 112, 86, byte.MaxValue));
			player.Avatar.Eyes.SetPupilDilation(0.1f, false);
			player.Avatar.Eyes.ForceBlink();
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.MethVisuals = true;
				Singleton<PostProcessingManager>.Instance.ColorFilterController.AddOverride((this.definition as MethDefinition).TintColor, 1, "Meth");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
			}
			base.ApplyEffectsToPlayer(player);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00108F9C File Offset: 0x0010719C
		public override void ClearEffectsFromPlayer(Player Player)
		{
			Player.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			Player.Avatar.Eyes.ResetEyeballTint();
			Player.Avatar.Eyes.ResetEyeLids();
			Player.Avatar.Eyes.ResetPupilDilation();
			Player.Avatar.Eyes.ForceBlink();
			if (Player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.MethVisuals = false;
				Singleton<PostProcessingManager>.Instance.ColorFilterController.RemoveOverride("Meth");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
			}
			base.ClearEffectsFromPlayer(Player);
		}
	}
}
