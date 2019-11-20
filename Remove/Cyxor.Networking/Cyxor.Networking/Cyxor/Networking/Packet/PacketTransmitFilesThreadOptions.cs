/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace Cyxor.Networking
{
   // Resumen:
   //     La enumeración System.Net.Sockets.TransmitFileOptions define valores utilizados
   //     en solicitudes de transferencia de archivos.
   public enum PacketTransmitFilesThreadOptions
   {
      // Resumen:
      //     Utilice el subproceso predeterminado para procesar las solicitudes de transferencia
      //     de archivos largas.
      UseDefaultWorkerThread = 0,

      // Resumen:
      //     Utilice el subproceso del sistema para procesar las solicitudes largas de
      //     transferencia de archivos.
      UseSystemThread = 16,

      // Resumen:
      //     Utilice llamadas a procedimientos asincrónicos (APC) del kernel, en lugar
      //     de subprocesos de trabajo, para procesar las solicitudes largas de transferencia
      //     de archivos.Las solicitudes largas se definen como solicitudes que requieren
      //     más de una lectura del archivo o de una caché; la solicitud depende, por
      //     tanto, del tamaño del archivo y de la longitud especificada del paquete de
      //     envío.
      UseKernelApc = 32,
   }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
