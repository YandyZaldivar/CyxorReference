/*
  { Frameview } - Sistema de videoconferencia por imágenes
  Copyright (C) 2018 Alimatic
  Authors:  Ramón Menéndez
            Yandy Zaldivar
*/

namespace Frameview
{
    using Cyxor.Networking.Config;
    using Cyxor.Networking.Config.Client;

    class NetworkConfig : ClientConfig
    {
        int frameWidth = 240;
        [ConnectedModifiable(enabled: true)]
        public int FrameWidth
        {
            get => frameWidth;
            set => SetProperty(ref frameWidth, value);
        }

        int frameHeight = 160;
        [ConnectedModifiable(enabled: true)]
        public int FrameHeight
        {
            get => frameHeight;
            set => SetProperty(ref frameHeight, value);
        }
    }
}
/* { Frameview } - Sistema de videoconferencia por imágenes */
