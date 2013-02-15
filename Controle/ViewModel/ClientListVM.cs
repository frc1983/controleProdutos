using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Domain;

namespace Controle.ViewModel
{
	public class ClientListVM
	{
		Client p;

		public long Id { get { return p.Id; } }
		public string Name { get { return p.Name; } }
		public string CpfCnpj { get { return p.CpfCnpj; } }
		public string Sex { get { return p.Sex; } }
		public string BirthDate { get { return p.BirthDate.ToString(); } }
		public string Birthplace { get { return p.Birthplace.Name; } }
		public string MarriageState { get { return p.MarriageState.Name; } }
		public DateTime LastModification { get { return p.LastModification; } }
		
		public ClientListVM(Client p)
		{
			this.p = p;
		}

		public static implicit operator ClientListVM(Client p)
		{
			return new ClientListVM(p);
		}
	}
}
