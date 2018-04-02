using System;
using System.Text.RegularExpressions;
using GleamSolverLibrary.GleamDb.Exceptions;

namespace GleamSolverLibrary.GleamDb.Enums
{
    public static class AccountRequirementFactory
    {
        private const string BadgeClassNameFormat = "^(\\w+)-bg$";

        public static AccountRequirement GetRequirementByBadgeClassName(string badgeClassName)
        {
            var classNameRegex = new Regex(BadgeClassNameFormat);

            if (!classNameRegex.IsMatch(badgeClassName))
                throw new BadFormatException("Account requirement badge class name format was incorrect.");

            var requirementName = classNameRegex.Match(badgeClassName).Groups[1].Value;

            var parsed = Enum.TryParse(requirementName, true, out AccountRequirement requirement);

            return parsed ? requirement : AccountRequirement.Unknown;
        }
    }
}