﻿using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record MaritalStatusCriterion : Criterion, ICriterion<MaritalStatusCriterion>
    {
        public MaritalStatus MaritalStatus { get; init; }

        private MaritalStatusCriterion(string id, char @operator, MaritalStatus maritalStatus) :
            base(id, @operator)
        {
            MaritalStatus = maritalStatus;
        }

        public static MaritalStatusCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && Enum.TryParse(parameters[0], out MaritalStatus maritalStatus))
                return new(id, @operator, maritalStatus);

            return null;
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.MaritalStatus.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            return GetDescription(MaritalStatus.GetDescription());
        }
    }
}