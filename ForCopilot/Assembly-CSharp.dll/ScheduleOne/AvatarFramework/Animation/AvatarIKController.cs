using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x020009D8 RID: 2520
	public class AvatarIKController : MonoBehaviour
	{
		// Token: 0x0600441C RID: 17436 RVA: 0x0011E0BC File Offset: 0x0011C2BC
		private void Awake()
		{
			this.BodyIK.InitiateBipedIK();
			this.defaultLeftLegBendTarget = this.BodyIK.solvers.leftFoot.bendGoal;
			this.defaultRightLegBendTarget = this.BodyIK.solvers.rightFoot.bendGoal;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0011E10A File Offset: 0x0011C30A
		private void Start()
		{
			this.SetIKActive(false);
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0011E113 File Offset: 0x0011C313
		public void SetIKActive(bool active)
		{
			this.BodyIK.enabled = active;
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0011E121 File Offset: 0x0011C321
		public void OverrideLegBendTargets(Transform leftLegTarget, Transform rightLegTarget)
		{
			this.BodyIK.solvers.leftFoot.bendGoal = leftLegTarget;
			this.BodyIK.solvers.rightFoot.bendGoal = rightLegTarget;
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x0011E14F File Offset: 0x0011C34F
		public void ResetLegBendTargets()
		{
			this.BodyIK.solvers.leftFoot.bendGoal = this.defaultLeftLegBendTarget;
			this.BodyIK.solvers.rightFoot.bendGoal = this.defaultRightLegBendTarget;
		}

		// Token: 0x040030FD RID: 12541
		[Header("References")]
		public BipedIK BodyIK;

		// Token: 0x040030FE RID: 12542
		private Transform defaultLeftLegBendTarget;

		// Token: 0x040030FF RID: 12543
		private Transform defaultRightLegBendTarget;
	}
}
