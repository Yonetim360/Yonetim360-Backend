using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.Entity.CRM
{
    public class Offer : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public Representative Representative  { get; set; }
        public Guid RepresentativeId { get; set; }
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public string ServiceExplanation { get; set; }

        #region Fiyatlandırma kısmı
        public string Amount { get; set; }
        public decimal? DiscountValue { get; set; }
        public DiscountType? DiscountType { get; set; }
        public decimal? FinalAmount { get; set; }
        #endregion

        public DateTime ValidityDate { get; set; }
        public string Note { get; set; }
        public string DocumentUrl { get; set; }
        public Currency Currency { get; set; }
       


    }
        
    public enum OfferStatus
    {
        Pending=1,
        Accepted = 2,
        Signed=3,
        Rejected = 4
    }
    public enum Currency
    {
        TRY, // Türk Lirası
        USD, // Amerikan Doları
        EUR, // Euro
        GBP, // İngiliz Sterlini
        JPY  // Japon Yeni
    }
    public enum DiscountType
    {
        None = 0, // İndirim Yok
        Percentage = 1, // Yüzde İndirim
        FixedAmount = 2 // Sabit Tutar İndirim
    }
}