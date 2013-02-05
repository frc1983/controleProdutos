using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controle.Domain
{
	class SaleItem
	{
		public int Id { get; set; }
		public Product ProductId { get; set; }
		public int SaleQuantity { get; set; }
	}
}
