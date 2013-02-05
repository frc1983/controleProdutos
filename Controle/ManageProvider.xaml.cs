using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controle.Domain;
using Controle.database;

namespace Controle
{
	/// <summary>
	/// Interaction logic for ManageCategory.xaml
	/// </summary>
	public partial class ManageProvider : UserControl
	{
		public ManageProvider()
		{
			InitializeComponent();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			var provider = new Provider()
			{
				Id = Database.GetInstance.redisClient.As<Provider>().GetNextSequence(),
				Name = txtFornecedor.Text
			};

			using (var trans = Database.GetInstance.redisClient.CreateTransaction())
			{
				trans.QueueCommand(r => r.Store<Provider>(provider));

				trans.Commit();
			}
		}
	}
}
