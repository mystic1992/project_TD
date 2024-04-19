/********************************************************************
  filename: LayerMgr
  author: Mario Chen
  purpose:  游戏层级管理类型
  Tips:
*********************************************************************/
namespace Game {
    using UnityEngine;

    public class LayerMgr {
        public static int Enemy = LayerMask.NameToLayer("Enemy");
        public static int Ui = LayerMask.NameToLayer("UI");

        public static int BulletTrigger = ((((int)1) << Enemy));



    }
}
