using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float moveSpeed;
	[SerializeField] private float jumpPower;
	private Rigidbody2D rigidbody2d;
	private float speedVx = 0.0f;
	private float dir;
	private float basScaleX = 1.0f;
	private bool jumped = false;
	private bool grounded = false;
	private bool groundedPrev = false;
	private float jumpStartTime = 0.0f;
	private float jumpForceTime = 1.0f;
	private bool breakEnabled = true;
    private float groundFriction = 0.0f;
	Transform[] groundCheckTrfm;
	
	void Awake(){
		rigidbody2d = GetComponent<Rigidbody2D>();

		dir = (transform.localScale.x > 0.0f) ? 1 : -1;
        basScaleX = Mathf.Abs(transform.localScale.x * dir);
        transform.localScale = new Vector3(basScaleX, transform.localScale.y, transform.localScale.z);
	}

	void Start () {
		
	}
	
	void Update () {
        // 矢印の左右の入力検出
        //float joyMv = Input.GetAxis("Horizontal");
        float vx = 0.0f;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            vx -= 1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            vx += 1.0f;
        }
        ActionMove(vx);

        if (Input.GetKeyDown(KeyCode.Space)) {
            ActionJump();
        }
	}

	void FixedUpdate(){
		// 接地チェック
		//GroundCheck();

		//移動停止処理
        if (breakEnabled) {
            speedVx *= groundFriction;
        }

		// Playerの向きの更新
		transform.localScale = new Vector3(basScaleX * dir, transform.localScale.y, transform.localScale.z);

		// カメラを強制的に追尾させる
        Camera.main.transform.position = transform.position - Vector3.forward;

		// Player
		rigidbody2d.velocity = new Vector2(speedVx, rigidbody2d.velocity.y);
	}

	private void GroundCheck(){
		// 地面チェック
        groundedPrev = grounded;
        grounded = false;
        Collider2D[][] groundCheckCollider = new Collider2D[groundCheckTrfm.Length][];

		for (int i = 0; i < groundCheckCollider.Length; i++){
			groundCheckCollider[i] = Physics2D.OverlapPointAll(groundCheckTrfm[i].position);
		}

        foreach (Collider2D[] groundCheckList in groundCheckCollider) {
            if (groundCheckList != null) {
                foreach (Collider2D groundCheck in groundCheckList) {
                    if (groundCheck != null) {
                        if (!groundCheck.isTrigger) {
                            grounded = true;
                        }
                    }
                }
            }
        }

		if (jumped) {
            if((grounded && !groundedPrev) ||(grounded && Time.fixedTime > jumpStartTime + jumpForceTime)) {
                jumped = false;
            }
        }
	}

	public void ActionMove(float n) {
        float dirOld = dir;
        breakEnabled = false;

        if(n != 0.0f) {
            dir = Mathf.Sign(n);
            speedVx = moveSpeed * n;
        } else {
            breakEnabled = true;
        }

        if(dirOld != dir) {
            breakEnabled = true;
        }
    }

	public void ActionJump() {
        //if (!grounded) return;
        jumped = true;
        jumpStartTime = Time.fixedTime;
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpPower);
    }
}
