using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    [HideInInspector] public bool jumping = false;
    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool recoilingX, recoilingY;
    [HideInInspector] public bool lookingRight;
    [HideInInspector] public bool invincible;

    public bool canJump = false;
    public bool canAttack = false;    
    public bool canDobleJump = false;
    public bool canDash = false;

    //Variables para desbloquear habiliades
    public enum SkillType
    {
        Attack,
        Dash,
        Jump,       
        DoubleJump
    }

    public void UnlockSkill(SkillType ability)
    {
        switch (ability)
        {
            case SkillType.Attack:
                UnlockAttack();
                break;

            case SkillType.Dash:
                UnlockDash();
                break;

            case SkillType.Jump:
                UnlockJump();
                break;                  

            case SkillType.DoubleJump:
                UnlockDoubleJump();
                break;

            default:
                Debug.LogWarning("Unknown ability");
                break;
        }

        UpdateSkills();
    }


    void UnlockAttack()
    {
        canAttack = true;        
    }

    void UnlockDash()
    {
        canDash = true;
    }

    void UnlockJump()
    {
        canJump = true;
    }

    void UnlockDoubleJump()
    {
        canDobleJump = true;
    }

    void UpdateSkills()
    {
        GetComponent<PlayerController>().UpdateSkills();
    }
}
