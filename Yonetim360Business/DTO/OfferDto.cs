using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360.Entity.CRM;
using Yonetim360.Entity;

namespace Yonetim360Business.DTO
{
    public class OfferDto
    {
             public Guid Id { get; set; }
             public Guid UserId { get; set; }
            public string Title { get; set; }
            public Guid RepresentativeId { get; set; }
            public RepresentativeDto Representative { get; set; }
            public Guid CustomerId { get; set; }
            public CustomerDto Customer { get; set; }
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


