using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour {

    //    private Rigidbody2D _rigid;
    // Use this for initialization
    public float walkspeed = 1.5f;
    float x_input;
    float y_input;
    float move_anim;

   [SerializeField]
    private bool _reset_jump = false;

    [SerializeField]
    private float char_speed = 2.0f;

    private player_animation _Player_anim;
    public SpriteRenderer _Player_sprite;
    public Animator n_animator;

    //jump
    Rigidbody2D rigidbody_j;
    public float axisY;
    public float pre_y;
    public bool isJumping;
    public float jumpforce = 300.0f;
    float temp_gap_y;
    Vector3 jump_stop;
    public bool isJumpkick=false;

    Vector3 movement;

    public float shake_speed; //how fast it shakes
    public float shake_amount;
    public float shake_time;

    public int shake=0;
    public bool shaking;
    public int punch=0;
    public int hit_combo = 0;
    public attack hit_attack;


    public bool attack_pressed=false;
    public float combo_time = 0.0f;

    void Awake()
    {
       rigidbody_j = GetComponent<Rigidbody2D>();
       rigidbody_j.Sleep();

    }
    void Start() {
        //       _rigid = GetComponent<Rigidbody2D>();
        _Player_anim = GetComponent<player_animation>();
        n_animator = GetComponentInChildren<Animator>();

        _Player_sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hit_attack = GetComponentInChildren<attack>();
        Movement();
        float x_input_temp = Mathf.Abs(x_input);
        float y_input_temp = Mathf.Abs(y_input);
        
        if (x_input_temp >= y_input_temp) { 
            move_anim =Mathf.Abs(x_input_temp);
        }
        else if (y_input_temp > x_input_temp)
        {
            move_anim = Mathf.Abs(y_input_temp);
        }

         n_animator.SetFloat("Speed", move_anim);


        if (Input.GetKeyDown("q") && !isJumping)
        {

            attacking_combo();
            
          
        }
        else if(Input.GetKeyDown("q")&& isJumping)
        {
            n_animator.SetInteger("Jump",3);
            Debug.Log("jump_kick");
        }
        hit_combo_count();

        if (punch > 0 && n_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f && hit_attack.enemy_hit)
        {

            //shake = true;
            if(shake==0)
            shake = 1;
        }
        else if (hit_attack.enemy_hit && isJumping)
        {
            if (shake == 0) { 
            shake = 1;
                jump_stop = transform.position;
            }
        }

        if (shake==1)
        {
            
           if(isJumpkick==false)
            body_shake();
        }

        
        if (shake >= 1)
        {
            combo_time += Time.deltaTime;
            if(combo_time>0.18f)
            {
                combo_time = 0.0f;
                shake = 0;
            }
        }

    }
    void FixedUpdate()
    {
//        main_char.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
        if (punch==0) { 
        movement = new Vector3(x_input * walkspeed, y_input * walkspeed, 0.0f);
        transform.position = transform.position + movement * Time.deltaTime;
        }
        if (transform.position.y >= 1.79f)
        {
            

        }
        else if(transform.position.y < 1.79f)
        {
            transform.position = new Vector3(transform.position.x, 1.79f, transform.position.z);

        }

        if (transform.position.x >= 0.5f)
        {

        }
        else if (transform.position.x < 0.5f)
        {
            transform.position = new Vector3(0.5f,transform.position.y, transform.position.z);

        }
        

    }

    void Movement () {

       
        if (!isJumping) { 
        y_input = Input.GetAxis("Vertical");
        x_input = Input.GetAxis("Horizontal");
        }
        else if(isJumping)
        {
            y_input = 0.0f;
        }
        if (x_input<0)
        {
            //  _Player_sprite.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (x_input > 0)
        {
            //_Player_sprite.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(isJumping == true && temp_gap_y < 0.0f && transform.position.y <= axisY + 1.3f)
        {
            n_animator.SetInteger("Jump", 2);
        }
        if (isJumping == true && transform.position.y <= axisY+0.01f )
        {
            OnLanding();
 
        }

        if (isJumping == true) {
            temp_gap_y = transform.position.y - pre_y;
            pre_y = transform.position.y;

        }
        if (Input.GetKey(KeyCode.Space)&& !isJumping)
        {
            axisY = transform.position.y;
            isJumping = true;
            
            rigidbody_j.gravityScale = 1.5f;
            rigidbody_j.WakeUp();
            rigidbody_j.AddForce(new Vector2(transform.position.x+100.0f*x_input, jumpforce));

            n_animator.SetInteger("Jump", 1);
        }
    

    }
    void OnLanding()
    {
        isJumping = false;
        rigidbody_j.gravityScale = 0.0f;
        rigidbody_j.Sleep();
        transform.position = new Vector3(transform.position.x, axisY, transform.position.z);
        n_animator.SetInteger("Jump", 0);
        temp_gap_y = 0;
        pre_y = 0;

    }
    bool IsGrounded()
    {

        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << 8);

        if (hitinfo.collider != null)
        {
            if(_reset_jump==false)
           return true;

        }
        return false;
    }
    IEnumerator Reset_Jump_R()
    {
        _reset_jump = true;
        yield return new WaitForSeconds(0.1f);
        _reset_jump = false;
    }

    void body_shake()
    {
    
           shake_time += Time.deltaTime;
//        if (n_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)

            //            if (shake_time < 0.12f && shake_time > 0.05f)
            if (shake_time<0.11f)
        {
            if(isJumping)
            transform.position = new Vector3(jump_stop.x, jump_stop.y, transform.position.z);

                n_animator.speed = 0.0f;
            if (!shaking)
                {
                    transform.Translate(0.05f, 0.0f, 0.0f);
                    shaking = true;
                }
                else if (shaking)
                {
                    transform.Translate(-0.05f, 0.0f, 0.0f);
                    shaking = false;
                }
                /*
                if (isJumpkick)
                {
                    transform.position = new Vector3(transform.position.x, jumpkickstop, transform.position.z);
                }*/
            }
           else if (shake_time >= 0.11f)
//        else if(n_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
                shake_time = 0.0f;
                n_animator.speed = 1.0f;
                shake = 2;
                shaking = false;
            if(isJumping)
            isJumpkick = true;
            //   n_animator.SetInteger("Attack1", 0);
            // hit_attack.enemy_hit = false;


            /*jumpkickstop = 0.0f;
             isJumpkick = false;*/
        }
        
    }
    void attacking_combo()
    {
       // Debug.Log("attacking combo");

        hit_combo++;
        if (hit_combo >= 2)
            hit_combo = 2;

        if (n_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && n_animator.GetCurrentAnimatorStateInfo(0).IsName("attack1") && !hit_attack.enemy_hit)
        {
            n_animator.SetInteger("Attack1", 0);
            punch = 0;
        }
        if (punch == 0 && !n_animator.IsInTransition(0))
        {
            punch = 1;
            n_animator.SetInteger("Attack1", 1);
        }

        else if (punch == 2 && !n_animator.IsInTransition(0))
       {
            n_animator.SetInteger("Attack1", 2);

        }
        else if (punch == 3 && !n_animator.IsInTransition(0))
       {
            n_animator.SetInteger("Attack1", 3);

        }
        else if (punch == 4 && !n_animator.IsInTransition(0))
       {
            n_animator.SetInteger("Attack1", 4);
      
        }
        

    }

    void hit_combo_count()
    {

        if (n_animator.GetCurrentAnimatorStateInfo(0).IsName("attack1") && n_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !hit_attack.enemy_hit)
        {
            //attack_pressed = false;
            // combo_time = 0.0f;
            punch = 0;
            n_animator.SetInteger("Attack1", 0);
            hit_attack.enemy_hit = false;
        }


        if (punch == 1 && n_animator.GetCurrentAnimatorStateInfo(0).IsName("attack1")&& hit_attack.enemy_hit)
        {
            punch = 2;

        }
        else if (punch == 2 && n_animator.GetCurrentAnimatorStateInfo(0).IsName("attack2")&& hit_attack.enemy_hit)
        {
            punch = 3;

        }
        else if (punch == 3 && n_animator.GetCurrentAnimatorStateInfo(0).IsName("kick")  && hit_attack.enemy_hit)
        {
            punch = 4;

        }

        if (n_animator.GetCurrentAnimatorStateInfo(0).IsName("uppercut1") && n_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            punch = 0;
            n_animator.SetInteger("Attack1", 0);
        }
        if (n_animator.GetCurrentAnimatorStateInfo(0).IsName("idle")&& punch>1)
        {
            punch = 0;
            n_animator.SetInteger("Attack1", 0);
          
        }
        else if(n_animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && isJumpkick)
        {
            isJumpkick = false;
        }

    }

}
