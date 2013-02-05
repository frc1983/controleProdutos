using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controle.Domain
{
	public class Product
	{
		public long Id { get; set; }
		public long Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Quantity { get; set; }
		public decimal UnityValue { get; set; }
		public decimal WeightValue { get; set; }
		public Category Category { get; set; }
		public float Weight { get; set; }
		public Make Make { get; set; }
		public Provider Provider { get; set; }
		public long EAN { get; set; }
		public DateTime LastModification { get; set; }
	}
}
