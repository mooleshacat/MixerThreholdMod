using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x02000643 RID: 1603
	public class ViewmodelSway : PlayerSingleton<ViewmodelSway>
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x000AACE1 File Offset: 0x000A8EE1
		protected float calculatedJumpJoltHeight
		{
			get
			{
				return this.jumpJoltHeight;
			}
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x000AACE9 File Offset: 0x000A8EE9
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x000AACF1 File Offset: 0x000A8EF1
		protected override void Awake()
		{
			base.Awake();
			this.initialPos = base.transform.localPosition;
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x000AAD0C File Offset: 0x000A8F0C
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			this.timeSinceLanded = this.landJoltTime;
			PlayerMovement instance = PlayerSingleton<PlayerMovement>.Instance;
			instance.onJump = (Action)Delegate.Combine(instance.onJump, new Action(this.StartJump));
			PlayerMovement instance2 = PlayerSingleton<PlayerMovement>.Instance;
			instance2.onLand = (Action)Delegate.Combine(instance2.onLand, new Action(this.Land));
			PlayerInventory instance3 = PlayerSingleton<PlayerInventory>.Instance;
			instance3.onInventoryStateChanged = (Action<bool>)Delegate.Combine(instance3.onInventoryStateChanged, new Action<bool>(this.InventoryStateChanged));
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000AADA0 File Offset: 0x000A8FA0
		protected void Update()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (this.breatheBobbingEnabled)
			{
				this.BreatheBob();
			}
			if (this.swayingEnabled)
			{
				this.Sway();
			}
			if (this.walkBobbingEnabled)
			{
				this.WalkBob();
			}
			if (this.jumpJoltEnabled)
			{
				this.UpdateJump();
			}
			Vector3 vector = this.landPos;
			if (PlayerSingleton<PlayerInventory>.Instance.currentEquipTime < this.equipBopTime)
			{
				this.equipBopPos = new Vector3(0f, this.equipBopVerticalOffset * (1f - Mathf.Sqrt(Mathf.Clamp(PlayerSingleton<PlayerInventory>.Instance.currentEquipTime / this.equipBopTime, 0f, 1f))), 0f);
			}
			else
			{
				this.equipBopPos = Vector3.zero;
			}
			if (!PlayerSingleton<PlayerInventory>.Instance.HotbarEnabled)
			{
				this.equipBopPos = new Vector3(0f, this.equipBopVerticalOffset, 0f);
			}
			if (!PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped)
			{
				this.equipBopPos = Vector3.zero;
				this.breatheBobPos.y = 0f;
			}
			this.RefreshViewmodel();
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000AAEB3 File Offset: 0x000A90B3
		private void InventoryStateChanged(bool active)
		{
			if (active)
			{
				this.Update();
			}
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x000AAEC0 File Offset: 0x000A90C0
		public void RefreshViewmodel()
		{
			if (this.DEBUG)
			{
				Console.Log(string.Concat(new string[]
				{
					"Viewmodel position breakdown: \nSway",
					this.swayPos.ToString("F6"),
					"\nBreatheBob",
					this.breatheBobPos.ToString("F6"),
					"\nWalkBob",
					this.walkBobPos.ToString("F6"),
					"\nJump",
					this.jumpPos.ToString("F6"),
					"\nLand",
					this.landPos.ToString("F6"),
					"\nFallOffset",
					this.fallOffsetPos.ToString("F6"),
					"\nEquipBop",
					this.equipBopPos.ToString("F6")
				}), null);
			}
			try
			{
				base.transform.localPosition = this.swayPos + this.breatheBobPos + this.walkBobPos + this.jumpPos + this.landPos + this.fallOffsetPos + this.equipBopPos;
			}
			catch
			{
				Console.LogWarning("Viewmodel pos set failed.", null);
			}
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x000AB020 File Offset: 0x000A9220
		protected void BreatheBob()
		{
			this.lastHeight = this.breatheBobPos.y + (Mathf.Sin(Time.timeSinceLevelLoad * this.breathingSpeedMultiplier) - this.lastHeight) * this.breathingHeightMultiplier;
			this.breatheBobPos = new Vector3(0f, this.lastHeight, 0f);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x000AB07C File Offset: 0x000A927C
		protected void Sway()
		{
			float x = this.swayPos.x;
			float y = this.swayPos.y;
			float num = 0f;
			float num2 = 0f;
			if (PlayerSingleton<PlayerCamera>.Instance.canLook)
			{
				num = x - GameInput.MouseDelta.x * this.horizontalSwayMultiplier;
				num2 = y - GameInput.MouseDelta.y * this.verticalSwayMultiplier;
			}
			num = Mathf.Clamp(num, -this.maxHorizontal, this.maxHorizontal);
			num2 = Mathf.Clamp(num2, -this.maxVertical, this.maxVertical);
			Vector3 a = Vector3.Lerp(new Vector3(num, num2, 0f), Vector3.zero, Time.deltaTime * this.returnMultiplier / (1f + Mathf.Sqrt(Mathf.Abs(GameInput.MouseDelta.x) + Mathf.Abs(GameInput.MouseDelta.y))));
			this.swayPos = Vector3.Lerp(this.swayPos, a + this.initialPos, Time.deltaTime * this.swaySmooth);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x000AB184 File Offset: 0x000A9384
		protected void WalkBob()
		{
			bool flag = false;
			float d = Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.x) + Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.z);
			if (Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.x) > 0f || Mathf.Abs(PlayerSingleton<PlayerMovement>.Instance.Movement.z) > 0f)
			{
				flag = true;
			}
			if (!flag)
			{
				this.timeSinceWalkStart_vert = 0f;
				this.timeSinceWalkStart_horiz = 0f;
			}
			float num = 1f;
			if (PlayerSingleton<PlayerMovement>.Instance.isSprinting)
			{
				num = 1.4f;
			}
			this.walkBobPos = Vector3.Lerp(this.walkBobPos, new Vector3(this.horizontalMovement.Evaluate(this.timeSinceWalkStart_horiz % 1f) * this.horizontalBobWidth * num, this.verticalMovement.Evaluate(this.timeSinceWalkStart_vert % 1f) * this.verticalBobHeight * num, 0f) * d, Time.deltaTime * this.walkBobSmooth);
			if (flag)
			{
				float num2 = 1f;
				if (PlayerSingleton<PlayerMovement>.Instance.isSprinting)
				{
					num2 = 1.6f;
				}
				this.timeSinceWalkStart_vert += Time.deltaTime * this.verticalBobSpeed * num2;
				this.timeSinceWalkStart_horiz += Time.deltaTime * this.horizontalBobSpeed * num2;
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x000AB2E4 File Offset: 0x000A94E4
		protected void StartJump()
		{
			this.timeSinceJumpStart = 0f;
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x000AB2F4 File Offset: 0x000A94F4
		protected void UpdateJump()
		{
			if (!PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped || !PlayerSingleton<PlayerInventory>.Instance.HotbarEnabled)
			{
				return;
			}
			if (PlayerSingleton<PlayerMovement>.Instance.airTime > 0f)
			{
				this.timeSinceJumpStart += Time.deltaTime;
				Vector3 b = new Vector3(0f, this.jumpCurve.Evaluate(Mathf.Clamp(this.timeSinceJumpStart / this.jumpJoltTime, 0f, 1f)) * this.calculatedJumpJoltHeight, 0f);
				this.jumpPos = Vector3.Lerp(this.jumpPos, b, Time.deltaTime * this.jumpJoltSmooth);
			}
			else if (PlayerSingleton<PlayerMovement>.Instance.IsGrounded)
			{
				this.timeSinceJumpStart = 0f;
				Vector3 b2 = new Vector3(0f, this.landCurve.Evaluate(Mathf.Clamp(this.timeSinceLanded / this.landJoltTime, 0f, 1f)) * this.landJoltMultiplier, 0f);
				if (this.landJoltMultiplier > 0f)
				{
					this.landPos = Vector3.Lerp(this.landPos, b2, Mathf.Abs(Time.deltaTime * this.landJoltSmooth / this.landJoltMultiplier));
				}
				else
				{
					this.landPos = Vector3.zero;
				}
				this.timeSinceLanded += Time.deltaTime;
				Vector3 zero = Vector3.zero;
				this.jumpPos = Vector3.Lerp(this.jumpPos, zero, Time.deltaTime * this.jumpJoltSmooth);
			}
			if (!PlayerSingleton<PlayerMovement>.Instance.IsGrounded && (this.timeSinceJumpStart > this.jumpJoltTime || PlayerSingleton<PlayerMovement>.Instance.airTime == 0f))
			{
				this.fallOffsetPos.y = this.fallOffsetPos.y + this.fallOffsetRate * Time.deltaTime;
				this.fallOffsetPos.y = Mathf.Clamp(this.fallOffsetPos.y, 0f, this.maxFallOffsetAmount);
				return;
			}
			this.fallOffsetPos.y = 0f;
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000AB4F0 File Offset: 0x000A96F0
		protected void Land()
		{
			this.landJoltMultiplier = this.jumpPos.y + this.fallOffsetPos.y + this.landPos.y;
			this.landPos.y = this.landCurve.Evaluate(Mathf.Clamp(0f / this.landJoltTime, 0f, 1f)) * this.landJoltMultiplier;
			this.timeSinceLanded = 0f;
			this.jumpPos.y = 0f;
			this.fallOffsetPos.y = 0f;
		}

		// Token: 0x04001DF0 RID: 7664
		public bool DEBUG;

		// Token: 0x04001DF1 RID: 7665
		[Header("Settings - Breathing")]
		public bool breatheBobbingEnabled = true;

		// Token: 0x04001DF2 RID: 7666
		[Range(0f, 0.0004f)]
		[SerializeField]
		protected float breathingHeightMultiplier = 5E-05f;

		// Token: 0x04001DF3 RID: 7667
		[Range(0f, 10f)]
		[SerializeField]
		protected float breathingSpeedMultiplier = 1f;

		// Token: 0x04001DF4 RID: 7668
		private float lastHeight;

		// Token: 0x04001DF5 RID: 7669
		private Vector3 breatheBobPos;

		// Token: 0x04001DF6 RID: 7670
		[Header("Settings - Sway - Movement")]
		public bool swayingEnabled = true;

		// Token: 0x04001DF7 RID: 7671
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float horizontalSwayMultiplier = 1f;

		// Token: 0x04001DF8 RID: 7672
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float verticalSwayMultiplier = 1f;

		// Token: 0x04001DF9 RID: 7673
		[Range(0f, 0.5f)]
		[SerializeField]
		protected float maxHorizontal = 0.1f;

		// Token: 0x04001DFA RID: 7674
		[Range(0f, 0.5f)]
		[SerializeField]
		protected float maxVertical = 0.1f;

		// Token: 0x04001DFB RID: 7675
		[SerializeField]
		protected float swaySmooth = 3f;

		// Token: 0x04001DFC RID: 7676
		[SerializeField]
		protected float returnMultiplier = 0.1f;

		// Token: 0x04001DFD RID: 7677
		private Vector3 initialPos = Vector3.zero;

		// Token: 0x04001DFE RID: 7678
		private Vector3 swayPos;

		// Token: 0x04001DFF RID: 7679
		[Header("Settings - Walk Bob")]
		public bool walkBobbingEnabled = true;

		// Token: 0x04001E00 RID: 7680
		[SerializeField]
		protected AnimationCurve verticalMovement;

		// Token: 0x04001E01 RID: 7681
		[SerializeField]
		protected AnimationCurve horizontalMovement;

		// Token: 0x04001E02 RID: 7682
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float verticalBobHeight = 0.1f;

		// Token: 0x04001E03 RID: 7683
		[Range(0f, 5f)]
		[SerializeField]
		protected float verticalBobSpeed = 2f;

		// Token: 0x04001E04 RID: 7684
		[Range(0f, 0.1f)]
		[SerializeField]
		protected float horizontalBobWidth = 0.1f;

		// Token: 0x04001E05 RID: 7685
		[Range(0f, 5f)]
		[SerializeField]
		protected float horizontalBobSpeed = 2f;

		// Token: 0x04001E06 RID: 7686
		[SerializeField]
		protected float walkBobSmooth = 3f;

		// Token: 0x04001E07 RID: 7687
		[SerializeField]
		protected float sprintSpeedMultiplier = 1.25f;

		// Token: 0x04001E08 RID: 7688
		[HideInInspector]
		public float walkBobMultiplier = 1f;

		// Token: 0x04001E09 RID: 7689
		private Vector3 walkBobPos;

		// Token: 0x04001E0A RID: 7690
		private float timeSinceWalkStart_vert;

		// Token: 0x04001E0B RID: 7691
		private float timeSinceWalkStart_horiz;

		// Token: 0x04001E0C RID: 7692
		[Header("Settings - Jump Jolt")]
		public bool jumpJoltEnabled = true;

		// Token: 0x04001E0D RID: 7693
		[SerializeField]
		protected AnimationCurve jumpCurve;

		// Token: 0x04001E0E RID: 7694
		[SerializeField]
		protected float jumpJoltTime = 0.6f;

		// Token: 0x04001E0F RID: 7695
		[SerializeField]
		protected float jumpJoltHeight = 0.2f;

		// Token: 0x04001E10 RID: 7696
		[SerializeField]
		protected float jumpJoltSmooth = 5f;

		// Token: 0x04001E11 RID: 7697
		[Header("Settings - Equip Bop")]
		[SerializeField]
		protected float equipBopVerticalOffset = -0.5f;

		// Token: 0x04001E12 RID: 7698
		[SerializeField]
		protected float equipBopTime = 0.2f;

		// Token: 0x04001E13 RID: 7699
		private Vector3 equipBopPos;

		// Token: 0x04001E14 RID: 7700
		private float timeSinceJumpStart;

		// Token: 0x04001E15 RID: 7701
		private Vector3 jumpPos = Vector3.zero;

		// Token: 0x04001E16 RID: 7702
		[Header("Settings - Falling")]
		[Range(0f, 1f)]
		[SerializeField]
		protected float fallOffsetRate = 0.1f;

		// Token: 0x04001E17 RID: 7703
		[Range(0f, 2f)]
		[SerializeField]
		protected float maxFallOffsetAmount = 0.2f;

		// Token: 0x04001E18 RID: 7704
		private Vector3 fallOffsetPos = Vector3.zero;

		// Token: 0x04001E19 RID: 7705
		[Header("Settings - Land Jolt")]
		[SerializeField]
		protected AnimationCurve landCurve;

		// Token: 0x04001E1A RID: 7706
		[SerializeField]
		protected float landJoltTime = 0.6f;

		// Token: 0x04001E1B RID: 7707
		[SerializeField]
		protected float landJoltSmooth = 5f;

		// Token: 0x04001E1C RID: 7708
		private Vector3 landPos = Vector3.zero;

		// Token: 0x04001E1D RID: 7709
		private float timeSinceLanded;

		// Token: 0x04001E1E RID: 7710
		private float landJoltMultiplier = 1f;
	}
}
