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
using UnityEngine.UI;

namespace DemoProject
{
    public class LogonUIForm : BaseUIForm {

        public Text TxtLogonName;   //登陆名称
        public Text TextLogonNameByBtn;     //登陆按钮名称

        public void Awake()
        {
            //定义本窗体的性质(默认数值)
            base.CurrentUIType.UIForms_Type = UIFormType.Normal;
            base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;
            base.CurrentUIType.UIForms_LuccencyType = UIFormLucencyType.Lucency;

            //给按钮注册事件
            //RegisterButtonObjectEvent("Btn_OK", LogonSys);
            //用Lamda表达式
            RegisterButtonObjectEvent("Btn_OK",
                p=> OpenUIForm(ProConsts.SELECT_HERO_FORMS)
            );

        }
        //public void LogonSys(GameObject go)
        //{
        //    //前后胎账户密码验证
        //    //成功则进入下一个窗体
        //    OpenUIForm(ProConsts.SELECT_HERO_FORMS);
        //}

        public void Start()
        {
            if (TxtLogonName)
            {
                TxtLogonName.text = Show("LogonSystem");
            }
            if (TextLogonNameByBtn)
            {
                TextLogonNameByBtn.text = Show("LogonSystem");
            }
        }
    }
}
