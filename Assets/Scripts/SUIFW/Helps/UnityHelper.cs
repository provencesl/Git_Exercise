/**
 *	Title: "SOAP" UI框架
 *
 *  Description:Unity帮助脚本
 *  提供程序用户一些常用的方法，方便程序员快速开发
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class UnityHelper : MonoBehaviour {

        /// <summary>
        /// 查找子节点对象
        /// 内部使用递归算法
        /// </summary>
        /// <returns>The the child node.</returns>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">查找的子对象名称</param>
        public static Transform FindTheChildNode(GameObject goParent, string childName)
        {
            Transform searchTrans = null;
            searchTrans = goParent.transform.Find(childName);
            if(searchTrans == null)
            {
                foreach(Transform trans in goParent.transform)
                {
                    searchTrans = FindTheChildNode(trans.gameObject, childName);
                    if(searchTrans != null)
                    {
                        return searchTrans;
                    }
                }
            }
            return searchTrans;
        }

        /// <summary>
        /// 查找子对象（节点）脚本
        /// </summary>
        /// <returns>The the child node componet scripts.</returns>
        /// <param name="goParent">Go parent.</param>
        /// <param name="childName">Child name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetTheChildNodeComponetScripts<T>(GameObject goParent, string childName) where T : Component
        {
            Transform searchTransformNode = null;
            searchTransformNode = FindTheChildNode(goParent, childName);
            if(searchTransformNode != null)
            {
                return searchTransformNode.gameObject.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 给子对象添加脚本
        /// </summary>
        /// <returns>The child node component.</returns>
        /// <param name="goParent">Go parent.</param>
        /// <param name="childName">Child name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T AddChildNodeComponent<T>(GameObject goParent, string childName) where T: Component
        {
            Transform searchTransform = null;
            //查找特定子节点
            searchTransform = FindTheChildNode(goParent, childName);
            //如果查找成功，则考虑如果有相同的脚本，先删除，再添加
            if(searchTransform != null)
            {
                T[] componetScriptsArray = searchTransform.GetComponents<T>();
                for(int i = 0; i < componetScriptsArray.Length; i++)
                {
                    if(componetScriptsArray[i] != null)
                    {
                        Destroy(componetScriptsArray[i]);//删除相同脚本
                    }
                }
                return searchTransform.gameObject.AddComponent<T>();//添加
            }
            else
            {
                return null;//查找失败，返回null
            }

        }
        /// <summary>
        /// 给子节点添加父对象
        /// </summary>
        /// <param name="parent">父对象的方位</param>
        /// <param name="child">子对象的方位.</param>
        public static void AddChildNodeToParentNode(Transform parent, Transform child)
        {
            child.SetParent(parent, false); //false 表示局部坐标
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }
    }
}
