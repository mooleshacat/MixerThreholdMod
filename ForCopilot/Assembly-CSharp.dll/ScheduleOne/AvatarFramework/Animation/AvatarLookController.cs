using System;
using System.Collections.Generic;
using System.Linq;
using RootMotion.FinalIK;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x020009D9 RID: 2521
	public class AvatarLookController : MonoBehaviour
	{
		// Token: 0x06004422 RID: 17442 RVA: 0x0011E188 File Offset: 0x0011C388
		private void Awake()
		{
			this.avatar = base.GetComponent<Avatar>();
			this.avatar.onRagdollChange.AddListener(new UnityAction<bool, bool, bool>(this.RagdollChange));
			this.defaultIKWeight = this.Aim.solver.GetIKPositionWeight();
			this.lookAtTarget = new GameObject("LookAtTarget (" + base.gameObject.name + ")").transform;
			Transform transform = this.lookAtTarget;
			GameObject gameObject = GameObject.Find("_Temp");
			transform.SetParent((gameObject != null) ? gameObject.transform : null);
			this.LookForward();
			this.lookAtTarget.transform.position = this.lookAtPos;
			this.lastFrameOffset = this.LookOrigin.InverseTransformPoint(this.lookAtTarget.position);
			this.NPC = base.GetComponentInParent<NPC>();
			base.InvokeRepeating("UpdateNearestPlayer", 0f, 0.5f);
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x0011E278 File Offset: 0x0011C478
		private void UpdateShit()
		{
			if (this.ForceLookTarget != null && this.CanLookAt(this.ForceLookTarget.position))
			{
				this.OverrideLookTarget(this.ForceLookTarget.position, 100, this.ForceLookRotateBody);
				return;
			}
			if (this.AutoLookAtPlayer && Player.Local != null && (Player.Local.Paranoid || Player.Local.Schizophrenic))
			{
				this.OverrideLookTarget(Player.Local.MimicCamera.position, 200, false);
				this.Aim.enabled = (this.nearestPlayerDist < 20f * QualitySettings.lodBias);
				this.Aim.solver.clampWeight = Mathf.MoveTowards(this.Aim.solver.clampWeight, this.AimIKWeight, Time.deltaTime * 2f);
				return;
			}
			if (this.DEBUG)
			{
				Console.Log("Nearest player: " + ((this.nearestPlayer != null) ? this.nearestPlayer.name : "null") + " dist: " + this.nearestPlayerDist.ToString(), null);
				Console.Log("Visibility: " + this.NPC.awareness.VisionCone.GetPlayerVisibility(this.nearestPlayer).ToString(), null);
				Console.Log("AutoLookAtPlayer: " + this.AutoLookAtPlayer.ToString(), null);
				Console.Log("CanLookAt: " + this.CanLookAt(this.nearestPlayer.EyePosition).ToString(), null);
			}
			if (this.nearestPlayer != null && this.AutoLookAtPlayer && this.CanLookAt(this.nearestPlayer.EyePosition) && (this.NPC == null || this.NPC.awareness.VisionCone.GetPlayerVisibility(this.nearestPlayer) > this.NPC.awareness.VisionCone.MinVisionDelta))
			{
				Vector3 a = this.nearestPlayer.EyePosition;
				if (this.nearestPlayer.IsOwner)
				{
					a = this.nearestPlayer.MimicCamera.position;
				}
				if (this.nearestPlayerDist < 4f)
				{
					this.lookAtPos = a;
					if (this.DEBUG)
					{
						Console.Log("Looking at player: " + this.nearestPlayer.name, null);
					}
				}
				else if (this.nearestPlayerDist < 10f && Vector3.Angle(a - this.HeadBone.position, this.HeadBone.forward) < 45f)
				{
					Transform mimicCamera = this.nearestPlayer.MimicCamera;
					if (Vector3.Angle(mimicCamera.forward, (this.HeadBone.position - mimicCamera.position).normalized) < 15f)
					{
						this.lookAtPos = a;
						if (this.DEBUG)
						{
							Console.Log("Looking at player: " + this.nearestPlayer.name, null);
						}
					}
					else
					{
						this.LookForward();
					}
				}
				else
				{
					this.LookForward();
				}
			}
			else
			{
				this.LookForward();
			}
			if (this.Aim != null)
			{
				if (this.avatar.Ragdolled || this.avatar.Anim.StandUpAnimationPlaying)
				{
					this.Aim.solver.clampWeight = 0f;
					this.Aim.enabled = false;
					return;
				}
				this.Aim.enabled = (this.nearestPlayerDist < 20f * QualitySettings.lodBias);
				this.Aim.solver.clampWeight = Mathf.MoveTowards(this.Aim.solver.clampWeight, this.AimIKWeight, Time.deltaTime * 2f);
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0011E660 File Offset: 0x0011C860
		private void UpdateNearestPlayer()
		{
			if (Player.Local == null)
			{
				return;
			}
			this.localPlayerDist = Vector3.Distance(Player.Local.Avatar.CenterPoint, base.transform.position);
			this.cullRange = 30f * QualitySettings.lodBias;
			if (this.localPlayerDist > this.cullRange)
			{
				return;
			}
			List<Player> list = new List<Player>();
			foreach (Player player in Player.PlayerList)
			{
				if (player.Avatar.LookController == this)
				{
					list.Add(player);
				}
			}
			this.nearestPlayer = Player.GetClosestPlayer(base.transform.position, out this.nearestPlayerDist, list);
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x0011E73C File Offset: 0x0011C93C
		private void LateUpdate()
		{
			if (this.localPlayerDist > this.cullRange)
			{
				if (this.Aim != null && this.Aim.enabled)
				{
					this.Aim.enabled = false;
				}
				this.lastFrameLookOriginPos = this.LookOrigin.position;
				this.lastFrameLookOriginForward = this.LookOrigin.forward;
				return;
			}
			this.UpdateShit();
			if (this.overrideLookAt)
			{
				this.lookAtPos = this.overriddenLookTarget;
			}
			if (!this.avatar.Ragdolled)
			{
				if (this.overrideLookAt && this.overrideRotateBody)
				{
					Vector3 to = this.lookAtPos - base.transform.position;
					to.y = 0f;
					to.Normalize();
					float y = Vector3.SignedAngle(base.transform.parent.forward, to, Vector3.up);
					if (this.DEBUG)
					{
						Console.Log("Body rotation: " + y.ToString(), null);
					}
					this.avatar.transform.localRotation = Quaternion.Lerp(this.avatar.transform.localRotation, Quaternion.Euler(0f, y, 0f), Time.deltaTime * this.BodyRotationSpeed);
				}
				else if (this.avatar.transform.parent != null)
				{
					this.avatar.transform.localRotation = Quaternion.Lerp(this.avatar.transform.localRotation, Quaternion.identity, Time.deltaTime * this.BodyRotationSpeed);
				}
			}
			this.LerpTargetTransform();
			this.Eyes.LookAt(this.lookAtPos, false);
			this.overrideLookAt = false;
			this.overriddenLookTarget = Vector3.zero;
			this.overrideLookPriority = 0;
			this.overrideRotateBody = false;
			this.lastFrameLookOriginPos = this.LookOrigin.position;
			this.lastFrameLookOriginForward = this.LookOrigin.forward;
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x0011E934 File Offset: 0x0011CB34
		public void OverrideLookTarget(Vector3 targetPosition, int priority, bool rotateBody = false)
		{
			if (this.overrideLookAt && priority < this.overrideLookPriority)
			{
				return;
			}
			if (this.DEBUG)
			{
				Debug.DrawLine(base.transform.position, targetPosition, Color.red, 0.1f);
				string str = "Overriding look target to: ";
				Vector3 vector = targetPosition;
				Console.Log(str + vector.ToString() + " priority: " + priority.ToString(), null);
			}
			this.overrideLookAt = true;
			this.overriddenLookTarget = targetPosition;
			this.overrideLookPriority = priority;
			this.overrideRotateBody = rotateBody;
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x0011E9C0 File Offset: 0x0011CBC0
		private void LookForward()
		{
			if (this.DEBUG)
			{
				Console.Log("Looking forward", null);
			}
			this.LookForwardTarget.position = this.HeadBone.position + base.transform.forward * 1f;
			this.lookAtPos = this.LookForwardTarget.position;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0011EA24 File Offset: 0x0011CC24
		private void LerpTargetTransform()
		{
			this.lookAtTarget.position = this.LookOrigin.TransformPoint(this.lastFrameOffset);
			Vector3 normalized = (this.lookAtTarget.position - this.LookOrigin.position).normalized;
			Vector3 normalized2 = (this.lookAtPos - this.LookOrigin.position).normalized;
			Vector3 b = Vector3.Lerp(normalized, normalized2, Time.deltaTime * this.LookLerpSpeed);
			this.lookAtTarget.position = this.LookOrigin.position + b;
			if (this.Aim != null)
			{
				this.Aim.solver.target = this.lookAtTarget;
			}
			this.lastFrameOffset = this.LookOrigin.InverseTransformPoint(this.lookAtTarget.position);
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0011EB00 File Offset: 0x0011CD00
		private Player GetNearestPlayer()
		{
			List<Player> playerList = Player.PlayerList;
			if (playerList.Count <= 0)
			{
				return null;
			}
			return (from p in playerList
			orderby Vector3.Distance(p.transform.position, base.transform.position)
			select p).First<Player>();
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0011EB38 File Offset: 0x0011CD38
		private bool CanLookAt(Vector3 position)
		{
			Vector3 forward = this.avatar.transform.forward;
			Vector3 normalized = (position - this.avatar.transform.position).normalized;
			return Vector3.SignedAngle(forward, normalized, Vector3.up) < 90f;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x000045B1 File Offset: 0x000027B1
		protected void RagdollChange(bool oldValue, bool ragdoll, bool playStandUpAnim)
		{
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0011EB86 File Offset: 0x0011CD86
		public void OverrideIKWeight(float weight)
		{
			this.Aim.solver.SetIKPositionWeight(weight);
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0011EB99 File Offset: 0x0011CD99
		public void ResetIKWeight()
		{
			this.Aim.solver.SetIKPositionWeight(this.defaultIKWeight);
		}

		// Token: 0x04003100 RID: 12544
		public const float LookAtPlayerRange = 4f;

		// Token: 0x04003101 RID: 12545
		public const float EyeContractRange = 10f;

		// Token: 0x04003102 RID: 12546
		public const float AimIKRange = 20f;

		// Token: 0x04003103 RID: 12547
		public bool DEBUG;

		// Token: 0x04003104 RID: 12548
		[Header("References")]
		public AimIK Aim;

		// Token: 0x04003105 RID: 12549
		public Transform HeadBone;

		// Token: 0x04003106 RID: 12550
		public Transform LookForwardTarget;

		// Token: 0x04003107 RID: 12551
		public Transform LookOrigin;

		// Token: 0x04003108 RID: 12552
		public EyeController Eyes;

		// Token: 0x04003109 RID: 12553
		[Header("Optional NPC reference")]
		public NPC NPC;

		// Token: 0x0400310A RID: 12554
		[Header("Settings")]
		public bool AutoLookAtPlayer = true;

		// Token: 0x0400310B RID: 12555
		public float LookLerpSpeed = 1f;

		// Token: 0x0400310C RID: 12556
		public float AimIKWeight = 0.6f;

		// Token: 0x0400310D RID: 12557
		public float BodyRotationSpeed = 1f;

		// Token: 0x0400310E RID: 12558
		private Avatar avatar;

		// Token: 0x0400310F RID: 12559
		private Vector3 lookAtPos = Vector3.zero;

		// Token: 0x04003110 RID: 12560
		private Transform lookAtTarget;

		// Token: 0x04003111 RID: 12561
		private Vector3 lastFrameOffset = Vector3.zero;

		// Token: 0x04003112 RID: 12562
		private bool overrideLookAt;

		// Token: 0x04003113 RID: 12563
		private Vector3 overriddenLookTarget = Vector3.zero;

		// Token: 0x04003114 RID: 12564
		private int overrideLookPriority;

		// Token: 0x04003115 RID: 12565
		private bool overrideRotateBody;

		// Token: 0x04003116 RID: 12566
		private Vector3 lastFrameLookOriginPos;

		// Token: 0x04003117 RID: 12567
		private Vector3 lastFrameLookOriginForward;

		// Token: 0x04003118 RID: 12568
		public Transform ForceLookTarget;

		// Token: 0x04003119 RID: 12569
		public bool ForceLookRotateBody;

		// Token: 0x0400311A RID: 12570
		private float defaultIKWeight = 0.6f;

		// Token: 0x0400311B RID: 12571
		private Player nearestPlayer;

		// Token: 0x0400311C RID: 12572
		private float nearestPlayerDist;

		// Token: 0x0400311D RID: 12573
		private float localPlayerDist;

		// Token: 0x0400311E RID: 12574
		private float cullRange = 100f;
	}
}
