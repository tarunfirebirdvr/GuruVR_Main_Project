using UnityEngine;

public class FindOnClick : MonoBehaviour
{
   
    public void set()
    {
        Animator animator = FindAnyObjectByType<Animator>();
    

       
            PlayerPrefs.SetString("AvatarID", animator.gameObject.name);
            Debug.Log("Found Animator on GameObject: " + animator.gameObject.name);
        
    }
}
