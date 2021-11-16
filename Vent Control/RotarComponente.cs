using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vent_Control
{	
	// definicion de la clase
	public class RotarComponente
	{
		// Miembros de la clase

		// Propiedades
		private Bitmap ImagenRotada;
		public float velocidad;
		private float Acumulador;
		public float Angulo;

		//Constructor de la clase
		public RotarComponente()
		{
			
		}

		// Metodo de la clase para rotar una imagen bitmap
		public Bitmap RotarBitmap(Bitmap TipoElementoImagen , float Angulo, bool compuerta = false)
		{
			Acumulador = Angulo * (1 + velocidad);
			if (Acumulador >= 360)
			{
				Angulo = 0f;
			}
			if (ImagenRotada != null)
			{
				ImagenRotada.Dispose();
			}

			ImagenRotada = new Bitmap(TipoElementoImagen.Width, TipoElementoImagen.Height);

			//create a new empty bitmap to hold rotated image
			ImagenRotada.SetResolution(TipoElementoImagen.HorizontalResolution, TipoElementoImagen.VerticalResolution);
			//make a graphics object from the empty bitmap
			Graphics g = Graphics.FromImage(ImagenRotada);
			//g.InterpolationMode = InterpolationMode.High; // duda
			if (compuerta)
			{
				g.TranslateTransform((float)TipoElementoImagen.Width/2, (float)TipoElementoImagen.Height);
			}
			else
			{
				g.TranslateTransform((float)TipoElementoImagen.Width / 2, (float)TipoElementoImagen.Height / 2);

			}
			//rotate the image
			g.RotateTransform(Acumulador);
			//move the image back
			if (compuerta)
			{
				g.TranslateTransform(-(float)TipoElementoImagen.Width / 2, -(float)TipoElementoImagen.Height);
			}
			else
			{
				g.TranslateTransform(-(float)TipoElementoImagen.Width / 2, -(float)TipoElementoImagen.Height / 2);

			}
			
			//draw passed in image onto graphics object
			g.DrawImage(TipoElementoImagen, new PointF(0, 0));
			g.Dispose();
			return ImagenRotada;


		}


	}
}
