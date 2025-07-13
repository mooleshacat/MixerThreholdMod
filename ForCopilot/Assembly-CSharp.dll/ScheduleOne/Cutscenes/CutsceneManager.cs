using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.DevUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Cutscenes
{
	// Token: 0x0200075B RID: 1883
	public class CutsceneManager : Singleton<CutsceneManager>
	{
		// Token: 0x060032BD RID: 12989 RVA: 0x000D3374 File Offset: 0x000D1574
		[Button]
		private void RunCutscene()
		{
			this.Play(this.cutsceneName);
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x000D3384 File Offset: 0x000D1584
		public void Play(string name)
		{
			Cutscene cutscene = this.Cutscenes.Find((Cutscene c) => c.Name.ToLower() == name.ToLower());
			if (cutscene != null)
			{
				cutscene.Play();
				this.playingCutscene = cutscene;
				this.playingCutscene.onEnd.AddListener(new UnityAction(this.Ended));
			}
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x000D33E8 File Offset: 0x000D15E8
		private void Ended()
		{
			this.playingCutscene.onEnd.RemoveListener(new UnityAction(this.Ended));
			this.playingCutscene = null;
		}

		// Token: 0x040023D4 RID: 9172
		public List<Cutscene> Cutscenes;

		// Token: 0x040023D5 RID: 9173
		[Header("Run cutscene by name")]
		[SerializeField]
		private string cutsceneName = "Wake up morning";

		// Token: 0x040023D6 RID: 9174
		private Cutscene playingCutscene;
	}
}
