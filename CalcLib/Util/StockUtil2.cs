﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Util
{
    public class StockPrice2 : StockPrice
    {
        //株価取得日時
        public DateTime PriceGetDate { get; }

        public StockPrice2(string c, decimal p, DateTime d, DateTime dt) : base(c, p, d) { PriceGetDate = dt; }
    }


    public static class StockUtil2
    {
        /// <summary>
        /// 株価を取得する
        /// </summary>
        /// <param name="code">証券コード4桁</param>
        /// <returns>株価情報</returns>
        public static StockPrice2 GetStockPrice(string code)
        {
            //HTMLのコードを文書として保存
            var doc = new HtmlAgilityPack.HtmlDocument();
            var web = new System.Net.WebClient();
            web.Encoding = Encoding.UTF8;

            //Yahooファイナンスの株式ページURL
            string urlText = "https://stocks.finance.yahoo.co.jp/stocks/detail/?code=";
            //証券コードをURLに追加
            urlText += code;

            var html = "";

            try
            {
                html = web.DownloadString(urlText);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e)
                {
                    Data = { { "エラー種別", "ネットワークエラー" } }
                };
            }

            try
            {
                //webを通してHTMLのコード取得
                doc.LoadHtml(html);

                //株価を示す部分をXPathで指定
                string xPath = @"//td[@class=""stoksPrice""]";
                //株価取得時間
                string getTimeXPath = @"//dd[@class=""yjSb real""]/span";
                //株価取得日付
                string getDateXPath = @"//dd[@class=""ymuiEditLink mar0""]/span";
                var xPathList = new List<string> { xPath, getTimeXPath, getDateXPath, };

                var stock = doc.DocumentNode.SelectSingleNode(xPath);
                var time = doc.DocumentNode.SelectSingleNode(getTimeXPath);
                var date = doc.DocumentNode.SelectSingleNode(getDateXPath);

                var GetStockDate = NMethod(date.InnerText, time.InnerText);

                return new StockPrice2(code, decimal.Parse(stock.InnerText), GetStockDate, DateTime.Now);
            }
            catch (Exception e)
            {
                throw new ApplicationException("エラーが発生しました", e)
                {
                    Data = { { "エラー種別", "スクレイピングエラー" } }
                };
            }
        }


        /// <summary>
        /// 取得時刻と日付（カッコを省く）を合わせる 
        /// (12/12) 10:10 →　12/12 10:10
        /// </summary>
        /// <param name="Date"></param>
        /// <param name="Time"></param>
        private static DateTime NMethod(string Date, string Time)
        {
            var year = DateTime.Now.Year;
            var month = Date.Substring(1, Date.IndexOf('/') - 1);
            var date = Date.Substring(Date.IndexOf('/') + 1, 2); //現状、かっこも取っている
            var hour = Time.Substring(0, Time.IndexOf(':'));
            var minute = Time.Substring(Time.IndexOf(':') + 1, 2);

            return new DateTime(year, int.Parse(month), int.Parse(date), int.Parse(hour), int.Parse(minute), 0);
        }
    }
}
