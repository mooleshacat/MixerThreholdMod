using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A74 RID: 2676
	public class StaminaBar : MonoBehaviour
	{
		// Token: 0x060047FB RID: 18427 RVA: 0x0012EB3E File Offset: 0x0012CD3E
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			this.Group.alpha = 0f;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0012EB70 File Offset: 0x0012CD70
		private void PlayerSpawned()
		{
			PlayerMovement instance = PlayerSingleton<PlayerMovement>.Instance;
			instance.onStaminaReserveChanged = (Action<float>)Delegate.Combine(instance.onStaminaReserveChanged, new Action<float>(this.UpdateStaminaBar));
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0012EB98 File Offset: 0x0012CD98
		private void UpdateStaminaBar(float change)
		{
			if (!PlayerSingleton<PlayerMovement>.InstanceExists)
			{
				return;
			}
			Slider[] sliders = this.Sliders;
			for (int i = 0; i < sliders.Length; i++)
			{
				sliders[i].value = PlayerSingleton<PlayerMovement>.Instance.CurrentStaminaReserve / PlayerMovement.StaminaReserveMax;
			}
			this.Group.alpha = 1f;
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
			}
			this.routine = base.StartCoroutine(this.<UpdateStaminaBar>g__Routine|7_0());
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x0012EC10 File Offset: 0x0012CE10
		[CompilerGenerated]
		private IEnumerator <UpdateStaminaBar>g__Routine|7_0()
		{
			yield return new WaitForSeconds(1.5f);
			for (float i = 0f; i < 0.5f; i += Time.deltaTime)
			{
				this.Group.alpha = Mathf.Lerp(1f, 0f, i / 0.5f);
				yield return new WaitForEndOfFrame();
			}
			this.Group.alpha = 0f;
			this.routine = null;
			yield break;
		}

		// Token: 0x040034C9 RID: 13513
		public const float StaminaShowTime = 1.5f;

		// Token: 0x040034CA RID: 13514
		public const float StaminaFadeTime = 0.5f;

		// Token: 0x040034CB RID: 13515
		[Header("References")]
		public Slider[] Sliders;

		// Token: 0x040034CC RID: 13516
		public CanvasGroup Group;

		// Token: 0x040034CD RID: 13517
		private Coroutine routine;
	}
}
