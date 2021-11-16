using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Vent_Control
{
	public partial class Form1 : Form
	{


		public Form1()
		{
			InitializeComponent();
			propiedadesDinamicasComponentes = new Dictionary<string, List<string>>();
			seleccionarComponente = false;//new bool();
			DiccionarioComponentes = new Dictionary<string, Tuple<PictureBox, RotarComponente>>();
			// Instanciar clase para manejar datos recibidos
			valoresRecibidosClass = new ValoresRecibidosClass(this);
			formMonitoreo = new FormMonitoreo(this);
			//Control.CheckForIllegalCrossThreadCalls = false;
			buttonStop.Enabled = false;
			buttonValidaciones.Enabled = false;
			buttonModoManual.Enabled = false;
			buttonMonitoreo.Enabled = false;
			formValidaciones = new FormValidaciones(this);
			//setear timerModbus
			timerModbus.Interval = 1500;
			timerModbus.Tick += TimerModbus_Tick;


		}

		// --------------------- Variables globales ---------------------------------------------
		string pathEjecutable = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
		// Se define un diccionario donde la llave es un string, el cual identifica a cada nuevo PictureBox y su respectiva rotacion (clase) en el caso de que se gire la imagen en una tupa de dichos tipos de objetos
		// Este primer diccionario se usa para modificar o ver las propiedades de cada clase
		public static Dictionary<string, Tuple<PictureBox, RotarComponente>> DiccionarioComponentes = new Dictionary<string, Tuple<PictureBox, RotarComponente>>();
		// Se define un diciconario donde se almacena el identificador de la tupla anterior como llave y se asocia a una lista con todos las propiedades del picturebox 
		// Lista de solos Id de los componentes para poder acceder a ellos cuando se requiera
		List<string> soloIDcomponentes = new List<string>();
		// diccionario que guarda la informacion de una llave y una lista de propiedades del siguiente formato: llave->orientacion (1->90°,0->0°), bring to front (1) or send to back (0), contenedor, ubicacionImagen (en el caso de custom)
		public static Dictionary<string, List<string>> propiedadesDinamicasComponentes = new Dictionary<string, List<string>>();
		// path del archivo que se guardo, para no preguntar cada vez que se guarda.
		string pathArchivoActual = "";
		// Indica si hay un componente en el panel1 usado en AbrirArchivo(), guardarComponentes() y en AgregarComponentes()
		bool hayComponentes = false;
		// se hace un timer para actualizar el cursor dado que el colector lo borra despues de un tiempo, dado que considera que el cursor no esta en uso dado que se aplico a un control dinamico
		Timer TimerCursorEliminar = new Timer();
		// Para saber el estado del modo dibujo (true-> si se desea mover los PB, false-> solo responderá el evento doubleclick)
		bool modoDibujo = false;
		// Para saber el modo del mouse segun el formulario Galerias Form
		public static bool seleccionarComponente = false;
		// formulario galerias, solo uno abierto a la vez
		public Configuraciones FormConfigurar = null;
		// Instanciar clase para almacena valores
		ValoresRecibidosClass valoresRecibidosClass;
		// Form Monitoreo
		FormMonitoreo formMonitoreo;
		// Indica cuando tomar medidas de los sensores y enviar informacion al ventilador/compuerta
		public static bool comenzarLecturas = false;
		// bool que indica que no se puede abrir el form de configuraciones
		bool bloquearConfig = false;
		// instanciar formValidacion
		FormValidaciones formValidaciones;
		//instanciar libreria modbus
		ModBusRTU modbusRTU = new ModBusRTU();
		//timer para Modbus
		Timer timerModbus = new Timer();


		// --------------------- Funciones ---------------------------------------------

		// Funcion para agregar un componente al panel1 y todos los eventos asociados a cada uno
		public Tuple<PictureBox, RotarComponente> AgregarComponentePanel(Bitmap ImagenComponente, Panel panelContenedor, float velocidad, string IDcomponente, bool marcadoLive = false)
		{
			// booleans que indican si se va a cambiar el tamaño del picturebox
			bool sizeDerecha = false;
			bool sizeIzquierda = false;
			bool sizeArriba = false;
			bool sizeAbajo = false;
			bool moverPB = false;

			Label labelLive = new Label();
			Label labelLive2 = new Label();


			Point puntoitemsseleccionado = new Point();
			PictureBox pictureboxitem = new PictureBox();
			pictureboxitem.Image = ImagenComponente;
			pictureboxitem.SizeMode = PictureBoxSizeMode.StretchImage;//Zoom
			if (marcadoLive)
			{
				pictureboxitem.Size = new Size(100, 50);
				pictureboxitem.MaximumSize = new Size(500, 250);
			}
			else
			{
				pictureboxitem.Size = new Size(100, 100);
				pictureboxitem.MaximumSize = new Size(1000, 1000);

			}
			pictureboxitem.MinimumSize = new Size(30,30);
			pictureboxitem.Cursor.Tag = "MOVER"; // tag predeterminado


			// para guardar la posicion left y top maxima
			int pictureboxUltimaleft = 0;
			int pictureboxUltimaTop = 0;
			int pictureboxLeftClick = pictureboxitem.Left;
			int pictureboxTopClick = pictureboxitem.Top;
			// al presionar un boton del mouse sobre el pictureboxitem ----------------------------------------------------------------------> dependiendo del Tag del cursor, se borra el PB en otro caso guarda la informacion al hacer el click
			pictureboxitem.MouseDown += (sender1, e1) =>
			{
				if (e1.Button == MouseButtons.Left)
				{
					if (modoDibujo)
					{
						pictureboxitem.BorderStyle = BorderStyle.FixedSingle;
						deseleccionarTodos(IDcomponente);


						// Si el tag del cursor es ELIMINAR se pregunta si se quiere eliminar el picturebox
						if (pictureboxitem.Cursor.Tag.ToString() == "ELIMINAR")
						{
							if (MessageBox.Show("¿Quiere elminar este componente: " + IDcomponente + "?", "Precaucion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
							{
								// Para reestablecer el el cursor en todos los Componentes y parar el timer del cursor
								TimerCursorEliminar.Stop();
								foreach (string llave in soloIDcomponentes)
								{
									DiccionarioComponentes[llave].Item1.Cursor = Cursors.SizeAll;
									DiccionarioComponentes[llave].Item1.Cursor.Tag = "MOVER";
								}
								//Eliminar este Componente

								eliminarComponentes(IDcomponente);
								pulsarEliminar = 0;
								TimerCursorEliminar.Stop();
								toolStripBorrar.BackColor = toolStrip1.BackColor;
							}
						}
						else//en cualquier otro caso
						{
							// guarda la posicion inicial del cursor con respecto al pictureboxitem
							puntoitemsseleccionado = e1.Location;
							// guarda la posicion en funcion al panel1 del picturebox
							pictureboxLeftClick = pictureboxitem.Left;
							pictureboxTopClick = pictureboxitem.Top;
							// guarda la posicion maxima posible
							pictureboxUltimaleft = pictureboxitem.Left + pictureboxitem.Width - pictureboxitem.MaximumSize.Width;
							if (pictureboxUltimaleft <= 0)
							{
								pictureboxUltimaleft = 0;
							}
							// lo mismo con el top
							pictureboxUltimaTop = pictureboxitem.Top + pictureboxitem.Height - pictureboxitem.MaximumSize.Height;
							if (pictureboxUltimaTop <= 0)
							{
								pictureboxUltimaTop = 0;
							}

							// guarda la posicion global del cursor

							// si se presiona un borde del picturebox se activa el boolean size dependiendo del caso
							string tagCursor = pictureboxitem.Cursor.Tag.ToString();
							switch (tagCursor)
							{
								case "SIZE-R":
									sizeDerecha = true;
									break;
								case "SIZE-L":
									sizeIzquierda = true;
									break;
								case "SIZE-U":
									sizeArriba = true;
									break;
								case "SIZE-D":
									sizeAbajo = true;
									break;
								case "MOVER":
									moverPB = true;
									break;
							}

						}
					}


				}
			};
			// Al mover el Mouse sobre el pictureboxItem -------------------------------------------------------------------------------------> Setea el tag del cursor dependiendo de donde se hizo el click
			pictureboxitem.MouseMove += (sender1, e1) =>
			{
				// si se pone el cursor sobre uno de los bordes con una holgura de 8 pixels se tagea size en el cursor
				//pictureboxitem.BorderStyle = BorderStyle.FixedSingle;
				if (modoDibujo)
				{
					if (pictureboxitem.Cursor.Tag.ToString() != "ELIMINAR" && pictureboxitem.BorderStyle == BorderStyle.FixedSingle) // si el tag es distinto a eliminar y el borde esta activo (PB se encuentra seleccionado)
					{
						//Por la parte derecha
						if (e1.X <= pictureboxitem.Width && e1.X > pictureboxitem.Width - 8)
						{
							pictureboxitem.Cursor = Cursors.SizeWE;
							pictureboxitem.Cursor.Tag = "SIZE-R";
						}
						//Por la parte izquierda
						else if (e1.X >= 0 && e1.X < 8)
						{
							pictureboxitem.Cursor = Cursors.SizeWE;
							pictureboxitem.Cursor.Tag = "SIZE-L";

						}
						//Por la parte DE ARRIBA
						else if (e1.Y >= 0 && e1.Y < 8)
						{
							pictureboxitem.Cursor = Cursors.SizeNS;
							pictureboxitem.Cursor.Tag = "SIZE-U";

						}
						// por la parte de abajo
						else if (e1.Y > pictureboxitem.Height - 8 && e1.Y <= pictureboxitem.Height)
						{
							pictureboxitem.Cursor = Cursors.SizeNS;
							pictureboxitem.Cursor.Tag = "SIZE-D";

						}
						// si es que se posa el cursor sobre cualquier parte menos las Esquinas
						else
						{
							pictureboxitem.Cursor = Cursors.SizeAll;
							pictureboxitem.Cursor.Tag = "MOVER";

						}
					}

					// si se presiona el click izquierdo
					if (e1.Button == MouseButtons.Left && pictureboxitem.BorderStyle == BorderStyle.FixedSingle)
					{
						if (sizeDerecha)// si se activa el cambio de tamaño, con el cursor.tag = "SIZE-R"
						{
							pictureboxitem.Width = e1.X + 1;
						}
						else if (sizeIzquierda)
						{
							//si se excede la posicion maxima, se situa el control en la posicion maxima y el cursor se bloquea
							if (pictureboxitem.Left < pictureboxUltimaleft)
							{
								pictureboxitem.Left = pictureboxUltimaleft;
								Cursor.Position = new Point(this.Left + panel1.Left + pictureboxUltimaleft + 10, Cursor.Position.Y);
							}
							else//si no se alcanza la posicion maxima, se mueve la posicion left del pb segun el cursor
							{
								pictureboxitem.Left = e1.X + pictureboxitem.Left - puntoitemsseleccionado.X;
							}
							// si el cursor se mueve a la derecha (en comparacion a la posicion que se pulso el click
							if (pictureboxitem.Left > pictureboxLeftClick)
							{
								pictureboxitem.Width = pictureboxitem.Width - Math.Abs(e1.X - puntoitemsseleccionado.X);
							}
							else// si el cursor se mueve a la izquierda (en comparacion a la posicion que se pulso el click
							{
								pictureboxitem.Width = pictureboxitem.Width + Math.Abs(e1.X - puntoitemsseleccionado.X);
							}
							pictureboxLeftClick = pictureboxitem.Left;//se actualiza la posicion inicial a la nueva
						}
						else if (sizeArriba)
						{
							if (pictureboxitem.Top < pictureboxUltimaTop)
							{
								pictureboxitem.Top = pictureboxUltimaTop;
								Cursor.Position = new Point(Cursor.Position.X, this.Top + panel1.Top + pictureboxUltimaTop + 35);// ver si cambia el 10
							}
							else
							{
								pictureboxitem.Top = e1.Y + pictureboxitem.Top - puntoitemsseleccionado.Y;
							}
							if (pictureboxitem.Top > pictureboxTopClick)
							{
								pictureboxitem.Height = pictureboxitem.Height - Math.Abs(e1.Y - puntoitemsseleccionado.Y);
							}
							else
							{
								pictureboxitem.Height = pictureboxitem.Height + Math.Abs(e1.Y - puntoitemsseleccionado.Y);
							}
							pictureboxTopClick = pictureboxitem.Top;
						}
						else if (sizeAbajo)
						{
							pictureboxitem.Height = e1.Y + 1;
						}
						else if (moverPB)//pictureboxitem.Cursor.Tag.ToString() == "MOVER")
						{
							pictureboxitem.Left = e1.X + pictureboxitem.Left - puntoitemsseleccionado.X;
							pictureboxitem.Top = e1.Y + pictureboxitem.Top - puntoitemsseleccionado.Y;

						}

						// si el pictureboxitem toca el borde izquierdo, el Cursor no se mueve
						if (pictureboxitem.Left <= 0)
						{
							Cursor.Position = new Point(puntoitemsseleccionado.X + this.Left + panel1.Left + 11, Cursor.Position.Y);
							if (pictureboxitem.Top <= 0)
							{
								Cursor.Position = new Point(Cursor.Position.X, this.Top + panel1.Top + puntoitemsseleccionado.Y + 34);
							}
							else if (pictureboxitem.Top + pictureboxitem.Height >= panel1.Height)
							{
								Cursor.Position = new Point(Cursor.Position.X, this.Top + panel1.Top + panel1.Height + puntoitemsseleccionado.Y - pictureboxitem.Height + 30);
							}
						}
						else if (pictureboxitem.Top <= 0)
						{
							Cursor.Position = new Point(Cursor.Position.X, this.Top + panel1.Top + puntoitemsseleccionado.Y + 34);
							if (pictureboxitem.Left + pictureboxitem.Width >= panel1.Width)
							{
								Cursor.Position = new Point(this.Left + panel1.Left + panel1.Width + puntoitemsseleccionado.X - pictureboxitem.Width + 5, Cursor.Position.Y);
							}
						}
						else if (pictureboxitem.Left + pictureboxitem.Width >= panel1.Width)
						{
							Cursor.Position = new Point(this.Left + panel1.Left + panel1.Width + puntoitemsseleccionado.X - pictureboxitem.Width + 5, Cursor.Position.Y);
						}
						else if (pictureboxitem.Top + pictureboxitem.Height >= panel1.Height)
						{
							Cursor.Position = new Point(Cursor.Position.X, this.Top + panel1.Top + panel1.Height + puntoitemsseleccionado.Y - pictureboxitem.Height + 30);
						}
					}
					else
					{
						sizeDerecha = false;
						sizeIzquierda = false;
						sizeArriba = false;
						sizeAbajo = false;
						moverPB = false;
					}
				}
			};
			//al sacar el cursor del pictureboxitem
			pictureboxitem.MouseLeave += (sender1, e1) =>
			{
				pictureboxitem.Cursor = Cursors.Default;
				
			};
			//al hacer doble click sobre el picturebox
			pictureboxitem.DoubleClick += (sender1, e3) =>
				{
					if(propiedadesDinamicasComponentes[IDcomponente][2] != "1" && IDcomponente.Contains("GAL") == true)// si es una galeria y ademas no contenedora
					{
						DialogResult msg = MessageBox.Show("El Componente: " + IDcomponente + " , No es Contenedor.\n¿Desea Activarlo para que Pueda Contener Sensores?","Pregunta"
							,MessageBoxButtons.YesNo,MessageBoxIcon.Question);
						if(msg == DialogResult.Yes)//hace q la galeria sea contenedora
						{
							propiedadesDinamicasComponentes[IDcomponente][2] = "1";
							mostrarFormGalerias(IDcomponente);
						}
					}
					else if (propiedadesDinamicasComponentes[IDcomponente][2] == "1" && IDcomponente.Contains("GAL") == true)//si es una galeria y es contenedora
					{
						mostrarFormGalerias(IDcomponente);
					}
					else//en el caso de que no sea un galeria contenedora
					{
						if (seleccionarComponente)// esperando interaccio del usuario para seleccionar el sensor a agregar
						{
							FormConfigurar.sensorSeleccionadoClick = IDcomponente;
							FormConfigurar.Enabled = true;
							FormConfigurar.WindowState = FormWindowState.Normal;
							FormConfigurar.BringToFront();
							FormConfigurar.TopLevel = true;
							FormConfigurar.Focus();
							FormConfigurar.manejarSensorSeleccionadoClick();
						}
						else
						{
							mostrarFormGalerias(IDcomponente);
							FormConfigurar.sensorSeleccionadoClick = IDcomponente;
						}
					}
				};
			//se activa cuando cambia el tamaño del PB, para cambiar el tamaño del label
			pictureboxitem.SizeChanged += (sender1, e1) => 
				{
					if (marcadoLive)
					{
						int sizeFont = pictureboxitem.Height / 4;
						labelLive.Font = new Font("Arial", sizeFont, FontStyle.Regular);
						int sizeFont2 = pictureboxitem.Height / 8;
						labelLive2.Font = new Font("Arial", sizeFont2, FontStyle.Regular);
					}
				};

			RotarComponente ComponenteRotado = new RotarComponente();
			// si el componente requiere ser rotado
			if (IDcomponente.Contains("VENT"))
			{
				// Instanciar Clase RotarComponentes
				float Angulo = 0f;
				Timer TimerVent = new Timer();
				TimerVent.Interval = 50;
				ComponenteRotado.velocidad = velocidad;
				TimerVent.Start();
				TimerVent.Tick += (sender1, e1) =>
				{
					Angulo++;
					pictureboxitem.Image = ComponenteRotado.RotarBitmap(ImagenComponente, Angulo);
				};
			}

			panelContenedor.Controls.Add(pictureboxitem);

			if (marcadoLive)
			{
				// configuracion de la medicion en vivo tiene las mismas configuraciones que el picturebox original y un solo label dentro
				labelLive.Location = new Point(10, pictureboxitem.Height / 2 - labelLive.Height / 2 + 2);
				labelLive.AutoSize = true;
				labelLive.Text = "MEDICION";
				labelLive.Font = new Font("Arial", pictureboxitem.Height / 4, FontStyle.Regular);
				pictureboxitem.Controls.Add(labelLive);

				labelLive2.Location = new Point(5, 2);
				labelLive2.AutoSize = true;
				labelLive2.Text = "Nombre de la medicion";
				labelLive2.Font = new Font("Arial", pictureboxitem.Height / 8, FontStyle.Regular);
				pictureboxitem.Controls.Add(labelLive2);
			}
			else//si no se usa el label lo libera
			{
				labelLive.Dispose();
				labelLive2.Dispose();
			}
			return Tuple.Create(pictureboxitem, ComponenteRotado);
		}
		// Obtiene la imagen correspondiente a cada componente, "VENT","GALO","GALC","PRESSE","PRESST, "PRESDIF","GASCO2","GASCO","HUMID","TEMP","COMPUERTA","LIVE"
		public Bitmap imagenParaComponente(string tipoComponente)
		{
			switch (tipoComponente)
			{
				case "VENT":
					return Properties.Resources.Ventilador1;
				case "GALO":
					return Properties.Resources.galeria_abierta;
				case "GALOIA":
					return Properties.Resources.galeria_abierta_izq_abajo;
				case "GALODA":
					return Properties.Resources.galeria_abierta_der_arriba;
				case "GALC":
					return Properties.Resources.galeria_cerrada;
				case "PRESSE":
					return Properties.Resources.presion_estatica;
				case "PRESST":
					return Properties.Resources.Presion_total;
				case "PRESDIF":
					return Properties.Resources.sensor_Presion_diferencial;
				case "GASCO":
					return Properties.Resources.sensor_gas_CO;
				case "GASCO2":
					return Properties.Resources.sensor_gas_CO2;
				case "HUMID":
					return Properties.Resources.sensor_humedad;
				case "TEMP":
					return Properties.Resources.Sensor_Temp;
				case "COMPUERTA":
					return Properties.Resources.compuerta;
				case "LIVE":
					return Properties.Resources.marcoLiveMarcador;
				case "COMPORTS":
					return null;
				default:
					throw new ArgumentException("Elegir un Componente dentro de los Disponibles en el if - if else.", "Error");
			}
		}

		// Funcion que guarda las propiedades de cada componente en un archivo que el usuario elige
		private void GuardarPropiedades(string tituloGuardado = "Guardar")
		{

			// Este lista se usa para guardar las propiedades en un archivo
			// Formato del archivo a guardar llave, PB.left, PB.Top, PB.Width, PB.Height, VelocidadActualComponente (en el caso del ventilador), orientacion (1->90°,0->0°), bring to front (1) or send to back (0), contenedor(de sensores) 1 si 0 no.
			List<List<string>> PropiedadesComponentes = new List<List<string>>();
			for (int i = 0; i < soloIDcomponentes.Count; i++)
			{
				// q y p son el picturebox y el rotarimagen class respectivamente, para hacerlo mas legible
				PictureBox q = DiccionarioComponentes[soloIDcomponentes[i]].Item1;
				RotarComponente p = DiccionarioComponentes[soloIDcomponentes[i]].Item2;
				// si rotarimagen class es nulo, se agrega un string vacio como velocidad
				string velocidadComponenteTemp = "";
				if (p != null) { velocidadComponenteTemp = p.velocidad.ToString(); }
				List<string> listaPropiedadesComponente = new List<string> { soloIDcomponentes[i], q.Left.ToString(), q.Top.ToString(), q.Width.ToString(), q.Height.ToString(), p.velocidad.ToString() };
				// aca se pueden agregar todos las propiedades dinamicas
				foreach (string propiedadDinamica in propiedadesDinamicasComponentes[soloIDcomponentes[i]])
				{
					listaPropiedadesComponente.Add(propiedadDinamica);
				}
				// completa la lista general
				PropiedadesComponentes.Add(listaPropiedadesComponente);
				// formato guardado: ID componente, PB.Left, PB.Top, PB.Width, PB.Heigth, Velocidad Ventilador, (de aqui en adelante propiedades dinamicas)
				// formato guardado:        0     ,    1   ,   2   ,     3   ,     4    ,          5          ,
			}

			//Para las configuraciones del los puertos
			if (propiedadesDinamicasComponentes.ContainsKey("COMPORTS"))
			{
				List<string> listaTemp = propiedadesDinamicasComponentes["COMPORTS"];
				if(listaTemp[0] != "COMPORTS")
				{
					listaTemp.Insert(0, "COMPORTS");
				}
				PropiedadesComponentes.Add(listaTemp);
			}

			// Recorrer la historia y poner el Identificador y sus propiedades en la misma linea, separadas por coma
			if (PropiedadesComponentes.Count == 0)
			{
				labelMensaje.ForeColor = Color.Red;
				labelMensaje.Text = "No hay Ningun Componente en el Panel";
				return;
			}
			List<string> ListaLineas = new List<string>();
			foreach (List<string> subLista in PropiedadesComponentes)
			{
				string Linea = "";
				for (int i = 0; i < subLista.Count; i++)
				{
					if (i == subLista.Count - 1)
					{
						Linea = Linea + subLista[i];
					}
					else
					{
						Linea = Linea + subLista[i] + ",";
					}
				}
				ListaLineas.Add(Linea);
			}
			// trasformar a array
			string[] LineasArray = ListaLineas.ToArray();
			// Seleccionar directorio para guardar configuraciones y guarda el archivo

			if (pathArchivoActual == "")
			{

				SaveFileDialog pathSave = new SaveFileDialog();
				pathSave.Title = tituloGuardado;
				pathSave.DefaultExt = "txt"; // se podria cambiar a extension a una que nosotros fijemos
				pathSave.Filter = "Archivos de Texto|*.txt";
				if (pathSave.ShowDialog() == DialogResult.OK)
				{
					pathArchivoActual = pathSave.FileName;
					System.IO.File.WriteAllLines(pathArchivoActual, LineasArray);
					labelMensaje.ForeColor = Color.Green;
					labelMensaje.Text = "Guardado Correcto!";
					// pone el nombre del archivo actual en pantalla
					System.IO.FileInfo fi = new System.IO.FileInfo(pathArchivoActual);
					string NombreArchivo = fi.Name;
					labelArchivo.Text = "Archivo Actual: " + NombreArchivo;
				}
				else
				{
					labelMensaje.ForeColor = Color.Red;
					labelMensaje.Text = "Guardado Erroneo!";
				}
			}
			else
			{
				System.IO.File.WriteAllLines(pathArchivoActual, LineasArray);
				labelMensaje.Text = "Guardado Correcto!";
				// pone el nombre del archivo actual en pantalla
				System.IO.FileInfo fi = new System.IO.FileInfo(pathArchivoActual);
				string NombreArchivo = fi.Name;
				labelArchivo.Text = "Archivo Actual: " + NombreArchivo;
			}

			PropiedadesComponentes.Clear();


		}

		// Funcion que elimina todos los componenetes del panel o uno si se le indica la llave
		public void eliminarComponentes(string llaveOpcional = "BORRAR TODO")
		{
			if (llaveOpcional == "BORRAR TODO")
			{
				for (int i = 0; i < soloIDcomponentes.Count; i++)
				{
					DiccionarioComponentes[soloIDcomponentes[i]].Item1.Image = null;
					DiccionarioComponentes[soloIDcomponentes[i]].Item1.Dispose();
				}

				// liberar lista y diccionarios
				soloIDcomponentes.Clear();
				DiccionarioComponentes.Clear();
				propiedadesDinamicasComponentes.Clear();
				hayComponentes = false;
				//libera las mediciones y condiciones
				MedicionesClass.medicionesDiccionario.Clear();
				CondicionesClass.CondicionesListaAumentar.Clear();
				CondicionesClass.CondicionesListaDisminuir.Clear();
				ArduinoConexion.diccionarioConfigsCOM.Clear();
			}
			else
			{
				if (soloIDcomponentes.Contains(llaveOpcional) == false || DiccionarioComponentes.ContainsKey(llaveOpcional) == false)
				{
					throw new ArgumentException("La llave indicada no fue encontrada, asegurarse que la llave exista");
				}
				else
				{
					DiccionarioComponentes[llaveOpcional].Item1.Image = null;
					DiccionarioComponentes[llaveOpcional].Item1.Dispose();
					soloIDcomponentes.Remove(llaveOpcional);
					DiccionarioComponentes.Remove(llaveOpcional);
					propiedadesDinamicasComponentes.Remove(llaveOpcional);
					//Eliminar el componente (galeria, sensor, etc) de las configuraciones-> Tab1, tab2, tab3 y de sus respectivos diccionarios
					// eliminar el componente de la galeria contenedora
					List<string> listaPorBorrar = new List<string>();
					foreach (string componenteKey in propiedadesDinamicasComponentes.Keys)
					{
						if (componenteKey.Contains("GAL"))
						{
							foreach (string elementoEnLista in propiedadesDinamicasComponentes[componenteKey])
							{
								if (elementoEnLista == llaveOpcional)
								{
									propiedadesDinamicasComponentes[componenteKey].Remove(llaveOpcional);
									break;
								}
							}

						}
					}
					actualizarDiccionariosClases();

				}

			}
		}

		// Funcion que lee un archivo en donde se encuentran los componentes lo pasa a una lista y luego se itera la lista para que cada elemento quede donde estaba ademas aplica cada propiedad
		private void AbrirArchivo()
		{
			// si se accede por segunda vez primero liberar los diccionarios y listas
			//si se pasa a presionar otra ves se borra todo, por lo que se pone guarda y ademas se pregunta si se quiere abrir otro archivo

			if (pathArchivoActual != "")
			{
				if (MessageBox.Show("Si abre otro archivo, se quitara la configuracion actual\nPor su seguridad se han guardado los cambios actuales\n¿Desea continuar?",
					"Advertencia", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					//eliminar 
					buttonStop.PerformClick();
					formMonitoreo.Close();
					formMonitoreo = new FormMonitoreo(this);
					formValidaciones.Close();
					formValidaciones = new FormValidaciones(this);
					contadorFilasAll = 0;
					// liberar (Dispose) los picture box actuales
					eliminarComponentes();
					// Cerrar el form configuraciones si es que esta abierto
					if (FormConfigurar != null)
					{
						FormConfigurar.Close();
					}
				}
				else
				{
					return;
				}
			}
			// si hay algun componente en panel1, preguntar si se desea abrir un archivo, y da la opcion de guardar o no
			if (hayComponentes == true)
			{
				if (MessageBox.Show("Si abre un archivo, se quitara la configuracion actual\n¿Desea guardar cambios antes de seguir?",
					"Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					// guardar los cambios actuales
					GuardarPropiedades();
					eliminarComponentes();
					pathArchivoActual = "";
				}
				else
				{
					// elimina los componentes actuales y prosigue a cargar un archivo
					eliminarComponentes();

				}
			}

			//deja al usuario seleccionar el archivo
			OpenFileDialog openArchivo = new OpenFileDialog();
			openArchivo.Title = "Selecciona una Imagen";
			openArchivo.Filter = "Archivos de Texto|*.txt";//se puede cambiar en un futuro
			openArchivo.Multiselect = false;

			if(openArchivo.ShowDialog() == DialogResult.OK)
			{
				pathArchivoActual = openArchivo.FileName;
				System.IO.FileInfo fi = new System.IO.FileInfo(pathArchivoActual);
				string NombreArchivo = fi.Name;
				labelArchivo.Text = "Archivo Actual: " + NombreArchivo;
			}
			else
			{
				labelMensaje.ForeColor = Color.Red;
				labelMensaje.Text = "Error al Abrir el Archivo, Intente de Nuevo";
				return; // no sigue la ejecucion del programa
			}

			// leer archivo e iterar
			string[] lineasArchivo = System.IO.File.ReadAllLines(pathArchivoActual);
			foreach (string lineas in lineasArchivo)
			{
				string[] lineaDescompuesta = lineas.Split(',');
				// Agrega los componentes pero sin la funcion agregar componente, dado que los id deberian ser unicos
				// agrega las propiedades dinamicas al diccionario----------------------------------------------> Propiedades Dinamicas
				int comenzarDesde = 6;//comenzar desde las proiedades dinamicas (segun el orden del txt)
				if (lineaDescompuesta[0] == "COMPORTS")// comenzar desde uno, es decir despues de la key (segun el orden del txt) en el caso que sea COMPORTS
				{
					comenzarDesde = 1;
				}

				List<string> propiedadesDinamicasTemp = new List<string>();
				for (int i = comenzarDesde; i < lineaDescompuesta.Count(); i++)
				{
					propiedadesDinamicasTemp.Add(lineaDescompuesta[i]);
				}
				propiedadesDinamicasComponentes.Add(lineaDescompuesta[0], propiedadesDinamicasTemp);
				// si es un COMPORTS, es decir las configuraciones del puerto, avanza a la proxima iteracion.
				if (lineaDescompuesta[0] == "COMPORTS")
				{
					continue;
				}

				string[] tipoComponente = lineaDescompuesta[0].Split('-');
				bool esLiveMarcador = false;
				Bitmap imagenArchivo = null;
				if (tipoComponente[1] == "CUSTOM")
				{
					try
					{
						imagenArchivo = new Bitmap(lineaDescompuesta[9].Split(';')[1]);
					}
					catch(Exception errorCargarImagenAbrir)
					{
						MessageBox.Show("No se ha Encontrado la Imagen, del Componente " + lineaDescompuesta[0] + ", por favor Seleccione otra imagen para seguir","Error: No se encuentra la Imagen", MessageBoxButtons.OK, MessageBoxIcon.Error);
						//deja al usuario seleccionar el archivo
						OpenFileDialog openArchivoImagen = new OpenFileDialog();
						openArchivoImagen.Title = "Selecciona una Imagen para asociar Al sensor";
						openArchivoImagen.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.tif;...";
						openArchivo.Multiselect = false;
						string pathImagenActual = "";

						DialogResult openArchivoImagenResultado = openArchivoImagen.ShowDialog();
						bool resultado = true;
						bool error = false;
						if (openArchivoImagenResultado == DialogResult.OK || error == false)
						{
							pathImagenActual = openArchivoImagen.FileName;
							int indiceUltimoPunto = pathImagenActual.LastIndexOf('.');
							string extension = pathImagenActual.Substring(indiceUltimoPunto);
							if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".tif")
							{
								error = true;
								MessageBox.Show("Seleccione solo Archivos que sean Imagenes Compatibles.");
							}
						}
						if(error || openArchivoImagenResultado != DialogResult.OK)
						{
							DialogResult result = MessageBox.Show("Problema al Seleccionar la Imagen. Desea Seleccionarla Otra vez?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
							if(result == DialogResult.No)
							{
								resultado = false;
							}
							while (result == DialogResult.Yes)
							{
								openArchivoImagenResultado = openArchivoImagen.ShowDialog();
								if (openArchivoImagenResultado == DialogResult.OK)
								{
									pathImagenActual = openArchivoImagen.FileName;
									break;
								}
								else
								{
									resultado = false;
									result = MessageBox.Show("Problema al Seleccionar la Imagen. Desea Seleccionarla Otra vez?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
								}
							}
							
						}
						if (resultado)
						{

							try
							{
								imagenArchivo = new Bitmap(pathImagenActual);
							}
							catch(Exception errorssss)
							{
								MessageBox.Show("Eror. Se utilizara una imagen con signo de exclamacion en su lugar.");
								imagenArchivo = Properties.Resources.Exclamacion;
							}
							propiedadesDinamicasComponentes[lineaDescompuesta[0]][3] = "ImagenPath;" + pathImagenActual;
						}
						else
						{
							MessageBox.Show("Se utilizara una imagen con signo de exclamacion en su lugar.");
							imagenArchivo = Properties.Resources.Exclamacion;
						}
					}
				}
				else
				{
					// si es un LIVE marcador
					int opcion = 1;
					esLiveMarcador = false;
					if (tipoComponente[0] == "LIVE")
					{
						opcion = 0;
						esLiveMarcador = true;

					}
					// Imagen componente
					imagenArchivo = imagenParaComponente(tipoComponente[opcion]);
					// Aplica las propiedades dinamicas rotacion
	
				}
				if (lineaDescompuesta[6] == "1")//rota la imagen si esta rotada en el archivo
				{
					imagenArchivo.RotateFlip(RotateFlipType.Rotate90FlipY);
				}




				// Dibujar pictureBox y establecer su velocidad
				DiccionarioComponentes.Add(lineaDescompuesta[0], AgregarComponentePanel(imagenArchivo, panel1, float.Parse(lineaDescompuesta[5]), lineaDescompuesta[0], esLiveMarcador));//esta linea se puede cambiar, llamar a la funcion siguiente y ver que pasa
				//modificar picture box ya mostrado (contenido en el diccionario) y asignarles las propiedades
				DiccionarioComponentes[lineaDescompuesta[0]].Item1.Left = int.Parse(lineaDescompuesta[1]);//pb.left
				DiccionarioComponentes[lineaDescompuesta[0]].Item1.Top = int.Parse(lineaDescompuesta[2]);//pb.top
				DiccionarioComponentes[lineaDescompuesta[0]].Item1.Width = int.Parse(lineaDescompuesta[3]);//pb.width
				DiccionarioComponentes[lineaDescompuesta[0]].Item1.Height = int.Parse(lineaDescompuesta[4]);//pb.height																						
				soloIDcomponentes.Add(lineaDescompuesta[0]); //agrega las Keys a la lista
				// Formato: llave, PB.left, PB.Top, PB.Width, PB.Height, VelocidadActualComponente (en el caso del ventilador), //propdinamicas->// orientacion (1->90°,0->0°), bring to front (1) or send to back (0)

			}
			// Aplica el bring to front luego que se dibujen los componentes en el panel1
			foreach (string lineas in lineasArchivo)
			{
				if (lineas.Contains("COMPORTS"))
				{
					continue;
				}
				string[] lineaDescompuesta2 = lineas.Split(',');
				if (lineaDescompuesta2[7] == "1")
				{
					DiccionarioComponentes[lineaDescompuesta2[0]].Item1.BringToFront();
				}
			}
			// Asegura que los componentes esten sobre las galerias (bring to front)
			foreach (string lineas in lineasArchivo)
			{
				if (lineas.Contains("COMPORTS"))
				{
					continue;
				}
				string[] lineaDescompuesta2 = lineas.Split(',');
				if (lineaDescompuesta2[7] == "1" && (lineaDescompuesta2[0].Contains("GAL") == false))
				{
					DiccionarioComponentes[lineaDescompuesta2[0]].Item1.BringToFront();
				}
			}

			// Carga la informacion de las mediciones y las del LIVE marcador
			leerConfiguracionesDesdePropiedadesDinamicas();



		}

		// Funcion que se encarga de ir agregando los componentes y sus propiedades, para ello primero valida si alguna llave existe, para no agregar una llave que este en uso . Recordar modificar la funcion abrirArchivo si se hace algun cambio
		// Es para componentes nuevos
		public void AgregarComponentes(string componente, bool rotar90grados = false, bool contenedor = false, bool marcadoLive = false, string pathimagenSeleccionada = "")
		{
			// Codigo para que no agrege una llave existente
			// Ejemplo ID "COMPONENT-VENT-1"

			int indiceLlave = 1;
			while (indiceLlave < soloIDcomponentes.Count + 1 ) // el maximo de posibilidades + 1 dado que el indice empieza en 0.
			{
				string llaveTemp;
				if (marcadoLive)
				{
					llaveTemp = "LIVE-" + componente;

				}
				else
				{
					//si es custom seria COMPONENT-CUSTOM-nombre del sensor-numero
					llaveTemp = "COMPONENT-" + componente + "-" + indiceLlave.ToString();
				}
				if (soloIDcomponentes.Contains(llaveTemp) == false)// si la llave a probar no existe, queda el i con el numero que le corresponde
				{
					break;
				}
				indiceLlave++;
			}
			// String que identifica el componente que se agrego
			string componenteID;
			if (marcadoLive)
			{
				componenteID = "LIVE-" + componente;
			}
			else
			{
				componenteID = "COMPONENT-" + componente + "-" + indiceLlave.ToString();
			}

			// Bitmap del componente que se va agregar
			if (marcadoLive)
			{
				componente = "LIVE";
			}
			Bitmap imagenComponente;
			if (pathimagenSeleccionada != "")
			{
				try
				{
					imagenComponente = new Bitmap(pathimagenSeleccionada);
				}
				catch (Exception errorSubirImagen)
				{
					MessageBox.Show("No se pudo cargar la imagen seleccionada. Se cambio de Lugar?\nAgrege el Componente Nuevamente");
					return;
				}
			}
			else
			{
				imagenComponente = imagenParaComponente(componente);
			}

			// si es ventilador la imagen gira
			float velocidadComponente = 0f;
			if(componente == "VENT")
			{
				velocidadComponente = 0f;
			}

			if(componente != "COMPORTS")
			{
				// Crear el picturebox, lo agrega al panel1 y gira la imagen si se desea, finalmente se agrega a una tupla
				Tuple<PictureBox, RotarComponente> TuplaPrincipal = AgregarComponentePanel(imagenComponente, panel1, velocidadComponente, componenteID, marcadoLive);

				// para agregar Id componente creado a la lista de Llaves
				soloIDcomponentes.Add(componenteID);
				// se agrega a un diccionario el ID con la tupla Principal (picturebox y rotarimagen class)
				DiccionarioComponentes.Add(componenteID, TuplaPrincipal);
				// Le asigna el bring to front altiro
				DiccionarioComponentes[componenteID].Item1.BringToFront();

				//Texto inicial del marcador LIVE
				if (marcadoLive)
				{
					DiccionarioComponentes[componenteID].Item1.Controls[0].Text = MedicionesClass.medicionesDiccionario[componenteID.Split('-')[1]][2];
					DiccionarioComponentes[componenteID].Item1.Controls[1].Text = componenteID.Split('-')[1];
				}

				// --- propiedades dinamicas ----
				// orientacion
				//Rotar la imagen los grados especificados antes de agregar la imagen
				int indicaRotado = 0;
				if (rotar90grados)
				{
					imagenComponente.RotateFlip(RotateFlipType.Rotate90FlipY);
					indicaRotado = 1;
				}
				

				// Si es una galeria, indica si la galeria contendra sensores.
				int esContenedor = 0;
				if (contenedor == true)
				{
					esContenedor = 1;
				}
				// BRING TO FRONT por defecto, el usuario, luego puede cambiar ese valor en la ejecucion
				// se agregan las propiedades dinamicas, se agrega un null por defecto ya que hay componentes que no tienen esa propiedad---------------------> propiedades dinamicas
				propiedadesDinamicasComponentes.Add(componenteID, new List<string> { indicaRotado.ToString(), "1", esContenedor.ToString() });
			}
			else// En el caso de que el componente sea COMPORTS
			{
				// Se agrega solo a propiedadesDinamicas dado que es una configuracion y en ningun momento se mostrara en el panel 1, se usara para intercambiar la informacion entre los Formularios y poder guardar la informacion en un txt
				propiedadesDinamicasComponentes.Add(componente, new List<string>());
			}


			// Para indicar que se agrego el componente
			hayComponentes = true;

			//Agrega al diccionario la ubicacion de la imagen para un COMPONENT-CUSTOM (seria la posicion 3 en la lista de propiedades dinamicas)
			if(pathimagenSeleccionada != "")
			{
				propiedadesDinamicasComponentes[componenteID].Add("ImagenPath;" + pathimagenSeleccionada);
			}

		}

		// funcion que deseleccionna todos los PB del panel, menos el que se le indica
		public void deseleccionarTodos(string llavePictureboxActual = "nada")
		{
			if (llavePictureboxActual == "nada")
			{
				foreach (string llave in soloIDcomponentes)
				{
					DiccionarioComponentes[llave].Item1.BorderStyle = BorderStyle.None;
				}
			}
			else
			{
				foreach (string llave in soloIDcomponentes)
				{
					if (llave != llavePictureboxActual)
					{
						DiccionarioComponentes[llave].Item1.BorderStyle = BorderStyle.None;
					}
				}
			}

		}

		// Funcion que carga las configuraciones desde el txt y actualiza los diccionarios de las clases: MedicionesClass, CondicionesClass, ArduinoConexion
		public void leerConfiguracionesDesdePropiedadesDinamicas()
		{
			foreach (string componenteKey in propiedadesDinamicasComponentes.Keys)
			{
				foreach (string propiedadDinamicaKey in propiedadesDinamicasComponentes[componenteKey])
				{
					if (propiedadDinamicaKey.Contains("CONFIG-"))
					{
						List<string> configList = propiedadDinamicaKey.Split(';').ToList();
						configList[0] = configList[0].Split('-')[1];
						// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2
						//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10
						MedicionesClass.medicionesDiccionario.Add(configList[0], configList);
					}
					else if (propiedadDinamicaKey.Contains("CONDITION-"))
					{
						//Llave  : noombreMedicion; Orden: nombreMedicion, Galeria/Compuerta , nombreGaleria, condicion, tiempoEspera, Incremento, UnidadMedida, puertoCOM, Aumentar/Disminuir
						//indices:                     :      0        ,        1          ,      2       ,     3    ,      4      ,     5     ,      6      ,     7      ,         8 
						List<string> conditionList = propiedadDinamicaKey.Split(';').ToList();
						conditionList[0] = conditionList[0].Split('-')[1];
						if(conditionList[8] == "Aumentar")
						{
							CondicionesClass.CondicionesListaAumentar.Add(conditionList);
						}
						else if (conditionList[8] == "Disminuir")
						{
							CondicionesClass.CondicionesListaDisminuir.Add(conditionList);
						}
					}
				}
				if (componenteKey.Contains("LIVE-"))// carga la informacion de los LIVE marcadores (label dentro del picturebox)
				{
					DiccionarioComponentes[componenteKey].Item1.Controls[0].Text = MedicionesClass.medicionesDiccionario[componenteKey.Split('-')[1]][2];
					DiccionarioComponentes[componenteKey].Item1.Controls[1].Text = componenteKey.Split('-')[1];
				}
				if(componenteKey == "COMPORTS")
				{
					foreach (string cadenaInLista in propiedadesDinamicasComponentes["COMPORTS"])
					{
						List<string> listaSubir = Regex.Split(cadenaInLista, @"\ ; ").ToList();
						//List<string> listaSubir = cadenaInLista.Split(';').ToList();
						string key = listaSubir[0].Split('-')[1] + "-" + listaSubir[0].Split('-')[2];
						listaSubir.RemoveRange(0, 1);
						ArduinoConexion.diccionarioConfigsCOM.Add(key, listaSubir);
					}
					
				}

			} 
		}

		// Funcion que se encarga de actualizar los diccionarios en las clases en el caso de que se borre algun componente, este tambien se borre de los diccionarios
		public void actualizarDiccionariosClases()
		{
			void eliminarSiNoExiste(Dictionary<string,List<string>> diccionarioEnClase, Dictionary<string, List<string>> diccionarioPropDinamicas, int columna)
			{
				List<string> llavesPorBorrar = new List<string>();
				foreach (string llaveMedicion in diccionarioEnClase.Keys)
				{
					int contador = 0;
					foreach (string item in diccionarioPropDinamicas.Keys)
					{
						if(diccionarioEnClase[llaveMedicion][columna] != "")
						{
							if (diccionarioEnClase[llaveMedicion][columna] == item)
							{
								contador++;
								break;
							}
						}
						else
						{
							contador++;
							break;
						}
					}
					if (contador == 0)
					{
						llavesPorBorrar.Add(llaveMedicion);
					}
				}
				foreach (string item in llavesPorBorrar)
				{
					diccionarioEnClase.Remove(item);
				}
				llavesPorBorrar.Clear();
			}

			void eliminarSiNoExisteList(List<List<string>> listaClase, Dictionary<string, List<string>> diccionarioPropDinamicas, int columna)
			{
				int indiceLista = 0;
				while(indiceLista < listaClase.Count)
				{
					int contador = 0;
					foreach (string item in diccionarioPropDinamicas.Keys)
					{
						if(listaClase[indiceLista][columna] != "")
						{
							if(listaClase[indiceLista][columna] == item)
							{
								contador++;
								break;
							}
						}
						else
						{
							contador++;
							break;
						}
					}
					if(contador == 0)
					{
						listaClase.RemoveAt(indiceLista);
						if(indiceLista != 0)
						{
							indiceLista = indiceLista - 1;
						}
					}
					indiceLista++;
				}
			}

			//MedicionesClass
			eliminarSiNoExiste(MedicionesClass.medicionesDiccionario, propiedadesDinamicasComponentes, 3);//Galeria
			eliminarSiNoExiste(MedicionesClass.medicionesDiccionario, propiedadesDinamicasComponentes, 4);//NombreMedicion posicion1
			eliminarSiNoExiste(MedicionesClass.medicionesDiccionario, propiedadesDinamicasComponentes, 5);//NombreMedicion posicion 2

			//CondicionesClass
			eliminarSiNoExisteList(CondicionesClass.CondicionesListaAumentar, propiedadesDinamicasComponentes, 2);
			eliminarSiNoExisteList(CondicionesClass.CondicionesListaDisminuir, propiedadesDinamicasComponentes, 2);
			eliminarSiNoExisteList(CondicionesClass.CondicionesListaAumentar, MedicionesClass.medicionesDiccionario, 0);
			eliminarSiNoExisteList(CondicionesClass.CondicionesListaDisminuir, MedicionesClass.medicionesDiccionario, 0);
			eliminarSiNoExisteList(CondicionesClass.CondicionesListaAumentar, MedicionesClass.medicionesDiccionario, 0);
			eliminarSiNoExisteList(CondicionesClass.CondicionesListaDisminuir, MedicionesClass.medicionesDiccionario, 0);


		}

		// Funcion que cierra los puertos que estan siendo utilizados
		public void cerrrarPuertosCOM()
		{
			foreach (string keyCOM in ArduinoConexion.PuertosDisponibles.Keys)
			{
				List<string> puertosCerrados = new List<string>();
				if (puertosCerrados.Contains(keyCOM.Split('-')[0]) == false)
				{
					puertosCerrados.Add(keyCOM.Split('-')[0]);
					try
					{
						if (ArduinoConexion.PuertosDisponibles[keyCOM].IsOpen)
						{
							ArduinoConexion.PuertosDisponibles[keyCOM].Close();
						}
					}
					catch
					{
						//
					}
				}
			}
		}


		// --------------------- Eventos ---------------------------------------------
		// --- Botones---
		// Agregar un componente
		public void buttonAgregarComp_Click(object sender, EventArgs e)
		{
			// No se si dejar esto aca o como variable global???????
			Form form2 = new Form_Agregar_Componente(this);
			form2.Show();
			
		}

		// --- Barra de Herramientas ---
		// boton en la barra de herramientas que guarda en un archivo
		private void toolStripGuardar_Click(object sender, EventArgs e)
		{
			GuardarPropiedades();
		}
		// boton en la barra de herramientas que guarda como en un archivo
		private void toolStripGuardarComo_Click(object sender, EventArgs e)
		{
			pathArchivoActual = "";
			GuardarPropiedades("Guardar Como");
		}
		// lee un archivo y lo muestra en panel1
		private void toolStripAbrir_Click(object sender, EventArgs e)
		{
			AbrirArchivo();
		}
		// Al presionar el boton para eliminar un picturebox
		int pulsarEliminar = 0;
		private void toolStripBorrar_Click(object sender, EventArgs e)
		{
			if (modoDibujo)
			{
				toolStripBorrar.BackColor = Color.Gray;
				if (pulsarEliminar == 1)
				{
					foreach (string llave in soloIDcomponentes)
					{
						DiccionarioComponentes[llave].Item1.Cursor = Cursors.SizeAll;
						DiccionarioComponentes[llave].Item1.Cursor.Tag = "MOVER";
					}
					pulsarEliminar = 0;
					TimerCursorEliminar.Stop();
					toolStripBorrar.BackColor = toolStrip1.BackColor;
					return;
				}
				pulsarEliminar++;
				// Al presionar el boton eliminar cambia el icono del cursor de los componentes, se presiona un componente se elimina y el cursor vuelve a la normalidad
				// si se presiona otra cosa, no pasa nada y el cursor vuelve a la normalidad tambien.

				// Recorrer todos los componentes
				if (soloIDcomponentes.Count == 0)
				{
					// Si la lista esta vacia, manda un mensaje
					MessageBox.Show("No hay Componente que eliminar", "Aviso");
				}
				else
				{
					// Se implemento un timer dado que el handle asociado a la imagen del cursor se elimina, luego de un tiempo por que el programa considera que no esta siendo utilizado
					foreach (string llave in soloIDcomponentes)
					{
						DiccionarioComponentes[llave].Item1.Cursor = new Cursor(Properties.Resources.deleteCursor.Handle);
						DiccionarioComponentes[llave].Item1.Cursor.Tag = "ELIMINAR";
					}
					TimerCursorEliminar.Interval = 500;
					TimerCursorEliminar.Start();
					TimerCursorEliminar.Tick += (sender1, e1) =>
					{
						foreach (string llave in soloIDcomponentes)
						{
							DiccionarioComponentes[llave].Item1.Cursor = new Cursor(Properties.Resources.deleteCursor.Handle);
							DiccionarioComponentes[llave].Item1.Cursor.Tag = "ELIMINAR";
						}
					};

				}
			}
			else
			{
				MessageBox.Show("Para Eliminar un Componente por Favor, Active el Modo Dibujo", "Atención",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			

		}
		// para abrir un nuevo archivo, pregunta si quiere continuar y  borra los componentes
		private void toolStripNuevo_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("Si abre otro archivo, se quitara la configuracion actual\nPor su seguridad se han guardado los cambios actuales\n¿Desea continuar?",
				"Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				//eliminar 
				buttonStop.PerformClick();
				formMonitoreo.Close();
				formMonitoreo = new FormMonitoreo(this);
				formValidaciones.Close();
				formValidaciones = new FormValidaciones(this);

				contadorFilasAll = 0;

				// liberar (Dispose) los picture box actuales
				eliminarComponentes("BORRAR TODO");
				pathArchivoActual = "";
				labelArchivo.Text = "Archivo Actual: ";
				// Cerrar el form configuraciones si es que esta abierto
				if (FormConfigurar != null)
				{
					FormConfigurar.Close();
				}
			}
			else
			{
				return;
			}

		}
		//enviar el picturebox al Fondo
		private void toolStripEnviarFondo_Click(object sender, EventArgs e)
		{
			// se recorren todos los componentes en pantalla solo por que se asegura anteriormente que solo uno de ellos puede estar seleccionado
			foreach (string llave in soloIDcomponentes)
			{
				if (DiccionarioComponentes[llave].Item1.BorderStyle == BorderStyle.FixedSingle)
				{
					DiccionarioComponentes[llave].Item1.SendToBack();
					propiedadesDinamicasComponentes[llave][1] = "0";
					break;
				}
			}
		}
		//Enviar el picturebox al frente
		private void toolStripLlevarFrente_Click(object sender, EventArgs e)
		{
			foreach (string llave in soloIDcomponentes)
			{
				if(DiccionarioComponentes[llave].Item1.BorderStyle == BorderStyle.FixedSingle)
				{
					DiccionarioComponentes[llave].Item1.BringToFront();
					propiedadesDinamicasComponentes[llave][1] = "1";
					break;
				}
			}
		}
		// Activa o desactiva el modo dibujo
		int modoDibujoClick = 0;
		private void toolStripDibujo_Click(object sender, EventArgs e)
		{
			if (modoDibujoClick == 1)
			{
				modoDibujo = false;
				toolStripDibujo.BackColor = toolStrip1.BackColor;
				modoDibujoClick = 0;
				labelModo.Text = "Modo: ";// + indicar si esta conectado o no con una variable global mas adelante;
			}
			else if (modoDibujoClick == 0)
			{
				toolStripDibujo.BackColor = Color.Gray;
				modoDibujo = true;
				modoDibujoClick++;
				labelModo.Text = "Modo: Dibujo";
			}

		}

		// --- Menu de herramientas ---
		// Activa el toolstripnuevo
		private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStripNuevo_Click(sender, e);
		}
		// --- Activa el toolstrip Guardar --- 
		private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			guardarToolStripMenuItem_Click(sender, e);
		}
		// --- Activa el toolstrip Guardar Como --- 
		private void guardarcomoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStripGuardarComo_Click(sender, e);
		}
		// --- Cierra el programa ---
		private void salirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		// --- Formulario 1 ---
		// Si se cierra el formulario pregunta si desea guardar antes de salir, salir sin guardar o no salir.
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (soloIDcomponentes.Count != 0)
			{
				DialogResult MensajeResultado = MessageBox.Show("¿Desea Guardar Antes de Salir?", "Advertencia", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
				if (MensajeResultado == DialogResult.Yes)
				{
					GuardarPropiedades();
					if (comenzarLecturas)
					{
						comenzarLecturas = false;
						List<string> listaCOMtemporal = new List<string>();
						foreach (string puerto in ArduinoConexion.PuertosDisponibles.Keys)
						{
							if (listaCOMtemporal.Contains(puerto.Split('-')[0]) == false)
							{
								listaCOMtemporal.Add(puerto.Split('-')[0]);
								ArduinoConexion.PuertosDisponibles[puerto].Close();

							}
						}
					}
					comenzarLecturas = false;
					cerrrarPuertosCOM();
				}
				else if (MensajeResultado == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}

		}

		// --- Panel1 ---
		// se deseleccionan los PB al presionar el panel1
		private void panel1_Click(object sender, EventArgs e)
		{
			deseleccionarTodos();
		}

		public void mostrarFormGalerias(string IDgaleria)
		{
			if(FormConfigurar == null)//ya se llamo una vez al formulario, al cerrarlo setearlo como null denuevo
			{
				FormConfigurar = new Configuraciones(IDgaleria,this);
				FormConfigurar.FormClosed += FormAsociarSensores_FormClosed;
				FormConfigurar.Show();
			}
			
		}

		private void FormAsociarSensores_FormClosed(object sender, FormClosedEventArgs e)
		{
			FormConfigurar = null;
		}



		private void verGraficosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Graficos FormGraficos = new Graficos(this, soloIDcomponentes);
			FormGraficos.Show();
		}


		private void buttonConfig_Click(object sender, EventArgs e)
		{
			if (bloquearConfig)
			{
				MessageBox.Show("No se puede iniciar las configuraciones cuando se estan realizando mediciones!");
				return;
			}
			mostrarFormGalerias(null);
		}

		// Comenzar a tomar medidas de los sensores y enviar informacion al ventilador----------------------------------------------------------------------------------------------------------------
		List<string> listaCOMtemporal = new List<string>();
		int contadorFilasAll = 0;
		public async void buttonComenzar_Click(object sender, EventArgs e)
		{
			if(FormConfigurar != null)
			{
				MessageBox.Show("No se puede Iniciar la toma de muestras mientras el Formulario de Configuraciones este abierto");
				return;
			}
			// ------------------Realizar Validaciones--------------------
			// Eliminar Configuraciones que no existan
			actualizarDiccionariosClases();
			// Actualizar los puertos
			ArduinoConexion.obtenerPuertoCOM();

			// Verifica que existan puertos disponibles
			if(ArduinoConexion.PuertosDisponibles.Keys.Count == 0)
			{
				MessageBox.Show("No se ha encontrado ningun puerto COM disponible, conecte un dispositivo", "Problema con Puertos COM", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// Verifica que los puertos COM esten configurados
			if (ArduinoConexion.diccionarioConfigsCOM.Keys.Count == 0)
			{
				MessageBox.Show("No se ha encontrado ningun puerto COM Configurado, por favor Realice la configuracion de los puertos COM.", "Problema con Puertos COM", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Verificar que los puertos COM en MedicionesClass.medicionesDiccionario sean los mismos que los que estan en ArduinoConexion.diccionarioConfigsCOM, si no es asi, pedir que se configuren de nuevo
			foreach (string keyNombreMedicion in MedicionesClass.medicionesDiccionario.Keys)
			{
				if(ArduinoConexion.diccionarioConfigsCOM.ContainsKey(MedicionesClass.medicionesDiccionario[keyNombreMedicion][8]) == false)
				{
					MessageBox.Show("El Puerto: " + MedicionesClass.medicionesDiccionario[keyNombreMedicion][8] + ", de la Medicion: " + keyNombreMedicion + ". No Se encuentra Disponible, por favor Realice la Configuracion Nuevamente",
						"Problema con Puertos COM", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			// Eliminar Puertos que no esten disponibles que los puertos de ArduinoConexion.diccionarioConfigsCOM existan en ArduinoConexion.PuertosDisponibles
			List<string> eliminarCOMS = new List<string>();
			foreach (string keyPuertoCONFS in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				if (ArduinoConexion.PuertosDisponibles.ContainsKey(keyPuertoCONFS) == false)
				{
					MessageBox.Show("El Puerto: " + keyPuertoCONFS + " No esta conectado. Tienes que Conectarlo o Configurarlo de nuevo si lo quieres usar", "COM no Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
					if (MessageBox.Show("Desea continuar y Borrar el puerto " + keyPuertoCONFS, "COM no Encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						eliminarCOMS.Add(keyPuertoCONFS);
					}
					else
					{
						return;
					}

				}
			}

			foreach (string eliminarCOMSItemKey in eliminarCOMS)
			{
				ArduinoConexion.diccionarioConfigsCOM.Remove(eliminarCOMSItemKey);
			}

			//Recibir la informacion de los puertos COM
			// Abrir los puertos
			if(listaCOMtemporal.Count > 1)
			{
				listaCOMtemporal.Clear();
			}
			foreach (string puertoKey in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				if (listaCOMtemporal.Contains(puertoKey.Split('-')[0]) == false)
				{
					listaCOMtemporal.Add(puertoKey.Split('-')[0]);
					try
					{
						ArduinoConexion.PuertosDisponibles[puertoKey].Open();
					}
					catch
					{
						bloquearConfig = false;
						comenzarLecturas = false;
						MessageBox.Show("Error al intentar abrir el Puerto: " + puertoKey.Split('-')[0] + ". Compruebe que no esta siendo utilizado por otro programa.");
						return;
					}
				}
			}

			//asociar el ventilador a su consumo
			AsociarVentconConsumo();

			// Comenzar las lecturas, despues de las validaciones ----------------------------------------------------------------------------------------------------------------
			//desactivar/activar algunos botones u otros
			comenzarLecturas = true;
			buttonStop.Enabled = true;
			buttonValidaciones.Enabled = true;
			buttonModoManual.Enabled = true;
			buttonMonitoreo.Enabled = true;
			buttonComenzar.Enabled = false;
			//toolStrip1.Enabled = false;
			// Configurar las TAB del formMonitoreo segun el numero y nombre de los COM configurados.
			if (contadorFilasAll == 0)
			{
				formMonitoreo.agregarTABconDatasGridViewsporCOM();
				formMonitoreo.Show();
				crearTabConDataGridVIew();//form monitoreo
				formValidaciones.agregarComponentesEntrada();//en entradas no hay problemas
				incrementoMinimo();
				crearTimerSalidas();
			}

			// Obtener la informacion de los puertos
			bloquearConfig = true;

			comenzarLecturasOK(true);

			if(contadorFilasAll != 0)
			{
				retroAlimentarSalidasInicial();
			} 

		}

		//Crea las columnas de cada TAB en el datagruview que le corresponda segun su puerto, las columnas son las mediciones asociadas a el puerto
		string keyCOM_OK;
		
		public void crearTabConDataGridVIew()
		{

			foreach (string keyCOM in ArduinoConexion.PuertosDisponibles.Keys)
			{
				if (keyCOM.Contains("Recibir"))
				{
					
					keyCOM_OK = keyCOM.Split('-')[0];
					// Agregar Columnas (nombre y encabezado), para luego pder ir agregando las mediciones
					foreach (string llaveMedicion in MedicionesClass.medicionesDiccionario.Keys)
					{
						if (MedicionesClass.medicionesDiccionario[llaveMedicion][8] == keyCOM)//el mismo puerto COM (recibir)
						{
							string margnitudFisica = "[" + MedicionesClass.medicionesDiccionario[llaveMedicion][2] + "]";
							formMonitoreo.agregarColumnas("Columna-" + llaveMedicion, llaveMedicion + " " + margnitudFisica, keyCOM_OK);// configurar los encabezados primero
						}
					}

					//agregar por cada consumo asociado al ventilador una columna
					foreach (string llaveVent in diccionarioVentiladorConsumo.Keys)
                    {
						string consumoKey = diccionarioVentiladorConsumo[llaveVent];
						string margnitudFisicaConsumo = "[" +MedicionesClass.medicionesDiccionario[consumoKey][2] + "]";
						formMonitoreo.agregarColumnas("Columna-" + consumoKey, consumoKey + " " + margnitudFisicaConsumo, keyCOM_OK);

					}


					formMonitoreo.agregarColumnas("ColumnaCOM", "Puerto COM", keyCOM_OK);
					formMonitoreo.agregarColumnas("ColumnaDate", "Fecha y Hora", keyCOM_OK);

				}
			}
		}

		//Obtener la informacion de los puertos
		public void comenzarLecturasOK(bool comenzar)
		{
			// se configura cada puerto y se le asocia un evento cuando este recibe una informacion (en el caso del modbus no aplica)
			if (comenzar)
			{
				foreach (string keyCOM in listaCOMtemporal)
				{
					// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion1, valor1, dir2, val2
					//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6     ,   7   ,  8  ,   9
					if(ArduinoConexion.diccionarioConfigsCOM[keyCOM + "-Recibir"][0] == "ModBus")
                    {
						continue;
                    }
					string separadorRecibir = ArduinoConexion.diccionarioConfigsCOM[keyCOM + "-Recibir"][2];
					ArduinoConexion.PuertosDisponibles[keyCOM + "-Recibir"].DataReceived += (sender1, e1) =>
					{
						try
						{
							ValoresRecibidosClass.ValoresRecibidos.Add(ArduinoConexion.PuertosDisponibles[keyCOM + "-Recibir"].ReadLine() + separadorRecibir + keyCOM + "-Recibir" + separadorRecibir + DateTime.Now.ToString() + separadorRecibir);
						}
						catch { };
					};
				}
			}
			else
			{
				foreach (string keyCOM in listaCOMtemporal)
				{
					if (ArduinoConexion.diccionarioConfigsCOM[keyCOM + "-Recibir"][0] == "ModBus")
					{
						continue;
					}
					string separadorRecibir = ArduinoConexion.diccionarioConfigsCOM[keyCOM + "-Recibir"][2];
					ArduinoConexion.PuertosDisponibles[keyCOM + "-Recibir"].DataReceived -= (sender1, e1) =>
					{
						try
						{
							ValoresRecibidosClass.ValoresRecibidos.Add(ArduinoConexion.PuertosDisponibles[keyCOM + "-Recibir"].ReadLine() + separadorRecibir + keyCOM + "-Recibir" + separadorRecibir + DateTime.Now.ToString() + separadorRecibir);
						}
						catch { };
					};
				}
			}
			//while (comenzarLecturas)
			//{
			//	foreach (string puerto in listaCOMtemporal)
			//	{
			//		string separadorRecibir = ArduinoConexion.diccionarioConfigsCOM[puerto + "-Recibir"][2];
			//		ValoresRecibidosClass.ValoresRecibidos.Add(await ArduinoConexion.ObtenerLecturaUSB(puerto) + separadorRecibir + puerto + "-Recibir" + separadorRecibir);
			//	}
			//}
		}

		// suscrito al evento cuando se agrega un valor al ObservableCollection ValoresRecibidos
		public void ValoresRecibidos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			string stringPuro = e.NewItems[0].ToString();
			string comando = stringPuro.Substring(0, 4);
			string separadorRecibir = stringPuro.Substring(stringPuro.Length-1);
			string stringSinEscape = stringPuro.Replace("\r", "");


			Invoke(new MethodInvoker(() =>
			{
				aplicarFormatoDatosRecibidosySubirlos(stringSinEscape, separadorRecibir, comando);
			}));

		}


		// Recibir la informacion y plasmarla en el datagridview que corresponda
		StringToMath stringToMath = new StringToMath();
		DataGridView dataAludido = null;
		int numeroFilaAgregada = -1;
		public async void aplicarFormatoDatosRecibidosySubirlos(string stringRecibido, string caracterSeparadorRecibir, string comandoInicial)
		{
			//Subir la informacion al form Monitoreo
			string stringSinComando = stringRecibido.Replace(comandoInicial, "");
			List<string> filasRecibidas = stringSinComando.Substring(0, stringSinComando.Length - 1).Split(caracterSeparadorRecibir.ToCharArray()[0]).ToList();

			string puertoCOMRecibido = ""; 

			try
			{
				puertoCOMRecibido = filasRecibidas[filasRecibidas.Count - 2];//falla cuando no llega bien el string desde los arduinos, si pasa se salta ese mensaje
			}
			catch { return; }

			string comandoInicialConfigurado;
			string fechaHoraRecibido = filasRecibidas[filasRecibidas.Count - 1];

			try// aveces no se recibe todo el string
			{
				comandoInicialConfigurado = ArduinoConexion.diccionarioConfigsCOM[puertoCOMRecibido][1];
			}
			catch { return; }

			//verificar que todos los elementos sean numeros 
			for (int i = 0; i < filasRecibidas.Count - 2; i++)
			{
				float FlotanteTemp;
				if(float.TryParse(filasRecibidas[i], out FlotanteTemp) == false)
				{
					return;//se salta ese string y pasa al siguiente recibido
				}
			}
			// Identificar la medicion y asignarle los valores recibidos, luego usarla para actualizar el live marcador y todo lo demas
			if (comandoInicial == comandoInicialConfigurado)
			{
				//Asociar cada medicion recibida con una medicion configurada
				// segun el puerto dejar solo las mediciones donde el puerto es el mismo, aca quede --------------------------
				// Determinar las posiciones recibidas segun el puerto en la primera cadena recibida por el puerto, para luego usarlas en las demas
				//Actualizar los valorea a los Live Marcadores y agrega a los datagridviews
				string lectura;
				Tuple<int, DataGridView> tupla;
				tupla = await formMonitoreo.agregarFilaVacia(puertoCOMRecibido);
				numeroFilaAgregada = tupla.Item1;
				dataAludido = tupla.Item2;

				foreach (string keyMedicion in MedicionesClass.medicionesDiccionario.Keys)
				{
					if (puertoCOMRecibido == MedicionesClass.medicionesDiccionario[keyMedicion][8])
					{
						//si las filas recibidas son menores a la posicion del COM configurado se salta, se debio a un error de comunicacion
						if (filasRecibidas.Count < int.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][9]))
						{
							return;
						}
						string valorRecibidoPos1 = filasRecibidas[int.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][9]) - 1];
						if (valorRecibidoPos1.Contains("COM"))//verifica que no se cuele el COM al principio
						{
							return;
						}
						if (MedicionesClass.medicionesDiccionario[keyMedicion][10] != "")//Para el caso de un componente
						{
							string valorRecibidoPos2 = filasRecibidas[int.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][10]) - 1];
							if (valorRecibidoPos2.Contains("COM"))
							{
								return;
							}
							String areagal = propiedadesDinamicasComponentes[MedicionesClass.medicionesDiccionario[keyMedicion][3]][10].Split(';')[1];
							lectura = stringToMath.stringToResultadoMath(MedicionesClass.medicionesDiccionario[keyMedicion][11], valorRecibidoPos1, 4,
										areagal, valorRecibidoPos2);//primero hacer la validacion en la form configurar componente
						}
						else//Para el caso de dos componentes
						{
							String areagal = propiedadesDinamicasComponentes[MedicionesClass.medicionesDiccionario[keyMedicion][3]][10].Split(';')[1];

							lectura = stringToMath.stringToResultadoMath(MedicionesClass.medicionesDiccionario[keyMedicion][11], filasRecibidas[int.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][9])-1], 4,
										areagal);//primero hacer la validacion en la form configurar componente
						}
						if (DiccionarioComponentes.ContainsKey("LIVE-" + keyMedicion))
						{
							DiccionarioComponentes["LIVE-" + keyMedicion].Item1.Controls[0].Text = lectura + " " + MedicionesClass.medicionesDiccionario[keyMedicion][2];
							double maximoOK = double.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][7]);
							double minimoOK = double.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][6]);
							if (double.Parse(lectura) > maximoOK || double.Parse(lectura) < minimoOK)
							{
								pintarLetraLiveMarcador(keyMedicion, "R");
							}
							else
							{
								pintarLetraLiveMarcador(keyMedicion, "N");

							}
						}
						if(DiccionarioComponentes.ContainsKey("LIVE-" + keyMedicion))
						{
							DiccionarioComponentes["LIVE-" + keyMedicion].Item1.Controls[0].Text = lectura + " " + MedicionesClass.medicionesDiccionario[keyMedicion][2];
						}

						dataAludido.Rows[numeroFilaAgregada].Cells["Columna-" + keyMedicion].Value = lectura;//al primer intento a veces no encuentra la columna
						//agrega el valor actual del consumo
						
						foreach (string llaveVent in diccionarioVentiladorConsumo.Keys)
                        {
							string llaveConsumo = diccionarioVentiladorConsumo[llaveVent];
							string valorConsumoActual = DiccionarioComponentes["LIVE-" + llaveConsumo].Item1.Controls[0].Text.Split(' ')[0];//como obtener el valor actual del consumo de otra manera
							float esnumer;
							bool esNumero = true;
							if(float.TryParse(valorConsumoActual, out esnumer))
                            {
								dataAludido.Rows[numeroFilaAgregada].Cells["Columna-" + llaveConsumo].Value = valorConsumoActual;
							}

							
						}
					}
				}
				dataAludido.Rows[numeroFilaAgregada].Cells["ColumnaCOM"].Value = puertoCOMRecibido;
				dataAludido.Rows[numeroFilaAgregada].Cells["ColumnaDate"].Value = fechaHoraRecibido;
				contadorFilasAll++;
				if(contadorFilasAll > 100)
				{
					contadorFilasAll = 2;
				}
			}


			// retroalimentar Compuertas y Ventiladores (enviar el comando )-----------------------------------------------------------------------------------------------
			//despues de recibir la primera medicion retroalimentar las salidas 

			//primera medicion, usada para retrolimentacion de salida
			if (contadorFilasAll == 1)//ejecutar solo una vez
			{
				retroAlimentarSalidasInicial();

			}
		}


		// Funcion que crea los timers segun los distientos tiempos de espera por cada tiempo de espera distinto un timer nuevo y ademas crea una lista con los valores de salida actuales (se basa en el valor incial configurado)
		List<int> posicionesSalidasOrdenados = new List<int>();
		Dictionary<int,string> valoresSegunPosicionSalida = new Dictionary<int, string>();
		List<string> componentesConSalida = new List<string>();
		List<string> listavaloresSalidasActualesOK = new List<string>();
		//numero de timers por cada tiempo diferente en las configuraciones se crea un nuevo timer
		Dictionary<int, Timer> segundosTimerDiccionario = new Dictionary<int, Timer>();
		private void retroAlimentarSalidasInicial()
		{
			// numeros de timers a crear: cada uno por un numero diferente de tiempo de espera (este tiempo es el que se dewbe esperar para verificar si el cambio surgio efecto en la medicion que esta sobre o bajo el limite
			segundosTimerDiccionario.Clear();
			foreach (List<string> listaCondiciones in CondicionesClass.CondicionesListaAumentar)//Aumentar
			{
				if (segundosTimerDiccionario.ContainsKey(int.Parse(listaCondiciones[4])) == false)
				{
					Timer timerTemp = new Timer();
					timerTemp.Interval = int.Parse(listaCondiciones[4])*1000;
					segundosTimerDiccionario.Add(int.Parse(listaCondiciones[4]), timerTemp);

					timerTemp.Tick += (sender, e) => 
					{
						actualizarValoresSalidaSegunMediciones(timerTemp.Interval);
					};
				}
			}
			foreach (List<string> listaCondiciones in CondicionesClass.CondicionesListaDisminuir)//Disminuir
			{
				if (segundosTimerDiccionario.ContainsKey(int.Parse(listaCondiciones[4])) == false)
				{
					Timer timerTemp = new Timer();
					timerTemp.Interval = int.Parse(listaCondiciones[4]);
					segundosTimerDiccionario.Add(int.Parse(listaCondiciones[4]), timerTemp);
					timerTemp.Tick += (sender, e) =>
					{
						actualizarValoresSalidaSegunMediciones(timerTemp.Interval);
					};
				}
			}

			// Identifica los componentes que envian comandos y guarda la key y // agregar una cantidad de elementos como posiciones a la lista
			int cantidadModbus = 0;
			foreach (string keyMedicion in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (MedicionesClass.medicionesDiccionario[keyMedicion][8].Contains("-Enviar") && componentesConSalida.Contains(keyMedicion) == false)
				{
					componentesConSalida.Add(keyMedicion);
					if(ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyMedicion][8]][0] == "ModBus")
					{
						cantidadModbus++;
						//posicionesSalidasOrdenados.Add(cantidadModbus);//duda
						//valoresSegunPosicionSalida.Add(cantidadModbus, MedicionesClass.medicionesDiccionario[keyMedicion][12]);// se asigna el valor inicial a la posicion de salida

					}
					else
					{
						posicionesSalidasOrdenados.Add(int.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][9]));//duda
						valoresSegunPosicionSalida.Add(int.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][9]), MedicionesClass.medicionesDiccionario[keyMedicion][12]);// se asigna el valor inicial a la posicion de salida

					}
				}
			}
			//posicionesSalidasOrdenados.Sort();
			//agregar el valor inicial a la lista global
			int nummaximo = 0;
			if (posicionesSalidasOrdenados.Count > 0)
			{
				nummaximo = posicionesSalidasOrdenados[posicionesSalidasOrdenados.Count - 1];
			}

			

			for (int i = 0; i < nummaximo; i++)
			{
				listavaloresSalidasActualesOK.Add("0");
			}
			// nose 
			foreach (int indice in posicionesSalidasOrdenados)
			{
				listavaloresSalidasActualesOK[indice-1] = valoresSegunPosicionSalida[indice];
			}

			//setea el valor inicial en el sensor por el puerto y en el panel (incremento igual a cero)
			foreach (string keyComponenteSalida in componentesConSalida)
			{
				incrementar_disminuir(keyComponenteSalida, true, 0);
			}

			//activar los timers
			activarTimerSalidas(true);
		}
		//Funcion que activa los timers
		public void activarTimerSalidas(bool activar)
		{
			foreach (int segundos in segundosTimerDiccionario.Keys)
			{
				if (activar)
				{
					segundosTimerDiccionario[segundos].Start();
				}
				else
				{
					segundosTimerDiccionario[segundos].Stop();
				}
			}

			foreach (int intervalo in timerOptimizacion.Keys)
			{
				if (activar)
				{
					timerOptimizacion[intervalo].Start();
				}
				else
				{
					timerOptimizacion[intervalo].Stop();
				}
			}

		}

		// Funcion que se ejecuta solo con los ticks y se encarga de ver si se exceden los condiciones para incrementar o disminuir los componentes de salidas (como ventiladores y compuertas)
		bool seIncrementoDisminiyo = false;
		private void actualizarValoresSalidaSegunMediciones(int tiempoIntervalo)
		{
			for (int i = 0; i < componentesConSalida.Count; i++)
			{
				double aumentar = 0;
				double disminuir = 0;
				// Enviar los comandos para subir/aumentar el componente de salida (considerando el maximo y minimo de ese componente) Por defecto es el minimo
				Dictionary<string, string> valoresActualesComponentesSalida = new Dictionary<string, string>();
				foreach (List<string> listaCondiciones in CondicionesClass.CondicionesListaAumentar)
				{
					if (int.Parse(listaCondiciones[4]) == tiempoIntervalo/1000)
					{
						if (listaCondiciones[1] == componentesConSalida[i])//si la condicion actual se corresponde con el componente de salida a evaluar
						{
							if (listaCondiciones[3].Split(' ')[1] == "Maximo")
							{
								double medicionActualvalor = medicionActual(listaCondiciones[0]);
								if (medicionActualvalor > double.Parse(listaCondiciones[3].Split(' ')[3]))
								{
									formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "Aumentar " + listaCondiciones[5], "");
									if(aumentar < double.Parse(listaCondiciones[5]))
									{
										aumentar = double.Parse(listaCondiciones[5]);
									}
									//incrementar_disminuir(componentesConSalida[i], true, double.Parse(listaCondiciones[5]));
									//seIncrementoDisminiyo = true;
									//return;
								}
								else
								{
									//formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "OK", "OK");
								}
							}
							else//minimo
							{
								double medicionActualvalor = medicionActual(listaCondiciones[0]);
								if (medicionActualvalor < double.Parse(listaCondiciones[3].Split(' ')[3]))
								{
									if (aumentar < double.Parse(listaCondiciones[5]))
									{
										aumentar = double.Parse(listaCondiciones[5]);
									}
									formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "Aumentar " + listaCondiciones[5], "");
									//incrementar_disminuir(componentesConSalida[i], true, double.Parse(listaCondiciones[5]));
									//seIncrementoDisminiyo = true;
									//return;
								}
								else
								{
									//formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "OK", "OK");
								}
							}
						}
					}
				}


				//if (seIncrementoDisminiyo)
				//{
				//	seIncrementoDisminiyo = false;
				//	return;
				//}

				// Enviar los comandos para bajar/disminuir el componente de salida (considerando el maximo y minimo de ese componente)
				foreach (List<string> listaCondiciones in CondicionesClass.CondicionesListaDisminuir)
				{
					if (int.Parse(listaCondiciones[4]) == tiempoIntervalo/1000)
					{
						if (listaCondiciones[1] == componentesConSalida[i])//si la condicion actual corresponde al componente de salida a evaluar
						{
							if (listaCondiciones[3].Split(' ')[1] == "Maximo")
							{
								double medicionActualvalor = medicionActual(listaCondiciones[0]);
								if (medicionActualvalor > double.Parse(listaCondiciones[3].Split(' ')[3]))
								{
									formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "" ,"Disminuir " + listaCondiciones[5]);
									if (disminuir < double.Parse(listaCondiciones[5]))
									{
										disminuir = double.Parse(listaCondiciones[5]);
									}
									//incrementar_disminuir(componentesConSalida[i], false, double.Parse(listaCondiciones[5]));
									//return;
								}
								else
								{
									//formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "OK","OK");
								}


							}
							else//minimo
							{
								double medicionActualvalor = medicionActual(listaCondiciones[0]);
								if (medicionActualvalor < double.Parse(listaCondiciones[3].Split(' ')[3]))//
								{
									formValidaciones.actualizarMedicion(listaCondiciones[0], medicionActualvalor.ToString(), "", "Disminuir " + listaCondiciones[5]);
									if (disminuir < double.Parse(listaCondiciones[5]))
									{
										disminuir = double.Parse(listaCondiciones[5]);
									}
									//incrementar_disminuir(componentesConSalida[i], false, double.Parse(listaCondiciones[5]));
									//return;
								}
								else
								{

								}
							}
						}
					}
				}

				// si no hay un incremento o disminucion se sale y configura como OK la validacion
				if (aumentar == 0 && disminuir == 0)
				{
					foreach (string llaveentrada in MedicionesClass.medicionesDiccionario.Keys)
					{
						if (MedicionesClass.medicionesDiccionario[llaveentrada][8].Contains("-Recibir"))
						{
							formValidaciones.actualizarMedicion(llaveentrada, medicionActual(llaveentrada).ToString(), "OK", "OK");
						}

					}
					return;
				}
				//si hay un incrmento o disminucion, la aplica segun corresponda
				if(aumentar >= disminuir)
				{
					incrementar_disminuir(componentesConSalida[i], true, aumentar);
				}
				else
				{
					incrementar_disminuir(componentesConSalida[i], false, disminuir);
				}


			}
		}


		//pinta de color la letra de live marcador segun su valor rojo si sobrepasa los valores minimos/maximo y negro si esta dentro de los parametros
		private void pintarLetraLiveMarcador(string keyMedicion, string color)
		{
			if(DiccionarioComponentes.ContainsKey("LIVE-" + keyMedicion))
			{
				if(color == "R")
				{
					DiccionarioComponentes["LIVE-" + keyMedicion].Item1.Controls[0].ForeColor = Color.Red;
				}
				else if(color == "N")
				{
					DiccionarioComponentes["LIVE-" + keyMedicion].Item1.Controls[0].ForeColor = Color.Black;
				}
			}
		}


		//obtiene la medicion actual del componente que se requiera
		public double medicionActual(string MedicionNombreKeyEntrada)
		{
			// encontrar segun el puerto COM y el nombre de la columna la medicion actual
			string puertoCOM = MedicionesClass.medicionesDiccionario[MedicionNombreKeyEntrada][8];
			double prueba;
			if(double.TryParse(formMonitoreo.ValorCelda(puertoCOM, "Columna-" + MedicionNombreKeyEntrada),out prueba))
			{
				return prueba;
			}
			else
			{
				return (double.Parse(MedicionesClass.medicionesDiccionario[MedicionNombreKeyEntrada][6]) + double.Parse(MedicionesClass.medicionesDiccionario[MedicionNombreKeyEntrada][7])) / 2;
			}
		}

		//Incrementa o disminuye el valor de un componente de salida y actualiza el valor que se incremento (o disminuyo) en la clase medicciones (aca se manda la orden al componente de salida)
		private void incrementar_disminuir(string keyComponenteSalida, bool incrementar, double valorIncremento)
		{
			//encontrar el valor actual de todos los componentes de salida
			double valorActual;
			if (MedicionesClass.medicionesDiccionario[keyComponenteSalida][14] != "")
			{
				valorActual = double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][12]) + double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][14]);
			}
			else//igual a ""
			{
				valorActual = double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][12]);
			}
			// Asignar el nuevo valor segun el incremento/disminucion configurado
			if (incrementar == false)
			{
				valorIncremento = -1 * valorIncremento;
			}

			double nuevoValor = valorActual + valorIncremento;
			// Verificar que el valor actual no sobrepase los limites y si los sobrepasa asignarle el el valor limite
			double valorMaximo = double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][7]);
			double valorMinimo = double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][6]);
			double acumulado;
			if (nuevoValor > valorMaximo)
			{
				nuevoValor = valorMaximo;
				valorIncremento = valorMaximo - double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][12]);
				acumulado = valorIncremento;
			}
			else if (nuevoValor < valorMinimo)
			{
				nuevoValor = valorMinimo;
				valorIncremento = valorMinimo -double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][12]);
				acumulado = valorIncremento;
			}
			else
			{
				double acumuladoActual = 0;
				if(MedicionesClass.medicionesDiccionario[keyComponenteSalida][14] == "")
				{
					acumuladoActual = 0;
				}
				else
				{
					acumuladoActual = double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][14]);
				}
				acumulado = acumuladoActual + valorIncremento;
			}

			//Actualizar el valor en mediccionesClass
			MedicionesClass.medicionesDiccionario[keyComponenteSalida][14] = acumulado.ToString();
			//enviarComandoSegunPosicionyPuerto(keyComponenteSalida, nuevoValor);//este es el comando en si que envia el comando
			enviarComandoSegunPosicionyPuerto(keyComponenteSalida, nuevoValor);//este es el comando en si que envia el comando
		}


		//asociar el ventilador a su consumo
		Dictionary<string, string> diccionarioVentiladorConsumo = new Dictionary<string, string>();
		void AsociarVentconConsumo()
        {
			string llaveVent = "";
			string ConsumoName = "";
			string componenteComun = "";

            foreach (string keyMediciones in MedicionesClass.medicionesDiccionario.Keys)
            {
                if (MedicionesClass.medicionesDiccionario[keyMediciones][4].Contains("VENT"))
				{
					componenteComun = MedicionesClass.medicionesDiccionario[keyMediciones][4];
					if (MedicionesClass.medicionesDiccionario[keyMediciones][8].Contains("-Enviar"))//si es enviar es Ventilador
					{
						llaveVent = keyMediciones;

						foreach (string keyMediciones2 in MedicionesClass.medicionesDiccionario.Keys)
						{
							if (MedicionesClass.medicionesDiccionario[keyMediciones2][8].Contains("-Recibir"))
							{
								if (componenteComun == MedicionesClass.medicionesDiccionario[keyMediciones][4])
								{
									ConsumoName = keyMediciones2;
									break;
								}
							}
						}

						if(diccionarioVentiladorConsumo.ContainsKey(llaveVent) == false)
                        {
							diccionarioVentiladorConsumo.Add(llaveVent, ConsumoName);
						}
					}
				}
			}
        }


		public async void enviarComandoSegunPosicionyPuerto(string keyComponenteSalida, double valorNuevo)
		{
			string protocolo = ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][0];
			string puertoCOM = MedicionesClass.medicionesDiccionario[keyComponenteSalida][8];
			string nombreComponenteVentilador = "";
			


			int posicion = 99;//no deberia dar problema por que para modbus no se usa
			string comandoInicial = "";

			string valorEscritura;
			if (protocolo != "ModBus")
			{
				posicion = int.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][9]);
				
				comandoInicial = ArduinoConexion.diccionarioConfigsCOM[puertoCOM][3];
			}
			
			double valorMaximo = double.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][7]);


			//EnviarComando

			string comandoSeparador = ArduinoConexion.diccionarioConfigsCOM[puertoCOM][4];


			//enviar comando al arduino o al modbus
			// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion
			//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6    
			bool actualizarPantalla = false;
			
			if(protocolo == "Personalizado")
			{
				ArduinoConexion.EnviarComando(comandoInicial + string.Join(comandoSeparador, listavaloresSalidasActualesOK.ToArray()), puertoCOM);
				actualizarPantalla = true;
			}
			else if(protocolo == "ModBus")
			{
				//Encontrar el nombre del la medicion Consumo (asociada al ventilador)
				nombreComponenteVentilador = MedicionesClass.medicionesDiccionario[keyComponenteSalida][4];




				// Aca se envia el valor al ventilador
				int IDEsclavo = int.Parse(ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][5]);
				int direccionModbus1 = int.Parse(ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][6]);
				int valorModBus1 = int.Parse(ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][7]);
				int direccionModbus2 = int.Parse(ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][8]);
				int valorModBus2 = int.Parse(ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][9]);

				// Antes de enviar el valor primero hay que transformarlo segun manual es en hertz, se multiplica por 10 y se redondea (debido a que no recibe decimales como input)
				// valor nuevo debe venir en RPM
				//se pasan las RPM al entero mas cercano

				valorEscritura = stringToMath.stringToResultadoMath(MedicionesClass.medicionesDiccionario[keyComponenteSalida][11], Math.Round(valorNuevo).ToString(), 0);

				int valorHaciaModbus;
				if(int.TryParse(valorEscritura, out valorHaciaModbus)== false)
				{
					throw new Exception("No se pudo convertir a entero la salida hacia el ventilador " + MedicionesClass.medicionesDiccionario[keyComponenteSalida][0]);
				}

				//primero hay que encender el ventilador en la primera ejecucion
				SerialPort puertoModbus = ArduinoConexion.PuertosDisponibles[puertoCOM];
				puertoModbus.Close();
				if (puertoModbus.IsOpen == false)
				{
					puertoModbus.Open();
				}

				puertoModbus.BaudRate = 19200;

				//si el valor actual del modbus es el mismo que se envia no hay necesidad de actualizar
				bool enviadoOK = false;
				int contadorModbus = 0;
				if (ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][9] != valorHaciaModbus.ToString())
				{
					enviadoOK = await EscribirAModBus(IDEsclavo, direccionModbus2, valorHaciaModbus, puertoModbus);
					while (enviadoOK == false)
					{

						await Task.Delay(300);
						enviadoOK = await EscribirAModBus(IDEsclavo, direccionModbus2, valorHaciaModbus, puertoModbus);
						contadorModbus++;
						if (contadorModbus > 10)
						{
							enviadoOK = true;
						}
					}
					ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][9] = valorHaciaModbus.ToString();

				}
				contadorModbus = 0;
				if (valorModBus1 == 0)
				{
					if (puertoModbus.IsOpen == false)
					{
						puertoModbus.Open();
					}
					puertoModbus.BaudRate = 19200;

					enviadoOK = await EscribirAModBus(IDEsclavo, direccionModbus1, 1, puertoModbus);
					while (enviadoOK == false)
					{
						await Task.Delay(300);
						enviadoOK = await EscribirAModBus(IDEsclavo, direccionModbus1, 1, puertoModbus);
						contadorModbus++;
						if (contadorModbus > 10)
						{
							enviadoOK = true;
						}
					}
					ArduinoConexion.diccionarioConfigsCOM[MedicionesClass.medicionesDiccionario[keyComponenteSalida][8]][7] = "1";
				}
				// activar timer modbus
				timerModbus.Start();

				//if (enviadoOK == false)
				//{
				//	actualizarPantalla = false;
				//}
				//else
				//{
				//	actualizarPantalla = true;
				//}
				actualizarPantalla = true;

			}

			if (actualizarPantalla)//se actualiza la pantalla solo si el comando es enviado correctamente
			{
				// actualiza el live marcador correspondiente (si es ventilador tambien modifica la velocidad)
				if (DiccionarioComponentes.ContainsKey("LIVE-" + keyComponenteSalida))
				{
					
					//Actualizar la velocidad del ventilador en pantalla
					if (MedicionesClass.medicionesDiccionario[keyComponenteSalida][4].Contains("VENT"))
					{
						DiccionarioComponentes["LIVE-" + keyComponenteSalida].Item1.Controls[0].Text = valorNuevo + " " + MedicionesClass.medicionesDiccionario[keyComponenteSalida][2];
						double velocidadMaximaVent = 15;//modificar si se quiere algo mejor
						float velocidadActual = (float)(valorNuevo / valorMaximo * velocidadMaximaVent);
						DiccionarioComponentes[MedicionesClass.medicionesDiccionario[keyComponenteSalida][4]].Item2.velocidad = velocidadActual;

						//actualizar el consumo (si es que esta configurado)
						// obtener key del consumo
						string keyConsumo = diccionarioVentiladorConsumo[keyComponenteSalida];
						string valorConsumo = stringToMath.stringToResultadoMath(MedicionesClass.medicionesDiccionario[keyConsumo][11], valorNuevo.ToString());
						DiccionarioComponentes["LIVE-" + keyConsumo].Item1.Controls[0].Text = valorConsumo + " " + MedicionesClass.medicionesDiccionario[keyConsumo][2];

					}
					else if (MedicionesClass.medicionesDiccionario[keyComponenteSalida][4].Contains("COMPUERTA"))
					{
						DiccionarioComponentes["LIVE-" + keyComponenteSalida].Item1.Controls[0].Text = valorNuevo + " " + MedicionesClass.medicionesDiccionario[keyComponenteSalida][2];
						//GIRAR LA COMPUERTA
						DiccionarioComponentes[MedicionesClass.medicionesDiccionario[keyComponenteSalida][4]].Item1.Image =
							DiccionarioComponentes[MedicionesClass.medicionesDiccionario[keyComponenteSalida][4]].Item2.RotarBitmap(imagenParaComponente("COMPUERTA"), -(float)valorNuevo, true);
					}
				}
			}

		}

		//funcion que envia valores a modbus e indica si es correcto el envio o sufrio algun error
		async Task<bool> EscribirAModBus(int IDesclavo, int Direccion, int valoraEscribir, SerialPort puerto)
		{
			bool enviadoOK = false;
			string enviado;
			string respuesta;
			Tuple<byte[], byte[]> tuplaRespuesta = null;

			try
			{
				tuplaRespuesta = await modbusRTU.EscribirRegistroUnico(IDesclavo, Direccion, valoraEscribir, puerto);
				enviado = BitConverter.ToString(tuplaRespuesta.Item1).Replace("-", " ");
				respuesta = BitConverter.ToString(tuplaRespuesta.Item2).Replace("-", " ");
				if (modbusRTU.verificarCRC(tuplaRespuesta.Item2))
				{
					enviadoOK = true;
				}
				else
				{
					enviadoOK = false;
				}
					

			}
			catch
			{
				enviadoOK = false;
			}

			return enviadoOK;



		}

		//tick del modBusTimer
		private async void TimerModbus_Tick(object sender, EventArgs e)
		{
			//escribir la info actual cada vez (no puede ser con una separacion mayor a 3 segundos, si no el VF se cae setting 36)
			foreach (string llaveCOM in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				if(ArduinoConexion.diccionarioConfigsCOM[llaveCOM][0] == "ModBus")
				{
					// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion1, valor1, dir2, val2
					//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6     ,   7   ,  8  ,   9
					int idEsclavo = int.Parse(ArduinoConexion.diccionarioConfigsCOM[llaveCOM][5]);
					int dir2Modbus = int.Parse(ArduinoConexion.diccionarioConfigsCOM[llaveCOM][8]);
					int Valor2ActualModbus = int.Parse(ArduinoConexion.diccionarioConfigsCOM[llaveCOM][9]);

					SerialPort puertoModbus = ArduinoConexion.PuertosDisponibles[llaveCOM];

					try
					{
						Tuple<byte[], byte[]> tuplaRespuestaPower = await modbusRTU.ReadHoldingRegisters(idEsclavo, dir2Modbus, 1, puertoModbus);
					}
					catch (Exception)
					{
					}
					
				}
				
			}
			


		}

		//funcion que crea timer segun el numero de salidas para poder optimizarlas
		Dictionary<int, Timer> timerOptimizacion;
		public void crearTimerSalidas()
		{
			//obtener las configuraciones de
			Dictionary<string, List<string>> diccionarioConfigs = formValidaciones.obtenerConfiguraciones();
			//si el timer no existe añadirlo y activarlo 
			timerOptimizacion = new Dictionary<int, Timer>();
			foreach (string llaveSalida in diccionarioConfigs.Keys)
			{
				int IntervaloActual;
				if(int.TryParse(diccionarioConfigs[llaveSalida][1], out IntervaloActual)==false)
				{
					continue;//si no hay valores se salta a la prox salida 
				}
				if (timerOptimizacion.ContainsKey(IntervaloActual) == false)
				{
					Timer timerTemp = new Timer();
					timerTemp.Interval = IntervaloActual * 1000;
					timerTemp.Tick += (sender, e) => 
					{
						OptimizarSalida(timerTemp.Interval);
					};
					timerOptimizacion.Add(IntervaloActual, timerTemp);
				}
			}

		}

		//Funcion que segun la configuracion en el formulario validaciones incrementa (maximiza) o disminuye (minimiza) constantemente el componente de salida si es que todas las mediciones estan en valores correctos
		public void OptimizarSalida(int intervaloEjecucion)
		{
			// verifica que las mediciones de entrada actuales se encuentren dentro de sus limites
			foreach (string keyMedicion in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (MedicionesClass.medicionesDiccionario[keyMedicion][8].Contains("-Recibir"))
				{
					double min = double.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][6]);
					double max = double.Parse(MedicionesClass.medicionesDiccionario[keyMedicion][7]);
					double medicionActualTemp = medicionActual(keyMedicion);
                    //se salta si es ventilador o medicion de consumo
                    if (MedicionesClass.medicionesDiccionario[keyMedicion][4].Contains("VENT") == false)
                    {
						if (medicionActualTemp < min || medicionActualTemp > max)//if (medicionActualTemp < min || medicionActualTemp > max)
						{
							return;
						}
					}

				}
			}

			Dictionary<string, List<string>> diccionarioConfigs = formValidaciones.obtenerConfiguraciones();
			//Aplicar la correccion
			foreach (string llaveSalida in diccionarioConfigs.Keys)
			{
				if(int.Parse(diccionarioConfigs[llaveSalida][1]) == intervaloEjecucion / 1000)
				{
					if (diccionarioConfigs[llaveSalida][0] == "Maximizar")
					{
						incrementar_disminuir(llaveSalida, true, incrementosList.Min());
					}
					else if (diccionarioConfigs[llaveSalida][0] == "Minimizar")
					{
						incrementar_disminuir(llaveSalida, false, incrementosList.Min());
					}
				}
			}


		}

		//funcion que busca el minimo de los incrementos/disminuciones, ejecutar una sola vez, al comienzo
		List<double> incrementosList = new List<double>();
		private void incrementoMinimo()
		{
			incrementosList = new List<double>();
			foreach (List<string> item in CondicionesClass.CondicionesListaAumentar)
			{
				double incremento = double.Parse(item[5]);
				if(incrementosList.Contains(incremento) == false)
				{
					incrementosList.Add(incremento);
				}
			}
			foreach (List<string> item in CondicionesClass.CondicionesListaDisminuir)
			{
				double incremento = double.Parse(item[5]);
				if (incrementosList.Contains(incremento) == false)
				{
					incrementosList.Add(incremento);
				}
			}
		}
		
		//funcion que da el valor enable del buttonModoManual
		public bool modoManualButtonEstado()
		{
			if (buttonModoManual.Enabled)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async void buttonStop_Click(object sender, EventArgs e)
		{
			//apagar modbus
			foreach (string keyCOM in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				if (ArduinoConexion.diccionarioConfigsCOM[keyCOM][0] == "ModBus")
				{
					// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion1, valor1, dir2, val2
					//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6     ,   7   ,  8  ,   9
					int idEsclavo = int.Parse(ArduinoConexion.diccionarioConfigsCOM[keyCOM][5]);
					int dir1Modbus = int.Parse(ArduinoConexion.diccionarioConfigsCOM[keyCOM][6]);
					//abrir puerto darle 19200 de rate
					SerialPort puertoModbus = ArduinoConexion.PuertosDisponibles[keyCOM];
					if (puertoModbus.IsOpen == false)
					{
						puertoModbus.Open();
						puertoModbus.BaudRate = 19200;
					}
				
					Tuple<byte[], byte[]> tuplaRespuestaPower = await modbusRTU.EscribirRegistroUnico(idEsclavo, dir1Modbus, 0, puertoModbus);
					ArduinoConexion.diccionarioConfigsCOM[keyCOM][7] = "0";

				}
			}
			timerModbus.Stop();
			comenzarLecturas = false;
			bloquearConfig = false;
			comenzarLecturas = false;
			activarTimerSalidas(false);
			comenzarLecturasOK(false);
			cerrrarPuertosCOM();
			buttonComenzar.Enabled = true;
			buttonStop.Enabled = false;
			buttonValidaciones.Enabled = false;
			buttonModoManual.Enabled = false;
			buttonMonitoreo.Enabled = false;
			//toolStrip1.Enabled = true;



		}

		private void buttonMonitoreo_Click(object sender, EventArgs e)
		{
			formMonitoreo.Show();
		}

		FormModoManual formModoManual = null;
		private void buttonModoManual_Click(object sender, EventArgs e)
		{
			if(MessageBox.Show("Al Activar el Modo Manual, la Retroalimentacion entre Sensores y Componentes de salida se Perdera.\nSin Embargo al Salir de Este Modo, se Reinicia la Retroalimentacion.\n¿Desea Coninuar?",
				"Pregunta.",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			//Iniciar el modo manual
			if(formModoManual == null)
			{
				formModoManual = new FormModoManual(this);
				formModoManual.FormClosed += FormModoManual_FormClosed1;
				buttonModoManual.Enabled = false;
				activarTimerSalidas(false);
				formModoManual.Show();

			}
			//desactivar la retroalimentacion y mostrar el form modo manual
		}

		private void FormModoManual_FormClosed1(object sender, FormClosedEventArgs e)
		{
			formModoManual = null;
			buttonModoManual.Enabled = true;
			//restablecer la retroalimentacion
			activarTimerSalidas(true);
		}


		private void buttonValidaciones_Click(object sender, EventArgs e)
		{
			formValidaciones.Show();
		}
	}
}