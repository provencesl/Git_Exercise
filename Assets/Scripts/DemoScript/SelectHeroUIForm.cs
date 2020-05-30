/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoProject;
namespace SUIFW
{
    public class SelectHeroUIForm : BaseUIForm
    {
        private void Awake()
        {
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;
            //注册事件
            //注册进入主城事件
            RegisterButtonObjectEvent("BtnConfirm", 
                p => {
                    OpenUIForm("MainCityUIForm");
                    OpenUIForm("HeroInfoUIForm");

                });
            //注册返回到登陆窗体UI
            RegisterButtonObjectEvent("BtnClose",(go) => CloseUIForm());//使用lamda表达式写法
        }

        public void EnterMainCityUIForm(GameObject go)
        {
            print("进入主城");
        }

    }
}
