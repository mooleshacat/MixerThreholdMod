using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.TV
{
	// Token: 0x020002A4 RID: 676
	public class Pong : TVApp
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x0003E56D File Offset: 0x0003C76D
		// (set) Token: 0x06000E16 RID: 3606 RVA: 0x0003E575 File Offset: 0x0003C775
		public Pong.EGameMode GameMode { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0003E57E File Offset: 0x0003C77E
		// (set) Token: 0x06000E18 RID: 3608 RVA: 0x0003E586 File Offset: 0x0003C786
		public Pong.EState State { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0003E58F File Offset: 0x0003C78F
		// (set) Token: 0x06000E1A RID: 3610 RVA: 0x0003E597 File Offset: 0x0003C797
		public int LeftScore { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0003E5A0 File Offset: 0x0003C7A0
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x0003E5A8 File Offset: 0x0003C7A8
		public int RightScore { get; set; }

		// Token: 0x06000E1D RID: 3613 RVA: 0x0003E5B1 File Offset: 0x0003C7B1
		private void Update()
		{
			if (!base.IsOpen || base.IsPaused)
			{
				return;
			}
			this.UpdateInputs();
			if (this.GameMode == Pong.EGameMode.SinglePlayer)
			{
				this.UpdateAI();
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0003E5D8 File Offset: 0x0003C7D8
		private void FixedUpdate()
		{
			if (!base.IsOpen || base.IsPaused)
			{
				this.Ball.RB.isKinematic = true;
				return;
			}
			this.ballVelocity = this.Ball.RB.velocity;
			this.Ball.RB.velocity += this.Ball.RB.velocity.normalized * this.VelocityGainPerSecond * Time.deltaTime;
			if (this.Ball.RB.velocity.magnitude > this.MaxVelocity)
			{
				this.Ball.RB.velocity = this.Ball.RB.velocity.normalized * this.MaxVelocity;
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0003E6B8 File Offset: 0x0003C8B8
		protected override void TryPause()
		{
			this.Ball.RB.isKinematic = true;
			if (this.State == Pong.EState.Ready || this.State == Pong.EState.GameOver)
			{
				this.Close();
				return;
			}
			base.TryPause();
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0003E6EC File Offset: 0x0003C8EC
		public void UpdateInputs()
		{
			if (this.State == Pong.EState.Playing)
			{
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.Rect, Input.mousePosition, PlayerSingleton<PlayerCamera>.Instance.Camera, ref vector);
				if (this.GameMode == Pong.EGameMode.SinglePlayer)
				{
					this.SetPaddleTargetY(Pong.ESide.Left, vector.y);
					return;
				}
			}
			else if (this.State == Pong.EState.Ready)
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
				{
					this.ServeBall();
					return;
				}
			}
			else if (this.State == Pong.EState.GameOver && GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				this.ResetGame();
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0003E768 File Offset: 0x0003C968
		private void UpdateAI()
		{
			if (this.State == Pong.EState.Playing)
			{
				this.reactionTimer += Time.deltaTime;
				if (this.reactionTimer >= this.ReactionTime)
				{
					float t = (Mathf.Clamp01(this.Ball.Rect.anchoredPosition.x / 300f) + 1f) / 2f;
					this.reactionTimer = 0f;
					float num = this.TargetRandomization * Mathf.Lerp(3f, 1f, t);
					float targetY = this.Ball.Rect.anchoredPosition.y + UnityEngine.Random.Range(-num, num);
					this.RightPaddle.SetTargetY(targetY);
					this.RightPaddle.SpeedMultiplier = Mathf.Lerp(0.1f, 1f, t) * this.SpeedMultiplier;
				}
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0003E844 File Offset: 0x0003CA44
		public void GoalHit(Pong.ESide side)
		{
			if (this.State != Pong.EState.Playing)
			{
				return;
			}
			if (side == Pong.ESide.Left)
			{
				int num = this.RightScore;
				this.RightScore = num + 1;
				if (this.onRightScore != null)
				{
					this.onRightScore.Invoke();
				}
			}
			else
			{
				int num = this.LeftScore;
				this.LeftScore = num + 1;
				if (this.onLeftScore != null)
				{
					this.onLeftScore.Invoke();
				}
			}
			this.LeftScoreLabel.text = this.LeftScore.ToString();
			this.RightScoreLabel.text = this.RightScore.ToString();
			this.Ball.RB.velocity = Vector3.zero;
			this.Ball.RB.isKinematic = true;
			this.State = Pong.EState.Ready;
			if (this.LeftScore >= this.GoalsToWin)
			{
				this.Win(Pong.ESide.Left);
				return;
			}
			if (this.RightScore >= this.GoalsToWin)
			{
				this.Win(Pong.ESide.Right);
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0003E930 File Offset: 0x0003CB30
		private void Win(Pong.ESide winner)
		{
			if (winner == Pong.ESide.Left)
			{
				this.WinnerLabel.text = "Player 1 Wins!";
				this.WinnerLabel.color = this.LeftPaddle.GetComponent<Image>().color;
				if (this.onLocalPlayerWin != null)
				{
					this.onLocalPlayerWin.Invoke();
				}
			}
			else
			{
				this.WinnerLabel.text = "Player 2 Wins!";
				this.WinnerLabel.color = this.RightPaddle.GetComponent<Image>().color;
			}
			this.State = Pong.EState.GameOver;
			if (this.onGameOver != null)
			{
				this.onGameOver.Invoke();
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0003E9C8 File Offset: 0x0003CBC8
		private void ResetBall()
		{
			this.Ball.RB.isKinematic = true;
			this.Ball.Rect.anchoredPosition = Vector2.zero;
			this.Ball.transform.localPosition = Vector3.zero;
			this.Ball.transform.localRotation = Quaternion.identity;
			this.Ball.RB.velocity = Vector3.zero;
			this.Ball.RB.isKinematic = false;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0003EA4C File Offset: 0x0003CC4C
		private void ServeBall()
		{
			this.ResetBall();
			this.Ball.RB.isKinematic = false;
			if (this.nextBallSide == Pong.ESide.Left)
			{
				Vector2 normalized = new Vector2(-1f, UnityEngine.Random.Range(-0.5f, 0.5f)).normalized;
				this.Ball.RB.AddRelativeForce(normalized * this.InitialVelocity, 2);
			}
			else
			{
				Vector2 normalized2 = new Vector2(1f, UnityEngine.Random.Range(-0.5f, 0.5f)).normalized;
				this.Ball.RB.AddRelativeForce(normalized2 * this.InitialVelocity, 2);
			}
			this.State = Pong.EState.Playing;
			this.nextBallSide = ((this.nextBallSide == Pong.ESide.Left) ? Pong.ESide.Right : Pong.ESide.Left);
			if (this.onServe != null)
			{
				this.onServe.Invoke();
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0003EB30 File Offset: 0x0003CD30
		private void ResetGame()
		{
			this.State = Pong.EState.Ready;
			this.LeftScore = 0;
			this.RightScore = 0;
			this.LeftScoreLabel.text = this.LeftScore.ToString();
			this.RightScoreLabel.text = this.RightScore.ToString();
			this.ResetBall();
			this.nextBallSide = Pong.ESide.Left;
			this.ballVelocity = Vector3.zero;
			if (this.onReset != null)
			{
				this.onReset.Invoke();
			}
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0003EBAF File Offset: 0x0003CDAF
		public void SetPaddleTargetY(Pong.ESide player, float y)
		{
			if (player == Pong.ESide.Left)
			{
				this.LeftPaddle.SetTargetY(y);
				return;
			}
			this.RightPaddle.SetTargetY(y);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0003EBCD File Offset: 0x0003CDCD
		public override void Resume()
		{
			base.Resume();
			this.Ball.RB.isKinematic = false;
			this.Ball.RB.velocity = this.ballVelocity;
		}

		// Token: 0x04000E75 RID: 3701
		public RectTransform Rect;

		// Token: 0x04000E76 RID: 3702
		public PongPaddle LeftPaddle;

		// Token: 0x04000E77 RID: 3703
		public PongPaddle RightPaddle;

		// Token: 0x04000E78 RID: 3704
		public PongBall Ball;

		// Token: 0x04000E79 RID: 3705
		public TextMeshProUGUI LeftScoreLabel;

		// Token: 0x04000E7A RID: 3706
		public TextMeshProUGUI RightScoreLabel;

		// Token: 0x04000E7B RID: 3707
		public TextMeshProUGUI WinnerLabel;

		// Token: 0x04000E7C RID: 3708
		[Header("Settings")]
		public float InitialVelocity = 0.8f;

		// Token: 0x04000E7D RID: 3709
		public float VelocityGainPerSecond = 0.05f;

		// Token: 0x04000E7E RID: 3710
		public float MaxVelocity = 2f;

		// Token: 0x04000E7F RID: 3711
		public int GoalsToWin = 10;

		// Token: 0x04000E80 RID: 3712
		[Header("AI")]
		public float ReactionTime = 0.1f;

		// Token: 0x04000E81 RID: 3713
		public float TargetRandomization = 10f;

		// Token: 0x04000E82 RID: 3714
		public float SpeedMultiplier = 0.5f;

		// Token: 0x04000E83 RID: 3715
		public UnityEvent onServe;

		// Token: 0x04000E84 RID: 3716
		public UnityEvent onLeftScore;

		// Token: 0x04000E85 RID: 3717
		public UnityEvent onRightScore;

		// Token: 0x04000E86 RID: 3718
		public UnityEvent onGameOver;

		// Token: 0x04000E87 RID: 3719
		public UnityEvent onLocalPlayerWin;

		// Token: 0x04000E88 RID: 3720
		public UnityEvent onReset;

		// Token: 0x04000E89 RID: 3721
		private Pong.ESide nextBallSide;

		// Token: 0x04000E8A RID: 3722
		private Vector3 ballVelocity = Vector3.zero;

		// Token: 0x04000E8B RID: 3723
		private float reactionTimer;

		// Token: 0x020002A5 RID: 677
		public enum EGameMode
		{
			// Token: 0x04000E8D RID: 3725
			SinglePlayer,
			// Token: 0x04000E8E RID: 3726
			MultiPlayer
		}

		// Token: 0x020002A6 RID: 678
		public enum ESide
		{
			// Token: 0x04000E90 RID: 3728
			Left,
			// Token: 0x04000E91 RID: 3729
			Right
		}

		// Token: 0x020002A7 RID: 679
		public enum EState
		{
			// Token: 0x04000E93 RID: 3731
			Ready,
			// Token: 0x04000E94 RID: 3732
			Playing,
			// Token: 0x04000E95 RID: 3733
			GameOver
		}
	}
}
