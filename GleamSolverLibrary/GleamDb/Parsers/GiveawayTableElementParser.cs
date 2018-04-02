using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using GleamSolverLibrary.GleamDb.Enums;
using GleamSolverLibrary.GleamDb.Exceptions;

namespace GleamSolverLibrary.GleamDb.Parsers
{
    public class GiveawayTableElementParser
    {
        private const string GiveawayElementFieldsSelector = "tr > td";
        private const string AccountRequirementElementSelector = "ul > li > a";
        private const string HiddenEntriesElement = "<i>Hidden</i>";
        private const int GiveawayElementFieldsCount = 6;

        public GleamDbGiveawayInfo Parse( IElement element)
        {
            var elementFieldsBlocks = element.QuerySelectorAll(GiveawayElementFieldsSelector);

            if (elementFieldsBlocks.Length != GiveawayElementFieldsCount)
                throw new BadFormatException($"Wrong columns count: {elementFieldsBlocks.Length}.");

            var giveawayInfo =
                new GleamDbGiveawayInfo
                {
                    StartedAt = ParseStartDate(elementFieldsBlocks[0]),
                    Name = ParseName(elementFieldsBlocks[1].FirstElementChild),
                    Link = ParseLink(elementFieldsBlocks[1].FirstElementChild),
                    Region = ParseRegion(elementFieldsBlocks[2]),
                    AccountRequirements = ParseGiveawayRequirements(elementFieldsBlocks[4].FirstElementChild),
                    DaysLeft = ParseDaysLeft(elementFieldsBlocks[5]),
                    TotalEntries = ParseEntries(elementFieldsBlocks[3], out var isHiddenEntriesCount),
                    IsHiddenEntries = isHiddenEntriesCount
                };


            return giveawayInfo;
        }

        private int ParseDaysLeft( IElement daysLeftElement)
        {
            try
            {
                var daysLeft = int.Parse(daysLeftElement.InnerHtml);
                return daysLeft;
            }
            catch (Exception e)
            {
                throw new BadFormatException(e.Message);
            }
        }

        private List<AccountRequirement> ParseGiveawayRequirements( IElement requirementsElement)
        {
            try
            {
                var badges = requirementsElement.QuerySelectorAll(AccountRequirementElementSelector);

                return badges.Select(badgeElement =>
                    AccountRequirementFactory.GetRequirementByBadgeClassName(badgeElement.ClassName)).ToList();
            }
            catch (Exception e)
            {
                throw new BadFormatException(e.Message);
            }
        }

        private int ParseEntries( IElement entriesElement, out bool isHidden)
        {
            try
            {
                var entriesCount = int.Parse(entriesElement.InnerHtml);
                isHidden = false;
                return entriesCount;
            }
            catch (Exception e)
            {
                if (entriesElement.InnerHtml != HiddenEntriesElement)
                    throw new BadFormatException(e.Message);


                isHidden = true;
                return -1;
            }
        }

        private DateTime ParseStartDate( IElement startedElement)
        {
            try
            {
                var dateString = startedElement.InnerHtml;
                return DateTime.ParseExact(dateString, "yyyy-MM-dd", null);
            }
            catch (Exception e)
            {
                throw new BadFormatException(e.Message);
            }
        }

        private string ParseName( IElement nameElement)
        {
            var name = nameElement.InnerHtml;
            return name;
        }

        private string ParseLink( IElement linkElement)
        {
            var hrefAttributeValue = linkElement.GetAttribute("href");
            return hrefAttributeValue;
        }

        private string ParseRegion( IElement regionElement)
        {
            var region = regionElement.InnerHtml;
            return region;
        }
    }
}