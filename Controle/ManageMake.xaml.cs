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
	public partial class ManageMake : UserControl
	{
		public ManageMake()
		{
			InitializeComponent();
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			var maker = new Make()
			{
				Id = Database.GetInstance.redisClient.As<Make>().GetNextSequence(),
				Name = txtFabricante.Text
			};

			using (var trans = Database.GetInstance.redisClient.CreateTransaction())
			{
				trans.QueueCommand(r => r.Store<Make>(maker));

				trans.Commit();
			}
		}
	}
}
