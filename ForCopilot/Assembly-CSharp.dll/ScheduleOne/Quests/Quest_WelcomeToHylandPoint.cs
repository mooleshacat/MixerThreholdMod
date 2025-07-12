using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ScheduleOne.Quests
{
	// Token: 0x02000308 RID: 776
	public class Quest_WelcomeToHylandPoint : Quest
	{
		// Token: 0x0600114F RID: 4431 RVA: 0x0004CDBC File Offset: 0x0004AFBC
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Active && this.ReadMessagesQuest.State == EQuestState.Active && this.Nelson.MSGConversation != null && this.Nelson.MSGConversation.read)
			{
				this.ReadMessagesQuest.Complete();
			}
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0004CE10 File Offset: 0x0004B010
		private void Update()
		{
			if (base.QuestState == EQuestState.Active && this.ReturnToRVQuest.State == EQuestState.Active && InstanceFinder.IsServer)
			{
				float num;
				Player closestPlayer = Player.GetClosestPlayer(this.RV.transform.position, out num, null);
				if (num < this.ExplosionMinDist)
				{
					this.ReturnToRVQuest.Complete();
					return;
				}
				if (num < this.ExplosionMaxDist)
				{
					if (Vector3.Angle(closestPlayer.MimicCamera.forward, this.RV.transform.position - closestPlayer.MimicCamera.position) < 60f)
					{
						this.cameraLookTime += Time.deltaTime;
						if (this.cameraLookTime > 0.4f)
						{
							this.ReturnToRVQuest.Complete();
							return;
						}
					}
					else
					{
						this.cameraLookTime = 0f;
					}
				}
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0004CEE8 File Offset: 0x0004B0E8
		[Button]
		public void Explode()
		{
			Console.Log("RV exploding!", null);
			if (this.onExplode != null)
			{
				this.onExplode.Invoke();
			}
			base.StartCoroutine(Quest_WelcomeToHylandPoint.<Explode>g__Shake|11_0());
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0004CF14 File Offset: 0x0004B114
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
			if (state == EQuestState.Active)
			{
				string text;
				string controlPath;
				InputActionRebindingExtensions.GetBindingDisplayString(Singleton<GameInput>.Instance.GetAction(GameInput.ButtonCode.TogglePhone), 0, ref text, ref controlPath, 0);
				string displayNameForControlPath = Singleton<InputPromptsManager>.Instance.GetDisplayNameForControlPath(controlPath);
				this.ReadMessagesQuest.SetEntryTitle("Open your phone (press " + displayNameForControlPath + ") and read your messages");
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0004CF8A File Offset: 0x0004B18A
		[CompilerGenerated]
		internal static IEnumerator <Explode>g__Shake|11_0()
		{
			yield return new WaitForSeconds(0.35f);
			PlayerSingleton<PlayerCamera>.Instance.StartCameraShake(2f, 1f, true);
			yield break;
		}

		// Token: 0x0400113A RID: 4410
		public QuestEntry ReturnToRVQuest;

		// Token: 0x0400113B RID: 4411
		public QuestEntry ReadMessagesQuest;

		// Token: 0x0400113C RID: 4412
		public RV RV;

		// Token: 0x0400113D RID: 4413
		public UncleNelson Nelson;

		// Token: 0x0400113E RID: 4414
		[Header("Settings")]
		public float ExplosionMaxDist = 25f;

		// Token: 0x0400113F RID: 4415
		public float ExplosionMinDist = 50f;

		// Token: 0x04001140 RID: 4416
		public UnityEvent onExplode;

		// Token: 0x04001141 RID: 4417
		private bool exploded;

		// Token: 0x04001142 RID: 4418
		private float cameraLookTime;
	}
}
