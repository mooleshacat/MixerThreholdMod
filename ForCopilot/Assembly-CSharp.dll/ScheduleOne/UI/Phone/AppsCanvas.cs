using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Tooltips;
using UnityEngine;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000ADE RID: 2782
	public class AppsCanvas : PlayerSingleton<AppsCanvas>
	{
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004A99 RID: 19097 RVA: 0x00139955 File Offset: 0x00137B55
		// (set) Token: 0x06004A9A RID: 19098 RVA: 0x0013995D File Offset: 0x00137B5D
		public bool isOpen { get; private set; }

		// Token: 0x06004A9B RID: 19099 RVA: 0x00139966 File Offset: 0x00137B66
		protected override void Awake()
		{
			base.Awake();
			this.SetIsOpen(false);
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x00139978 File Offset: 0x00137B78
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				return;
			}
			Phone instance = PlayerSingleton<Phone>.Instance;
			instance.onPhoneOpened = (Action)Delegate.Combine(instance.onPhoneOpened, new Action(this.PhoneOpened));
			Phone instance2 = PlayerSingleton<Phone>.Instance;
			instance2.onPhoneClosed = (Action)Delegate.Combine(instance2.onPhoneClosed, new Action(this.PhoneClosed));
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x001399DC File Offset: 0x00137BDC
		protected void PhoneOpened()
		{
			if (this.isOpen)
			{
				this.SetCanvasActive(true);
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x001399ED File Offset: 0x00137BED
		protected void PhoneClosed()
		{
			this.delayedSetOpenRoutine = base.StartCoroutine(this.DelayedSetCanvasActive(false, 0.25f));
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x00139A07 File Offset: 0x00137C07
		private IEnumerator DelayedSetCanvasActive(bool active, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.delayedSetOpenRoutine = null;
			this.SetCanvasActive(active);
			yield break;
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x00139A24 File Offset: 0x00137C24
		public void SetIsOpen(bool o)
		{
			this.isOpen = o;
			this.SetCanvasActive(o);
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x00139A34 File Offset: 0x00137C34
		private void SetCanvasActive(bool a)
		{
			if (this.delayedSetOpenRoutine != null)
			{
				base.StopCoroutine(this.delayedSetOpenRoutine);
			}
			this.canvas.enabled = a;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x00139A56 File Offset: 0x00137C56
		protected override void Start()
		{
			base.Start();
			Singleton<TooltipManager>.Instance.AddCanvas(this.canvas);
		}

		// Token: 0x040036FB RID: 14075
		[Header("References")]
		public Canvas canvas;

		// Token: 0x040036FC RID: 14076
		private Coroutine delayedSetOpenRoutine;
	}
}
