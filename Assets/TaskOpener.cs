using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskOpener : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Panel;
    public GameObject smtg;
    [SerializeField] TextMeshProUGUI toggleText;
    [SerializeField] TextMeshProUGUI toggleText2;
    public void OpenPanel()
    {
        Animator animator = Panel.GetComponent<Animator>();

        bool isOpen = animator.GetBool("open");
        
        animator.SetBool("open", !isOpen);

        if (isOpen)
        {
            toggleText.text = "New Destination";
            toggleText2.text = "";
        }
        else
        {
            toggleText2.text = "Rossebaendiger";
            toggleText.text = "Close";
        }



        Debug.Log("pressed");

    }

}
