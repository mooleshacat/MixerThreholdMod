using System;
using ScheduleOne.AvatarFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine.Events;

namespace ScheduleOne.Cutscenes
{
	// Token: 0x0200075D RID: 1885
	public class EndCutscene : Cutscene
	{
		// Token: 0x060032C3 RID: 12995 RVA: 0x000D343D File Offset: 0x000D163D
		public override void Play()
		{
			base.Play();
			this.Avatar.LoadAvatarSettings(Player.Local.Avatar.CurrentSettings);
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x000D345F File Offset: 0x000D165F
		public void StandUp()
		{
			if (this.onStandUp != null)
			{
				this.onStandUp.Invoke();
			}
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x000D3474 File Offset: 0x000D1674
		public void RunStart()
		{
			if (this.onRunStart != null)
			{
				this.onRunStart.Invoke();
			}
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x000D3489 File Offset: 0x000D1689
		public void EngineStart()
		{
			if (this.onEngineStart != null)
			{
				this.onEngineStart.Invoke();
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x000D349E File Offset: 0x000D169E
		public void On3rdPerson()
		{
			this.Avatar.gameObject.SetActive(true);
			this.Avatar.Anim.SetBool("Sitting", true);
		}

		// Token: 0x040023D8 RID: 9176
		public UnityEvent onStandUp;

		// Token: 0x040023D9 RID: 9177
		public UnityEvent onRunStart;

		// Token: 0x040023DA RID: 9178
		public UnityEvent onEngineStart;

		// Token: 0x040023DB RID: 9179
		public Avatar Avatar;
	}
}
