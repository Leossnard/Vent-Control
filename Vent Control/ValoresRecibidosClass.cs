using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vent_Control
{
	public class ValoresRecibidosClass
	{
		public static ObservableCollection<string> ValoresRecibidos = new ObservableCollection<string>();
		private Form1 formulario1;

		public ValoresRecibidosClass(Form1 form1)
		{
			formulario1 = form1;
			ValoresRecibidos.CollectionChanged += formulario1.ValoresRecibidos_CollectionChanged; ;
		}



		public void transformarDataRecibidaAndSubirla(string comandoInicial, string caracterSeparador)
		{

		}
	}
}
