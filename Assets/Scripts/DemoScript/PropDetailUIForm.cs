/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SUIFW
{
    public class PropDetailUIForm : BaseUIForm {

        public Text TxtName;    //窗体显示名称
        private void Awake()
        {
            //定义本窗体的性质(默认数值)
            base.CurrentUIType.UIForms_Type = UIFormType.Popup;
            base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.ReverseChange;
            base.CurrentUIType.UIForms_LuccencyType = UIFormLucencyType.Translucency;
            //按钮的注册
            RegisterButtonObjectEvent("PropClose", (go) => CloseUIForm());//使用lamda表达式写法
            /* 接受信息 */
            //ReceiveMessage.
            ReceiveMessage("Props",
                p =>
                {
                    if (TxtName)
                    {
                        //TxtName.text = p.Values.ToString();//单个字符串
                        string[] strArr = p.Values as string[];//多个
                        TxtName.text = strArr[0];
                        print("测试时道具的详细信息" + strArr[1]);
                    }
                }
            );

        }
    }
}
