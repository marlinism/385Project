using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopcornEnemyManager : Enemy
{
    public Animator animator;
    public ItemDropper itemDropper;

    public float attackSpeed;
    public float attackDuration;

    public bool attacking = false;
    public string direction = " "; 

    public float attackCoolDown;

    private Vector2 attackDirection;

    private float nextAction = 0f;
    
    private float attackDistance = 2f;
    private float distance = 0f;
    private bool directionLock = false;

    //private bool isAlive = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            return;
        }

        distance = DistanceFromPlayer();
        updateDirection();

        //update sprite 
        Vector3 characterScale = transform.localScale;
        if(direction.Equals("right") && !directionLock){
            characterScale.x = -(Mathf.Abs(transform.localScale.x));
        }
        if(direction.Equals("left") && !directionLock){
            characterScale.x = Mathf.Abs(transform.localScale.x);
        }
        transform.localScale = characterScale;

        //Check attack state
        if(attacking == false){
        //update movement
            if(distance > attackDistance){
                //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                MoveTowardsPoint(Player.transform.position);

            }else{
                StopMovement();

                //if cooldown time if over then attack
                if(Time.time > nextAction){
                    attackDirection = Player.transform.position - transform.position;
                    attacking = true;
                    nextAction = Time.time + attackDuration;
                    rb.velocity =  attackDirection * attackSpeed;
                    directionLock = true;
                    animator.SetBool("attacking", true);
                }
            }
        }else{
            //stop attacking if attack duration is over
            if (Time.time > nextAction){
                attacking = false;
                directionLock = false;
                animator.SetBool("attacking", false);
                rb.velocity = new Vector2(0, 0); 
                nextAction = Time.time + attackCoolDown;
            }
        }
        
    }

    // Update the direction string to indicate whatch direction we are primarily moving
    void updateDirection(){
        Vector2 currDirection = DirectionTowardsPlayer();
        float x = currDirection.x;
        float y = currDirection.y;

        if(Mathf.Abs(x) >= Mathf.Abs(y) ){
            animator.SetInteger("direction", 1);
            if(x > 0){
                direction = "right";
            }else{
                direction = "left";
            }

        }else{
            if(y > 0){
                animator.SetInteger("direction", 3);
                direction = "up";
            }else{
                animator.SetInteger("direction", 2);
                direction = "down";
            }
        }

    }

    // Damagable method implementations
    public override void Kill()
    {
        if (itemDropper != null)
        {
            itemDropper.DropItems();
        }

        // Stub, add death animation
        Destroy(gameObject);
    }
}
