﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_JSON;
using HtmlAgilityPack;

namespace BS_PokedexManager
{
    class WebScraper
    {
        public static List<JsonParse.Move> ScrapeLevelMoves(string namePokémon)
        {
            List<JsonParse.Move> moves = new List<JsonParse.Move>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://bulbapedia.bulbagarden.net/wiki/" + namePokémon + "_(Pok%C3%A9mon)/Generation_II_learnset");

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                HtmlNode row = table.SelectNodes("tr")[1];

                foreach (HtmlNode cell in row.SelectNodes("td"))
                {
                    foreach (HtmlNode innerTable in cell.SelectNodes("table"))
                    {
                        foreach (HtmlNode innerRow in innerTable.SelectNodes("tr"))
                        {
                            if ((innerRow.SelectNodes("td") != null) && innerRow.SelectNodes("td").Count > 1)
                            {
                                foreach (HtmlNode innerCell in innerRow.SelectNodes("td"))
                                {


                                    if (innerCell.SelectNodes("span") != null &&
                                        innerCell.SelectNodes("span").Count > 0)
                                    {
                                        foreach (var innerSpan in innerCell.SelectNodes("span"))
                                        {
                                            innerCell.RemoveChild(innerSpan);
                                        }
                                    }
                                }

                                JsonParse.Move temp = new JsonParse.Move();
                                temp.Level = innerRow.SelectNodes("td")[0].InnerText.Replace(" ", "").Replace("\n", "");
                                temp.Name = innerRow.SelectNodes("td")[1].InnerText.Replace(" ", "").Replace("\n", "");
                                temp.Type = innerRow.SelectNodes("td")[2].InnerText.Replace(" ", "").Replace("\n", "");

                                try
                                {
                                    temp.Power = Convert.ToInt32(innerRow.SelectNodes("td")[3].InnerText.Replace("\n", ""));
                                }
                                catch
                                {
                                    temp.Power = 0;
                                }

                                try
                                {
                                    temp.Accuracy = Convert.ToInt32(innerRow.SelectNodes("td")[4].InnerText.Replace("%\n", ""));
                                }
                                catch
                                {
                                    temp.Accuracy = 0;
                                }

                                temp.PP = Convert.ToInt32(innerRow.SelectNodes("td")[5].InnerText.Replace("\n", "").Replace("}", ""));
                                moves.Add(temp);


                            }
                        }



                    }

                    return moves;

                }
            }

            return null;
        }

        public static List<JsonParse.Move> ScrapeMachineMoves(string namePokémon)
        {
            List<JsonParse.Move> machines = new List<JsonParse.Move>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://bulbapedia.bulbagarden.net/wiki/" + namePokémon + "_(Pok%C3%A9mon)/Generation_II_learnset");

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    if (row.SelectNodes("td") != null && row.SelectNodes("td").Count > 1)
                    {
                        if (row.SelectNodes("td")[1].InnerText.Contains("TM") ||
                            row.SelectNodes("td")[1].InnerText.Contains("HM"))
                        {
                            JsonParse.Move temp = new JsonParse.Move();
                            temp.Level = row.SelectNodes("td")[1].InnerText.Replace(" ", "").Replace("\n", "");
                            temp.Name = row.SelectNodes("td")[2].InnerText.Replace(" ", "").Replace("\n", "");
                            temp.Type = row.SelectNodes("td")[3].InnerText.Replace(" ", "").Replace("\n", "");
                            try
                            {
                                temp.Power = Convert.ToInt32(row.SelectNodes("td")[4].InnerText.Replace("\n", ""));
                            }
                            catch
                            {
                                temp.Power = 0;
                            }

                            try
                            {
                                temp.Accuracy = Convert.ToInt32(row.SelectNodes("td")[5].InnerText.Replace("%\n", ""));
                            }
                            catch
                            {
                                temp.Accuracy = 0;
                            }

                            temp.PP = Convert.ToInt32(row.SelectNodes("td")[6].InnerText.Replace("\n", "").Replace("#", ""));

                            machines.Add(temp);
                        }
                    }

                }



            }

            return machines;

        }
    }
}
