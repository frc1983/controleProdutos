using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Domain;

namespace Controle.ViewModel
{
	public class AddressListVM
	{
		Address p;

		public long Id { get { return p.Id; } }
		public long IdClient { get { return p.IdClient; } }
		public string Place { get { return p.Place; } }
		public string Neighborhood { get { return p.Neighborhood; } }
		public string Number { get { return p.Number.ToString(); } }
		public string Complement { get { return p.Complement; } }
		public string City { get { return p.City.Name; } }
		public string State { get { return p.State.Name; } }
		public string Country { get { return p.Country.Name; } }
		public DateTime LastModification { get { return p.LastModification; } }

		public AddressListVM(Address p)
		{
			this.p = p;
		}

		public static implicit operator AddressListVM(Address p)
		{
			return new AddressListVM(p);
		}
	}
}
