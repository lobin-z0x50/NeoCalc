﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public class CalcSvcMoriguchi : ICalcSvcEx
    {
        /// <summary>
        /// サービス名の列挙体
        /// </summary>
        public static Dictionary<int, string> SvcName = new Dictionary<int, string>
            {
                { 99,"CalcClass"},
                { 21,"OmikujiClass"},
                { 22,"StockClass"},
            };

        /// <summary>
        /// 指定サービス番号
        /// </summary>
        public static int SvcNo;

        public class ContextMoriguchi : ICalcContext
        {
            /// <summary>
            /// メインディスプレイに表示する文字列
            /// </summary>
            public string DisplayText => FaCtx?.DisplayText;
            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public virtual string SubDisplayText => FaCtx?.SubDisplayText;

            /// <summary>
            /// サービスの入れ物
            /// </summary>
            public ISubSvc FaSvc;
            public ISubContext FaCtx;
        }

        public virtual ICalcContext CreateContext() => new ContextMoriguchi();

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1) return "%";
            else if (num == 2) return "おみくじ";
            else if (num == 3) return "株価取得";
            return null;
        }

        /// <summary>
        /// まずここから始まる
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            //ctx0(ディスプレイとサブディスプレイ)
            var ctx = ctx0 as ContextMoriguchi;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            //defaultでは電卓モード
            if (ctx.FaSvc == null)
            {
                SvcNo = 99;
                MakeFactory(ctx);
            }
            else if ((int)btn >= 21)
            {
                //サービスの切り替え時
                SvcNo = (int)btn;
                MakeFactory(ctx);
            }

            //サービスメソッド実行
            var ret = ctx.FaSvc.OnClick(ctx.FaCtx, btn);

            //サービスインスタンス破棄
            if (!ret)
            {
                ctx.FaSvc = null;
            }
        }

        /// <summary>
        /// ファクトリーによるサービス製造
        /// </summary>
        private void MakeFactory(ContextMoriguchi prevCtx)
        {
            var c = SvcFactory.CreateContext();
            var s = SvcFactory.CreateService();
            s.Init(c, prevCtx);
            prevCtx.FaCtx = c;
            prevCtx.FaSvc = s;
        }
    }
}
