using API.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace API.Domain
{
    //Учредитель
    public class Founder
    {
        public INN INN { get; private set; }
        public FullName FullName { get; private set; }
        public DateTime DateCreate { get; private set; }
        public DateTime? DateUpdate { get; private set; }

        //Юридическое лицо, к которому привязан учредитель
        private readonly List<LegalEntity> _legalEntities;
        public IReadOnlyCollection<LegalEntity> LegalEntities => _legalEntities.AsReadOnly();

        // Индивидуальный предприниматель, к которому привязан учредитель
        public IndividualEntrepreneur IndividualEntrepreneur { get; private set; }

        public Founder(INN inn, FullName fullName, DateTime dateCreate)
        {
            INN = inn ?? throw new ArgumentNullException(nameof(inn));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            DateCreate = dateCreate;
            _legalEntities = new List<LegalEntity>();
        }

        public void UpdateFullName(FullName fullName)
        {
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            DateUpdate = DateTime.UtcNow;
        }

        public void AddLegalEntity(LegalEntity legalEntity)
        {
            if (legalEntity == null) throw new ArgumentNullException(nameof(legalEntity));
            _legalEntities.Add(legalEntity);
            DateUpdate = DateTime.UtcNow;
        }

        public void AssignIndividualEntrepreneur(IndividualEntrepreneur individualEntrepreneur)
        {
            IndividualEntrepreneur = individualEntrepreneur ?? throw new ArgumentNullException(nameof(individualEntrepreneur));
            DateUpdate = DateTime.UtcNow;
        }

        public void DeleteIndividualEntrepreneur()
        {
            IndividualEntrepreneur = null;
        }

        public void DeleteLegalEntity(LegalEntity legalEntity)
        {
            if (legalEntity == null) throw new ArgumentNullException(nameof(legalEntity));
            _legalEntities.Remove(legalEntity);
            DateUpdate = DateTime.UtcNow;
        }
    }
}
