using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Vent_Control
{
	public class ModBusRTU
	{
		public async Task<Tuple<byte[], byte[]>> EscribirRegistroUnico(int IDEsclavo, int direccionInicial, int valor, SerialPort puertoCOMModBus)
		{
			byte idEsclavo = BitConverter.GetBytes(IDEsclavo)[0];
			byte funcionModBus = BitConverter.GetBytes(6)[0];
			UInt16 direccion = (UInt16)direccionInicial;
			UInt16 valorBytes = (UInt16)valor;
			byte[] bytesAenviar = new byte[8];
			bytesAenviar[0] = idEsclavo;
			bytesAenviar[1] = funcionModBus;
			bytesAenviar[2] = BitConverter.GetBytes(direccionInicial)[1];
			bytesAenviar[3] = BitConverter.GetBytes(direccionInicial)[0];
			bytesAenviar[4] = BitConverter.GetBytes(valorBytes)[1];
			bytesAenviar[5] = BitConverter.GetBytes(valorBytes)[0];
			byte[] CRCOK = calculateCRC(bytesAenviar);
			bytesAenviar[6] = CRCOK[0];
			bytesAenviar[7] = CRCOK[1];
			puertoCOMModBus.Write(bytesAenviar, 0, bytesAenviar.Length);
			await Task.Delay(100);
			int numeroBytesEntrantes = puertoCOMModBus.BytesToRead;
			if (numeroBytesEntrantes == 0)
			{
				byte[] Vacio = new byte[1];
				byte[] Vacio2 = new byte[1];
				return new Tuple<byte[], byte[]>(Vacio, Vacio2);
			}
			else
			{
				byte[] bytesRespuesta = new byte[numeroBytesEntrantes];
				puertoCOMModBus.Read(bytesRespuesta, 0, numeroBytesEntrantes);
				return new Tuple<byte[], byte[]>(bytesAenviar, bytesRespuesta);
			}

		}

		public async Task<Tuple<byte[], byte[]>> ReadHoldingRegisters(int IDEsclavo, int direccionInicial, int numeroRegistros, SerialPort puertoCOMModBus)
		{
			byte idEsclavo = BitConverter.GetBytes(IDEsclavo)[0];
			byte funcionModBus = BitConverter.GetBytes(3)[0];
			UInt16 direccion = (UInt16)direccionInicial;
			UInt16 numeroRegistrosok = (UInt16)numeroRegistros;

			byte[] bytesAenviar = new byte[8];
			bytesAenviar[0] = idEsclavo;
			bytesAenviar[1] = funcionModBus;
			bytesAenviar[2] = BitConverter.GetBytes(direccionInicial)[1];
			bytesAenviar[3] = BitConverter.GetBytes(direccionInicial)[0];
			bytesAenviar[4] = BitConverter.GetBytes(numeroRegistrosok)[1];
			bytesAenviar[5] = BitConverter.GetBytes(numeroRegistrosok)[0];
			byte[] CRCOK = calculateCRC(bytesAenviar);
			bytesAenviar[6] = CRCOK[0];
			bytesAenviar[7] = CRCOK[1];
			puertoCOMModBus.DiscardInBuffer();
			puertoCOMModBus.DiscardOutBuffer();
			puertoCOMModBus.Write(bytesAenviar, 0, bytesAenviar.Length);
			await Task.Delay(180);
			int numeroBytesEntrantes = puertoCOMModBus.BytesToRead;
			if (numeroBytesEntrantes == 0)
			{
				byte[] Vacio = new byte[1];
				byte[] Vacio2 = new byte[1];
				return new Tuple<byte[], byte[]>(Vacio, Vacio2);
			}
			else
			{
				byte[] bytesRespuesta = new byte[numeroBytesEntrantes];
				puertoCOMModBus.Read(bytesRespuesta, 0, numeroBytesEntrantes);
				return new Tuple<byte[], byte[]>(bytesAenviar, bytesRespuesta);
			}
		}

		public bool verificarCRC(byte[] Recibido)
		{
			byte[] crcRecibido = new byte[2];
			bool resultado = true;
			if (Recibido.Length >= 2)
			{
				crcRecibido[0] = Recibido[Recibido.Length - 2];
				crcRecibido[1] = Recibido[Recibido.Length - 1];
			}
			else
			{
				return false;
			}
			byte[] crcCalculado = calculateCRC(Recibido);
			for (int i = 0; i < 2; i++)
			{
				if (crcCalculado[i] != crcRecibido[i])
				{
					resultado = false;
				}
			}
			return resultado;
		}

		private byte[] calculateCRC(byte[] data)
		{
			ushort CRCFull = 0xFFFF;
			char CRCLSB;
			byte[] CRC = new byte[2];
			for (int i = 0; i < (data.Length) - 2; i++)
			{
				CRCFull = (ushort)(CRCFull ^ data[i]); // 

				for (int j = 0; j < 8; j++)
				{
					CRCLSB = (char)(CRCFull & 0x0001);
					CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

					if (CRCLSB == 1)
						CRCFull = (ushort)(CRCFull ^ 0xA001);
				}
			}
			CRC[1] = (byte)((CRCFull >> 8) & 0xFF);
			CRC[0] = (byte)(CRCFull & 0xFF);
			return CRC;
		}

		public List<string> leerRegistros(Tuple<byte[], byte[]> tuplaRespuesta)
		{



			string respuestaModBus = BitConverter.ToString(tuplaRespuesta.Item2);
			List<string> listaRespuestas = respuestaModBus.Split('-').ToList();
			List<string> respuestas = new List<string>();
			if (verificarCRC(tuplaRespuesta.Item2))
			{
				string direccionEsclavo = listaRespuestas[0];
				string DireccionInicion = listaRespuestas[1];
				string funcion = listaRespuestas[2];
				listaRespuestas.RemoveAt(0);
				listaRespuestas.RemoveAt(0);
				listaRespuestas.RemoveAt(0);
				List<string> Results = new List<string>();
				int jj = 0;
				while (jj < listaRespuestas.Count - 1)
				{
					Results.Add(listaRespuestas[jj] + listaRespuestas[jj + 1]);
					jj = jj + 2;

				}
				decimal d;


				
				for (int i = 0; i < Results.Count; i++)
				{
					d = (decimal)Int64.Parse(Results[i], System.Globalization.NumberStyles.HexNumber);
					respuestas.Add(d.ToString());
				}

			}
			else
			{
				respuestas.Add("-99");
			}
			return respuestas;
		}
	}
}
