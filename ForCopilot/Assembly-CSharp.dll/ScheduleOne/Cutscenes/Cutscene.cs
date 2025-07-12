using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Cutscenes
{
	// Token: 0x0200075A RID: 1882
	[RequireComponent(typeof(Animation))]
	public class Cutscene : MonoBehaviour
	{
		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060032B6 RID: 12982 RVA: 0x000D315D File Offset: 0x000D135D
		// (set) Token: 0x060032B7 RID: 12983 RVA: 0x000D3165 File Offset: 0x000D1365
		public bool IsPlaying { get; private set; }

		// Token: 0x060032B8 RID: 12984 RVA: 0x000D316E File Offset: 0x000D136E
		protected virtual void Awake()
		{
			this.animation = base.GetComponent<Animation>();
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x000D317C File Offset: 0x000D137C
		private void LateUpdate()
		{
			if (this.IsPlaying)
			{
				PlayerSingleton<PlayerCamera>.Instance.transform.position = this.CameraControl.position;
				PlayerSingleton<PlayerCamera>.Instance.transform.rotation = this.CameraControl.rotation;
			}
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000D31BC File Offset: 0x000D13BC
		public virtual void Play()
		{
			Console.Log("Playing cutscene: " + this.Name, null);
			this.animation.Play();
			this.IsPlaying = true;
			if (this.onPlay != null)
			{
				this.onPlay.Invoke();
			}
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement("Cutscene (" + this.Name + ")");
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraControl.position, this.CameraControl.rotation, 0f, false);
			Singleton<HUD>.Instance.canvas.enabled = false;
			if (this.DisablePlayerControl)
			{
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			}
			if (this.OverrideFOV)
			{
				PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(this.CameraFOV, 0f);
			}
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000D3298 File Offset: 0x000D1498
		public void InvokeEnd()
		{
			Console.Log("Cutscene ended: " + this.Name, null);
			this.animation.Stop();
			this.IsPlaying = false;
			if (this.onEnd != null)
			{
				this.onEnd.Invoke();
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement("Cutscene (" + this.Name + ")");
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.25f);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<HUD>.Instance.canvas.enabled = true;
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
		}

		// Token: 0x040023CC RID: 9164
		[Header("Settings")]
		public string Name = "Cutscene";

		// Token: 0x040023CD RID: 9165
		public bool DisablePlayerControl = true;

		// Token: 0x040023CE RID: 9166
		public bool OverrideFOV;

		// Token: 0x040023CF RID: 9167
		public float CameraFOV = 70f;

		// Token: 0x040023D0 RID: 9168
		[Header("References")]
		public Transform CameraControl;

		// Token: 0x040023D1 RID: 9169
		[Header("Events")]
		public UnityEvent onPlay;

		// Token: 0x040023D2 RID: 9170
		public UnityEvent onEnd;

		// Token: 0x040023D3 RID: 9171
		private Animation animation;
	}
}
