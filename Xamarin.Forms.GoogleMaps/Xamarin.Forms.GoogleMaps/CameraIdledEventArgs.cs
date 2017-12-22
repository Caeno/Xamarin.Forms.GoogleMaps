﻿using System;

namespace Xamarin.Forms.GoogleMaps
{
    public sealed class CameraIdledEventArgs : EventArgs
    {
        public CameraPosition Position { get;  }

        internal CameraIdledEventArgs(CameraPosition position)
        {
            this.Position = position;
        }
    }
}