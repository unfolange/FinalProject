using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    [HideInInspector] public bool jumping = false;
    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool recoilingX, recoilingY;
    [HideInInspector] public bool lookingRight;
    [HideInInspector] public bool invincible;

    //Variables para desbloquear habiliades
    public enum SkillType
    {
        Attack,
        Dash,
        Jump,       
        DoubleJump,        
        TripleJump
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

            case SkillType.TripleJump:
                UnlockTripleJump();
                break;

            default:
                Debug.LogWarning("Unknown ability");
                break;
        }

        UpdateSkills();
    }


    void UnlockAttack()
    {
        PlayerPrefs.SetInt("Attack", 1);
        PlayerPrefs.Save();
        Debug.Log("Attack unlocked!");
    }

    void UnlockDash()
    {
        PlayerPrefs.SetInt("Dash", 1);
        PlayerPrefs.Save();
        Debug.Log("Dash unlocked!");
    }

    void UnlockJump()
    {
        PlayerPrefs.SetInt("MaxJumps", 1);
        PlayerPrefs.Save();
        Debug.Log("Jump unlocked!");
    }

    void UnlockDoubleJump()
    {
        PlayerPrefs.SetInt("MaxJumps", 2);
        PlayerPrefs.Save();
        Debug.Log("Double Jump unlocked!");
    }    

    void UnlockTripleJump()
    {
        PlayerPrefs.SetInt("MaxJumps", 3);
        PlayerPrefs.Save();
        Debug.Log("Triple Jump unlocked!");
    }

    void UpdateSkills()
    {
        GetComponent<PlayerController>().UpdateSkills();
    }
}
