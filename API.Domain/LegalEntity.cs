using API.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace API.Domain
{
    //ЮЛ
    public class LegalEntity
    {
        public INN INN { get; private set; }
        public string Name { get; private set; }
        public DateTime DateCreate { get; private set; }
        public DateTime? DateUpdate { get; private set; }

        private readonly List<Founder> _founders;
        public IReadOnlyCollection<Founder> Founders => _founders.AsReadOnly();

        public LegalEntity(INN inn, string name, DateTime dateCreate, List<Founder> founders)
        {
            INN = inn ?? throw new ArgumentNullException(nameof(inn));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DateCreate = dateCreate;
            _founders = founders ?? throw new ArgumentNullException(nameof(inn));
        }

        public LegalEntity()
        {

        }

        public void UpdateName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void AddFounder(Founder founder)
        {
            if (founder == null) throw new ArgumentNullException(nameof(founder));

            if (!_founders.Contains(founder))
            {
                _founders.Add(founder);
            }
        }

        public void RemoveFounder(Founder founder)
        {
            if(founder == null) throw new ArgumentNullException(nameof(founder));
            _founders.Remove(founder);
        }
    }
}
