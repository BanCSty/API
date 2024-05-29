using API.Domain.ValueObjects;
using System;

namespace API.Domain
{
    //ИП
    public class IndividualEntrepreneur
    {
        public INN INN { get; private set; }
        public string Name { get; private set; }
        public DateTime DateCreate { get; private set; }
        public DateTime? DateUpdate { get; private set; }
        public INN FounderINN { get; private set; }

        // Учредитель ИП
        public virtual Founder Founder { get; private set; }

        public IndividualEntrepreneur(INN inn, string name, DateTime dateCreate, INN founderINN)
        {
            INN = inn ?? throw new ArgumentNullException(nameof(inn));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DateCreate = dateCreate;
            FounderINN = founderINN;
        }

        public IndividualEntrepreneur()
        {

        }

        public void UpdateName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void AddFounder(Founder founder)
        {
            Founder = founder ?? throw new ArgumentNullException(nameof(founder));
        }
    }

}
