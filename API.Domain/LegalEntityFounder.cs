using API.Domain.ValueObjects;

namespace API.Domain
{
    //Промежуточная таблица для связи многие ко многим таблиц Founder и LegalEntity
    public class LegalEntityFounder
    {
        public INN LegalEntityINN { get; set; }
        public LegalEntity LegalEntity { get; set; }

        public INN FounderINN { get; set; }
        public Founder Founder { get; set; }
    }
}
