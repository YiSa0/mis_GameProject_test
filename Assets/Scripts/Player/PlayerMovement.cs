using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    void Start()
    {
        // 遊戲開始時，程式會自動在「自己身上」尋找這兩個組件並填入
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. 取得輸入
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. 關鍵判斷：只有在「有移動」時，才更新面向 (MoveX, MoveY)
        // movement.sqrMagnitude > 0 代表玩家正在按方向鍵
        if (movement.sqrMagnitude > 0)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
        }
        
        // 3. Speed 則「隨時」更新
        // 因為沒動時 Speed 變為 0，混合樹才會從 Walk 切換回 Idle 狀態
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // 物理移動，避免穿牆
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}