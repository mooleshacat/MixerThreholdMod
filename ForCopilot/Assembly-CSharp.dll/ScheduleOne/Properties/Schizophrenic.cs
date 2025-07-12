using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.FX;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Properties
{
	// Token: 0x02000327 RID: 807
	[CreateAssetMenu(fileName = "Schizophrenic", menuName = "Properties/Schizophrenic Property")]
	public class Schizophrenic : Property
	{
		// Token: 0x060011DD RID: 4573 RVA: 0x0004E124 File Offset: 0x0004C324
		public override void ApplyToNPC(NPC npc)
		{
			npc.Avatar.Eyes.SetPupilDilation(0.1f, false);
			npc.Avatar.EmotionManager.AddEmotionOverride("Scared", "Schizophrenic", 0f, this.Tier);
			npc.PlayVO(EVOLineType.Concerned);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004E174 File Offset: 0x0004C374
		public override void ApplyToPlayer(Player player)
		{
			player.Avatar.Eyes.SetPupilDilation(0.1f, false);
			player.Avatar.EmotionManager.AddEmotionOverride("Scared", "Schizophrenic", 0f, this.Tier);
			player.Schizophrenic = true;
			if (player.IsLocalPlayer)
			{
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(true, 5f);
				Singleton<MusicPlayer>.Instance.SetTrackEnabled("Schizo music", true);
				Singleton<AudioManager>.Instance.SetDistorted(true, 5f);
				Singleton<PostProcessingManager>.Instance.SaturationController.AddOverride(110f, 7, "Schizophrenic");
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.AddOverride(0.7f, 6, "sedating");
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0004E22F File Offset: 0x0004C42F
		public override void ClearFromNPC(NPC npc)
		{
			npc.Avatar.Eyes.ResetPupilDilation();
			npc.Avatar.EmotionManager.RemoveEmotionOverride("Schizophrenic");
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0004E258 File Offset: 0x0004C458
		public override void ClearFromPlayer(Player player)
		{
			player.Avatar.Eyes.ResetPupilDilation();
			player.Avatar.EmotionManager.RemoveEmotionOverride("Schizophrenic");
			player.Schizophrenic = false;
			if (player.IsLocalPlayer)
			{
				Singleton<MusicPlayer>.Instance.SetMusicDistorted(false, 5f);
				Singleton<MusicPlayer>.Instance.SetTrackEnabled("Schizo music", false);
				Singleton<AudioManager>.Instance.SetDistorted(false, 5f);
				Singleton<PostProcessingManager>.Instance.SaturationController.RemoveOverride("Schizophrenic");
				Singleton<EyelidOverlay>.Instance.OpenMultiplier.RemoveOverride("sedating");
			}
		}
	}
}
