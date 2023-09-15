using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCtr : MonoBehaviour
{
    enum State { Chic, Chicken }

    State state;

    [SerializeField] private Animator anim;
    [SerializeField] Collider collider;

    [SerializeField] float moveTime;
    public float speed;
    float hAxis;
    float vAxis;

    Vector3 moveVec;

    [SerializeField] float delay = 5.0f;


    void Update()
    {
        //Debug.LogError(Random.Range(-1, 2));

        if (delay > 0)
            delay -= Time.deltaTime;
        else
        {
            int random = Random.Range(0, 3);

            switch(random)
            {
                case 0:
                    {
                        anim.SetTrigger("Eat");
                        delay = 5.0f;
                    }
                    break;
                case 1:
                    {
                        anim.SetTrigger("Turn Head");
                        delay = 5.0f;
                    }
                    break;
                case 2:
                    {
                        hAxis = Random.Range(-1, 2);
                        vAxis = Random.Range(-1, 2);

                        if(hAxis == 0 && vAxis == 0)
                        {
                            anim.SetTrigger("Eat");
                            delay = 5.0f;
                        }
                        else
                        {
                            moveTime = Random.Range(2, 5);
                            anim.SetBool("Walk", true);
                            delay = 8.0f;
                        }

                    }
                    break;
            }

            
        }

        // 이동 AI
        if (moveTime > 0)
        {
            moveTime -= Time.deltaTime;

            moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
            transform.position += moveVec * speed * Time.deltaTime;
        
            transform.LookAt(transform.position + moveVec);
        }
        else
            anim.SetBool("Walk", false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (moveTime > 0)
        {
            hAxis *= -1;
            vAxis *= -1;
        }
    }

}
