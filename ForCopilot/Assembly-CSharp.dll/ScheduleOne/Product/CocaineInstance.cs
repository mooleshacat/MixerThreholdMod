using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x0200091A RID: 2330
	[Serializable]
	public class CocaineInstance : ProductItemInstance
	{
		// Token: 0x06003EE3 RID: 16099 RVA: 0x00108557 File Offset: 0x00106757
		public CocaineInstance()
		{
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x0010855F File Offset: 0x0010675F
		public CocaineInstance(ItemDefinition definition, int quantity, EQuality quality, PackagingDefinition packaging = null) : base(definition, quantity, quality, packaging)
		{
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0010856C File Offset: 0x0010676C
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new CocaineInstance(base.Definition, quantity, this.Quality, base.AppliedPackaging);
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x001085A0 File Offset: 0x001067A0
		public override void SetupPackagingVisuals(FilledPackagingVisuals visuals)
		{
			base.SetupPackagingVisuals(visuals);
			if (visuals == null)
			{
				Console.LogError("CocaineInstance: visuals is null!", null);
				return;
			}
			CocaineDefinition cocaineDefinition = base.Definition as CocaineDefinition;
			if (cocaineDefinition == null)
			{
				string str = "CocaineInstance: definition is null! Type: ";
				ItemDefinition definition = base.Definition;
				Console.LogError(str + ((definition != null) ? definition.ToString() : null), null);
				return;
			}
			MeshRenderer[] rockMeshes = visuals.cocaineVisuals.RockMeshes;
			for (int i = 0; i < rockMeshes.Length; i++)
			{
				rockMeshes[i].material = cocaineDefinition.RockMaterial;
			}
			visuals.cocaineVisuals.Container.gameObject.SetActive(true);
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x0010863F File Offset: 0x0010683F
		public override ItemData GetItemData()
		{
			return new CocaineData(base.Definition.ID, this.Quantity, this.Quality.ToString(), this.PackagingID);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00108670 File Offset: 0x00106870
		public override void ApplyEffectsToNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.AddEmotionOverride("Cocaine", this.Name, 0f, 0);
			npc.Avatar.Eyes.OverrideEyeballTint(new Color32(200, 240, byte.MaxValue, byte.MaxValue));
			npc.Avatar.Eyes.SetPupilDilation(1f, false);
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.MoveSpeedMultiplier = 1.25f;
			npc.Avatar.LookController.LookLerpSpeed = 10f;
			base.ApplyEffectsToNPC(npc);
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x00108720 File Offset: 0x00106920
		public override void ClearEffectsFromNPC(NPC npc)
		{
			npc.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			npc.Avatar.Eyes.ResetEyeballTint();
			npc.Avatar.Eyes.ResetEyeLids();
			npc.Avatar.Eyes.ResetPupilDilation();
			npc.Avatar.Eyes.ForceBlink();
			npc.Movement.MoveSpeedMultiplier = 1f;
			npc.Avatar.LookController.LookLerpSpeed = 3f;
			base.ClearEffectsFromNPC(npc);
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x001087B0 File Offset: 0x001069B0
		public override void ApplyEffectsToPlayer(Player player)
		{
			player.Avatar.EmotionManager.AddEmotionOverride("Cocaine", this.Name, 0f, 0);
			player.Avatar.Eyes.OverrideEyeballTint(new Color32(200, 240, byte.MaxValue, byte.MaxValue));
			player.Avatar.Eyes.SetPupilDilation(1f, false);
			player.Avatar.Eyes.ForceBlink();
			player.Avatar.LookController.LookLerpSpeed = 10f;
			if (player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.CocaineVisuals = true;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.AddOverride(10f, 6, "Cocaine");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
			}
			base.ApplyEffectsToPlayer(player);
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x0010889C File Offset: 0x00106A9C
		public override void ClearEffectsFromPlayer(Player Player)
		{
			Player.Avatar.EmotionManager.RemoveEmotionOverride(this.Name);
			Player.Avatar.Eyes.ResetEyeballTint();
			Player.Avatar.Eyes.ResetEyeLids();
			Player.Avatar.Eyes.ResetPupilDilation();
			Player.Avatar.Eyes.ForceBlink();
			Player.Avatar.LookController.LookLerpSpeed = 3f;
			if (Player.IsOwner)
			{
				PlayerSingleton<PlayerCamera>.Instance.CocaineVisuals = false;
				PlayerSingleton<PlayerCamera>.Instance.FoVChangeSmoother.RemoveOverride("Cocaine");
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
			}
			base.ClearEffectsFromPlayer(Player);
		}
	}
}
