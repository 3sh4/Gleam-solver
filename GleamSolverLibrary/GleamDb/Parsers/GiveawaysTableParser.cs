using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Parser.Html;
using GleamSolverLibrary.GleamDb.Exceptions;

namespace GleamSolverLibrary.GleamDb.Parsers
{
    public class GiveawaysTableParser
    {
        private const string CompetitionsTableId = "Competition_table";
        private const string RewardsTableId = "Reward_table";

        private const string GiveawayElementsSelector = "tbody > tr";

        private List<GleamDbGiveawayInfo> ParseTableFromHtml(string html, string tableId)
        {
            if (string.IsNullOrWhiteSpace(html))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(html));

            var parser = new HtmlParser();
            var document = parser.Parse(html);


            var competitionTableElement = document.All.FirstOrDefault(m => m.Id == tableId);

            if (competitionTableElement == null)
                return new List<GleamDbGiveawayInfo>();


            var giveawayElements = competitionTableElement.QuerySelectorAll(GiveawayElementsSelector);

            var elementParser = new GiveawayTableElementParser();
            var giveawayInfos = new List<GleamDbGiveawayInfo>();

            foreach (var giveawayElement in giveawayElements)
            {
                try
                {
                    var giveawayInfo = elementParser.Parse(giveawayElement);
                    giveawayInfos.Add(giveawayInfo);
                }
                catch (BadFormatException e)
                {
                    //Log
                }
            }

            return giveawayInfos;
        }

        public List<GleamDbGiveawayInfo> ParseCompetitionsTableFromHtml(string html)
        {
            return ParseTableFromHtml(html, CompetitionsTableId);
        }


        public List<GleamDbGiveawayInfo> ParseRewardsTableFromHtml(string html)
        {
            return ParseTableFromHtml(html, RewardsTableId);
        }
    }
}