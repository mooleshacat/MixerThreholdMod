using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class SwooshTest : MonoBehaviour
{
	// Token: 0x060001EC RID: 492 RVA: 0x0000B728 File Offset: 0x00009928
	private void Start()
	{
		float num = this._animation.frameRate * this._animation.length;
		this._startN = (float)this._start / num;
		this._endN = (float)this._end / num;
		this._animationState = base.GetComponent<Animation>()[this._animation.name];
		this._trail.Emit = false;
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000B794 File Offset: 0x00009994
	private void Update()
	{
		this._time += this._animationState.normalizedTime - this._prevAnimTime;
		if (this._time > 1f || this._firstFrame)
		{
			if (!this._firstFrame)
			{
				this._time -= 1f;
			}
			this._firstFrame = false;
		}
		if (this._prevTime < this._startN && this._time >= this._startN)
		{
			this._trail.Emit = true;
		}
		else if (this._prevTime < this._endN && this._time >= this._endN)
		{
			this._trail.Emit = false;
		}
		this._prevTime = this._time;
		this._prevAnimTime = this._animationState.normalizedTime;
	}

	// Token: 0x040001FC RID: 508
	[SerializeField]
	private AnimationClip _animation;

	// Token: 0x040001FD RID: 509
	private AnimationState _animationState;

	// Token: 0x040001FE RID: 510
	[SerializeField]
	private int _start;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private int _end;

	// Token: 0x04000200 RID: 512
	private float _startN;

	// Token: 0x04000201 RID: 513
	private float _endN;

	// Token: 0x04000202 RID: 514
	private float _time;

	// Token: 0x04000203 RID: 515
	private float _prevTime;

	// Token: 0x04000204 RID: 516
	private float _prevAnimTime;

	// Token: 0x04000205 RID: 517
	[SerializeField]
	private MeleeWeaponTrail _trail;

	// Token: 0x04000206 RID: 518
	private bool _firstFrame = true;
}
