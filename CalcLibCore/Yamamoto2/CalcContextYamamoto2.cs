﻿using System;
using CalcLib.Yamamoto2.Executors;

namespace CalcLib.Yamamoto2
{
	internal class CalcContextYamamoto2 : CalcContext
	{
		public enum StateEnum {
            Init,
            InputedNumber,
            InputedOperator,
            InputedEqual,
        }
        public StateEnum State { get; set; }

		public decimal? left;     // 左辺を入れておく
        public List<string> subDisplayItems;  // サブディスプレイの項目を保持する
		public OperatorButtonExecutor? ope;  // オペレータを入れておく

        public override string SubDisplayText
        {
            get
            {
                return string.Join(" ", subDisplayItems);
            }
        }

        public override string DisplayText
        {
            get
            {
                if(string.IsNullOrWhiteSpace(base.DisplayText))
                {
                    // DisplayTextが空かもしれないことを考慮
                    return base.DisplayText;
                }

                // 整数部分のカンマ編集
                var splitResult = base.DisplayText.Split(".");
                var result = decimal.Parse(splitResult[0]).ToString("#,0");
                if(splitResult.Length > 1)
                {
                    return $"{result}.{splitResult[1]}";
                }
                return $"{result}";
            }
        }

        public CalcContextYamamoto2()
		{
            State = StateEnum.Init;
            subDisplayItems = new List<string>();
		}

        /// <summary>
        /// DisplayTextに対してdecimal型からセットする
        /// </summary>
        /// <remarks>
        /// decimal型に対して、ToStringすると、末尾の0がある状態で文字列になるため、
        /// そうならないようにセッターを用意した。
        /// </remarks>
        /// <param name="value"></param>
        public void SetDisplayText(decimal value)
        {
            // [NOTE]
            // ToStringするだけだと、少数部分の末尾の0が出てしまうため、フォーマットを指定する
            // https://dobon.net/vb/dotnet/string/inttostring.html#section3
            // 0: ゼロプレースホルダー 対応する数字で0を置き換える。対応する数字がない場合は0が表示される
            // #: 桁プレースホルダー 対応する数字で#を置き換える。対応する数字がない場合は表示されない
            var s = value.ToString("0.#############");
            DisplayText = s;
        }

        public void Reset()
        {
            SetDisplayText(0);
            subDisplayItems = new List<string>();
            left = null;
            ope = null;
            State = StateEnum.Init;
        }
    }
}

