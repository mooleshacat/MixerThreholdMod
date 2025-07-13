using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.Settings
{
	// Token: 0x02000ABD RID: 2749
	public class Keybinder : MonoBehaviour
	{
		// Token: 0x060049D8 RID: 18904 RVA: 0x001369FE File Offset: 0x00134BFE
		private void Awake()
		{
			RebindActionUI rebindActionUI = this.rebindActionUI;
			rebindActionUI.onRebind = (Action)Delegate.Combine(rebindActionUI.onRebind, new Action(this.OnRebind));
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00136A28 File Offset: 0x00134C28
		private void Start()
		{
			Settings instance = Singleton<Settings>.Instance;
			instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.OnSettingsApplied));
			Settings instance2 = Singleton<Settings>.Instance;
			instance2.onInputsApplied = (Action)Delegate.Combine(instance2.onInputsApplied, new Action(this.OnSettingsApplied));
			this.rebindActionUI.UpdateBindingDisplay();
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x00136A8C File Offset: 0x00134C8C
		private void OnDestroy()
		{
			if (this.rebindActionUI != null)
			{
				RebindActionUI rebindActionUI = this.rebindActionUI;
				rebindActionUI.onRebind = (Action)Delegate.Remove(rebindActionUI.onRebind, new Action(this.OnRebind));
			}
			if (Singleton<Settings>.InstanceExists)
			{
				Settings instance = Singleton<Settings>.Instance;
				instance.onInputsApplied = (Action)Delegate.Remove(instance.onInputsApplied, new Action(this.OnSettingsApplied));
			}
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00136AFB File Offset: 0x00134CFB
		private void OnRebind()
		{
			base.StartCoroutine(Keybinder.<OnRebind>g__ApplySettings|4_0());
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x00136B09 File Offset: 0x00134D09
		private void OnSettingsApplied()
		{
			this.rebindActionUI.UpdateBindingDisplay();
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x00136B16 File Offset: 0x00134D16
		[CompilerGenerated]
		internal static IEnumerator <OnRebind>g__ApplySettings|4_0()
		{
			yield return new WaitForEndOfFrame();
			Singleton<Settings>.Instance.WriteInputSettings(Singleton<Settings>.Instance.InputSettings);
			Singleton<Settings>.Instance.ApplyInputSettings(Singleton<Settings>.Instance.ReadInputSettings());
			yield break;
		}

		// Token: 0x04003658 RID: 13912
		public RebindActionUI rebindActionUI;
	}
}
