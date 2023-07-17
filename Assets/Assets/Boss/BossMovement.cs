using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    //----------------Moving------------------
    private bool FacingRight;
    private bool CanMove;
    [SerializeField] private float runspeed = 5f;
    //----------------------------------------

    //--------Pulse Ability--------
    public bool Pulsing;
    [SerializeField] private float pulsePower = 54f;
    [SerializeField] private Rigidbody2D playerrb;
    //-----------------------------

    //---------------- Attack1 Ability -----------------
    private bool Attacking1;   
    [SerializeField] private float attack1range = 4.5f;
    private GameObject Boss;
    private GameObject attack1object;
    private PolygonCollider2D attack1coll;
    // -------------------------------------------------

    //------------------ SpinAttack Ability -------------------
    private bool Spining;
    [SerializeField] private float spinPower = 26f;
    private GameObject spinobject;
    private PolygonCollider2D spincoll;
    //----------------------------------------------------------


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //----------------Moving------------------
        CanMove = true;
        FacingRight = false;
        //----------------------------------------

        //--------Pulse Ability--------
        Pulsing = false;
        //-----------------------------

        //---------------- Attack1 Ability -----------------
        Attacking1 = false;
        Boss = GameObject.Find("Boss");
        attack1object = Boss.transform.GetChild(0).gameObject;
        attack1coll = attack1object.GetComponent<PolygonCollider2D>();
        attack1coll.enabled = false;
        //--------------------------------------------------

        //------------------ SpinAttack Ability -------------------
        Spining = false;
        Boss = GameObject.Find("Boss");
        spinobject = Boss.transform.GetChild(1).gameObject;
        spincoll = spinobject.GetComponent<PolygonCollider2D>();
        spincoll.enabled = false;
        //----------------------------------------------------------
    }


    void Update()
    {
        if (Vector2.Distance(player.position, rb.position) > attack1range)
        {
            CanMove = true;
        }
        else
        {
            CanMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (Spining) {return;}
        if (Attacking1) {return;}

        LookAtPlayer();

        if (CanMove)
        {
            Walk();
        }
        else
        {
            StartCoroutine(Attack1());
        }

    }

    private void Walk()
    {
        anim.SetBool("Walking", true);
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, runspeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    //---------------- Attack1 Ability -----------------
    private IEnumerator Attack1()
    {
        Attacking1 = true;
        anim.SetBool("Walking", false);        
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.8f);
        Attacking1 = false;
    }  
    void activateatack1Collider(){attack1coll.enabled = true;}void deactivateatack1Collider(){attack1coll.enabled = false;}void quake(){playerrb.velocity = new Vector2(0f,transform.up.y * 5f);}
    //--------------------------------------------------

    //------------------ Pulse Ability -------------------
    private IEnumerator Pulse()
    {
        Pulsing = true;
        anim.SetTrigger("Pulse");
        if (FacingRight)
        {
            playerrb.velocity = new Vector2(1 * pulsePower, rb.velocity.y);
        }
        else
        {
            playerrb.velocity = new Vector2(-1 * pulsePower, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.6f);
        Pulsing = false;
    }
    //----------------------------------------------------

    //------------------ SpinAttack Ability -------------------
    private IEnumerator Spinattack()
    {
        Spining = true;
        anim.SetTrigger("Spinattack");
        if (FacingRight)
        {
            rb.velocity = new Vector2(1 * spinPower, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-1 * spinPower, rb.velocity.y);
        }
        yield return new WaitForSeconds(0.3f);
        Spining = false;
    }
    void activatespinatackCollider(){spincoll.enabled = true;} void deactivatespinatackCollider() {spincoll.enabled = false;}
    //----------------------------------------------------------

    private void flip()
    {
        transform.Rotate(0f, 180f, 0f);
        FacingRight = !FacingRight;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z = -1f;
        if (transform.position.x > player.position.x && FacingRight && !Attacking1)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = false;
        }
        else if (transform.position.x < player.position.x && !FacingRight && !Attacking1)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            FacingRight = true;
        }
    }

}
