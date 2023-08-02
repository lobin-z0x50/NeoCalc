﻿using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;
using CalcLibCore.Tomida.Commands.Omikuji;
using CalcLibCore.Tomida.Extensions;

namespace CalcLibCore.Tomida
{
	public class OmikujiContext: ICalcContextEx
	{
        private static string[] _SUITS_DEFAULT = new string[] { "大吉", "中吉", "小吉", "凶" };

        private string[] _suits;

        private int? selectedIndex;

		public OmikujiContext()
		{
            Factory = new OmikujiCommandFactory();
            _suits = _SUITS_DEFAULT.Shuffle();
		}

        public string DisplayText => DisplayTextImpl();

        private string DisplayTextImpl()
        {
            string str = string.Empty;
            switch (GetState())
            {
                case OmikujiState.BeforeLotted:
                    str = "[1 ] [2 ] [3 ] [4 ]";
                    break;
                case OmikujiState.AfterLotted:
                    // TODO: おみくじ引いたあとのディスプレイ
                    break;
            }
            return str;
        }

        public string SubDisplayText => SubDisplayTextImpl();

        private string SubDisplayTextImpl()
        {
            string str = string.Empty;
            switch (GetState())
            {
                case OmikujiState.BeforeLotted:
                    str = "おみくじを選択して下さい";
                    break;
                case OmikujiState.AfterLotted:
                    // TODO: おみくじ引いたあとのディスプレイ
                    break;
            }
            return str;
        }

        public OmikujiState State => GetState();

        public IFactory Factory { get; }

        private OmikujiState GetState()
        {
            // selectedIndexに値が入っているかどうかで状態を決める
            OmikujiState state;
            if (selectedIndex.HasValue)
            {
                state = OmikujiState.AfterLotted;
            }
            else
            {
                state = OmikujiState.BeforeLotted;
            }
            return state;
        }
    }

    public enum OmikujiState
    {
        BeforeLotted,
        AfterLotted
    }
}
