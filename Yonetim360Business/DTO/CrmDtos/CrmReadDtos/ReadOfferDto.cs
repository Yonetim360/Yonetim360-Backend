using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;

namespace Yonetim360Business.DTO.CrmReadDtos
{
    public class ReadOfferDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public string Title { get; set; }
        public Guid RepresentativeId { get; set; }
        public string? RepresentativeName { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerCompanyName { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public string ServiceExplanation { get; set; }

        #region Fiyatlandırma kısmı


        public decimal Amount { get; set; }
        public TaxIncluded? TaxIncluded { get; set; }
        public decimal? DiscountValue { get; set; }
        public DiscountType? DiscountType { get; set; }
        public decimal? FinalAmount { get; set; }

        #endregion

        public DateTime ValidityDate { get; set; }
        public string Note { get; set; }
        public string DocumentUrl { get; set; }
        public Currency Currency { get; set; }
    }
}
