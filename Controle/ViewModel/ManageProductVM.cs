using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Domain;

namespace Controle.ViewModel
{
	public class ManageProductVM
	{
		Product p;

		public long Id { get { return p.Id; } }
		public long Code { get { return p.Code; } }
		public string Name { get { return p.Name; } }
		public string Description { get { return p.Description; } }
		public int Quantity { get { return p.Quantity; } }
		public decimal UnityValue { get { return p.UnityValue; } }
		public decimal WeightValue { get { return p.WeightValue; } }
		public string Category { get { return p.Category.Name; } }
		public double Weight { get { return p.Weight; } }
		public string Make { get { return p.Make.Name; } }
		public string Provider { get { return p.Provider.Name; } }
		public long EAN { get { return p.EAN; } }
		public DateTime LastModification { get { return p.LastModification; } }

		public ManageProductVM(Product p)
		{
			this.p = p;
		}

		public static implicit operator ManageProductVM(Product p)
		{
			return new ManageProductVM(p);
		}
	}
}
