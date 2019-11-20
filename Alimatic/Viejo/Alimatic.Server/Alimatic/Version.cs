/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.Reflection;

namespace Alimatic
{
    using Cyxor.Networking;

    public static class Version
    {
        public static string Value => Utilities.Reflection.GetCustomAssemblyAttribute
            <AssemblyInformationalVersionAttribute>(typeof(Version)).InformationalVersion ??
            typeof(Version).GetTypeInfo().Assembly.GetName().Version.ToString();
    }
}
/* { Alimatic.Server } */
