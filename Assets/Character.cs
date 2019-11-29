using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Character : MonoBehaviour
    {
        CharacterController controller;
        public float speed = 4f;
        public float gravity = 8f;
        public float rotSpeed = 80;

        /*public float speed = 6.0f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;*/

        Vector3 movDir = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                movDir *= speed;
            }

            movDir.y -= gravity * Time.deltaTime;
            controller.Move(movDir * Time.deltaTime);
        }
    }
}
