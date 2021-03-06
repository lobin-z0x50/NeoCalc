﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CalcLibTest
{
    [TestClass]
    public class CalcSvcTest2
    {
        [TestMethod]
        public void 小数点以下の有効桁数は13桁でそれ以下は四捨五入されること()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 0.0000000000005 (小数点以下13桁)
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            Enumerable.Range(1, 12).ToList().ForEach(i => svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0));
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("0.0000000000005", ctx.DisplayText);

            // × 0.7
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn7);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // 0.00000000000035 であるが、有効桁数以下は四捨五入される
            Assert.AreEqual("0.0000000000004", ctx.DisplayText);

            // × 1.1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            // 0.00000000000044 であるが、有効桁数以下は四捨五入される
            Assert.AreEqual("0.0000000000004", ctx.DisplayText);

        }

        [TestMethod]
        public void 小数の2進数演算誤差のテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 30000
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            // ×
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);

            // 0.0021
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            // -
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);

            // 62
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn6);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            // ×
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);

            // 10
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("10", ctx.DisplayText);
        }

        [TestMethod]
        public void サブディスプレイのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2.2 - 3333

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMultiple);

            Assert.AreEqual("1 + 2.2 ×", ctx.SubDisplayText);
            Assert.AreEqual("3.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnMinus);

            Assert.AreEqual("1 + 2.2 -", ctx.SubDisplayText);
            Assert.AreEqual("3.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);

            Assert.AreEqual("1 + 2.2 -", ctx.SubDisplayText);
            Assert.AreEqual("3,333", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("-3,329.8", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("-3329.8 +", ctx.SubDisplayText);
            Assert.AreEqual("-3,329.8", ctx.DisplayText);
        }

        [TestMethod]
        public void BackSpaceのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2 + <BS> 44 <BS> 5 =

            // 1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            // 2
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2", ctx.DisplayText);

            // + 
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("3", ctx.DisplayText);

            // <BS>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnBS);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("3", $"{ctx.DisplayText}");

            // 44
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("44", ctx.DisplayText);

            // <BS>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnBS);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("4", $"{ctx.DisplayText}");

            // 5
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("45", ctx.DisplayText);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("48", ctx.DisplayText);
        }

        [TestMethod]
        public void ClearEndのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2 + <CE> 44 <CE> 5 =

            // 1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            // 2
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2", ctx.DisplayText);

            // + 
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("3", ctx.DisplayText);

            // <CE>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnClearEnd);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // 44
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("44", ctx.DisplayText);

            // <CE>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnClearEnd);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // 5
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("5", ctx.DisplayText);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("8", ctx.DisplayText);
        }

        [TestMethod]
        public void Clearのテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            // 1 + 2 + 4 <C> 5 =

            // 1
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("1", ctx.DisplayText);

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("1", ctx.DisplayText);

            // 2
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("1 +", ctx.SubDisplayText);
            Assert.AreEqual("2", ctx.DisplayText);

            // + 
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("3", ctx.DisplayText);

            // 4
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn4);

            Assert.AreEqual("1 + 2 +", ctx.SubDisplayText);
            Assert.AreEqual("4", ctx.DisplayText);

            // <C>
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnClear);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // +
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            Assert.AreEqual("0 +", $"{ctx.SubDisplayText}");
            Assert.AreEqual("0", $"{ctx.DisplayText}");

            // 5
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn5);

            Assert.AreEqual("0 +", ctx.SubDisplayText);
            Assert.AreEqual("5", ctx.DisplayText);

            // =
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("", $"{ctx.SubDisplayText}");
            Assert.AreEqual("5", ctx.DisplayText);
        }

        [TestMethod]
        public void DisplayTextのカンマ編集のテスト()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();


            // 100
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("100", ctx.DisplayText);

            // 1000
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("1,000", ctx.DisplayText);

            // BS -> 100
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnBS);

            Assert.AreEqual("100", ctx.DisplayText);

            // 1000
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("1,000", ctx.DisplayText);

            // 1000000
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("1,000,000", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("1,000,000.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("1,000,000.1", ctx.DisplayText);
        }


        [TestMethod]
        public void 何もない状態で小数点を押した場合は小数入力が可能となること()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("0.1", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("0.2", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("0.3", ctx.DisplayText);
        }

        [TestMethod]
        public void 小数点以下のゼロの表示()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("0.0", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);

            Assert.AreEqual("0.01", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("0.010", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);

            Assert.AreEqual("0.0102", ctx.DisplayText);
        }

        [TestMethod]
        public void 小数点の連続押しは無視されることとイコールを押すと丸められること()
        {
            var ctx = CalcLib.Factory.CreateContext();
            var svc = CalcLib.Factory.CreateService();

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);

            Assert.AreEqual("0.0", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnDot);

            Assert.AreEqual("0.0", ctx.DisplayText);

            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);

            Assert.AreEqual("0", ctx.DisplayText);
        }
    }
}
