using System;
using System.Collections.Generic;
using GleamSolverLibrary.GleamDb.Enums;

namespace GleamSolverLibrary.GleamDb
{
    public struct GleamDbGiveawayInfo
    {
        public GleamDbGiveawayInfo(string name, string region, string link, int daysLeft, int totalEntries,
            DateTime startedAt, List<AccountRequirement> accountRequirements, bool isHiddenEntries = false)
        {
            Name = name;
            Region = region;
            Link = link;
            DaysLeft = daysLeft;
            TotalEntries = totalEntries;
            StartedAt = startedAt;
            AccountRequirements = accountRequirements;
            IsHiddenEntries = isHiddenEntries;
        }

        public string Name { get; set; }
        public string Region { get; set; }
        public string Link { get; set; }
        public int DaysLeft { get; set; }
        public int TotalEntries { get; set; }
        public bool IsHiddenEntries { get; set; }
        public DateTime StartedAt { get; set; }
        public List<AccountRequirement> AccountRequirements { get; set; }
    }
}