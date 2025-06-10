using System.Collections;
using UnityEngine;

public class BossSkillState : StateMachineBehaviour
{
    [SerializeField] private GameObject skill;    
    [SerializeField] private float offsetY;
    public float delay = 0.2f;

    private Enemy_Boss boss;
    private Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Enemy_Boss>();
        player = boss.player.transform;

        boss.LookPlayer();       

        boss.StartCoroutine(SpawnSkillAfterDelay());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private IEnumerator SpawnSkillAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        Vector2 appearPosition = new Vector2(player.position.x, player.position.y + offsetY);
        Instantiate(skill, appearPosition, Quaternion.identity);
    }
}
