using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controle.Domain
{
	class Sale
	{
		public int Id { get; set; }
		public SaleItem SaleId { get; set; }
		public DateTime SaleDate { get; set; }
		public string PaymentForm { get; set; }
		public decimal TotalSaleValue { get; set; }
	}
}
