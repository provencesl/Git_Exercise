/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;

namespace DemoProject
{
    public class StartProject : MonoBehaviour {

        void Start() {
            //Log.Write(GetType() + "/start()"); //日志
            UIManager.GetInstance().ShowUIForms("LogonUIForm");

        }
    
    }
}
