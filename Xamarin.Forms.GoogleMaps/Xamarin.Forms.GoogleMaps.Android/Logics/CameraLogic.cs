using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Java.Lang;
using Xamarin.Forms.GoogleMaps.Android.Extensions;
using Xamarin.Forms.GoogleMaps.Android.Logics;
using Xamarin.Forms.GoogleMaps.Internals;

using GCameraUpdateFactory = Android.Gms.Maps.CameraUpdateFactory;
using Xamarin.Forms.GoogleMaps.Android;

namespace Xamarin.Forms.GoogleMaps.Logics.Android
{
    internal sealed class CameraLogic : BaseCameraLogic<GoogleMap>
    {
        public override void OnMoveToRegionRequest(MoveToRegionMessage m)
        {
            if (_nativeMap == null)
                return;

            var span = m.Span;
            var animate = m.Animate;

            span = span.ClampLatitude(85, -85);
            var ne = new LatLng(span.Center.Latitude + span.LatitudeDegrees / 2, span.Center.Longitude + span.LongitudeDegrees / 2);
            var sw = new LatLng(span.Center.Latitude - span.LatitudeDegrees / 2, span.Center.Longitude - span.LongitudeDegrees / 2);
            var update = GCameraUpdateFactory.NewLatLngBounds(new LatLngBounds(sw, ne), 0);

            try
            {
                if (animate)
                    _nativeMap.AnimateCamera(update);
                else
                    _nativeMap.MoveCamera(update);
            }
            catch (IllegalStateException exc)
            {
                System.Diagnostics.Debug.WriteLine("MoveToRegion exception: " + exc);
            }
        }

        public override void OnMoveCameraRequest(CameraUpdateMessage m)
        {
            MoveCamera(m.Update);
            m.Callback.OnFinished();
        }

        internal void MoveCamera(CameraUpdate update)
        {
            _nativeMap.MoveCamera(update.ToAndroid(ScaledDensity));
        }

        public override void OnAnimateCameraRequest(CameraUpdateMessage m)
        {
            var update = m.Update.ToAndroid(ScaledDensity);
            var callback = new DelegateCancelableCallback(
                    () => m.Callback.OnFinished(),
                    () => m.Callback.OnCanceled());

            if (m.Duration.HasValue)
            {
                _nativeMap.AnimateCamera(update, (int)m.Duration.Value.TotalMilliseconds, callback);
            }
            else
            {
                _nativeMap.AnimateCamera(update, callback);
            }
        }

        public override bool OnMoveToUserLocationRequest()
        {
            var location = _nativeMap.MyLocation;
            if (location != null)
            {
                MoveCamera(new CameraUpdate(new Position(location.Latitude, location.Longitude)));
                return true;
            }

            return false;
        }

        public override void OnAnimateToViewAngleRequest(double a)
        {
            // TODO: Implement the logic for Camera Tilt in Android
        }

        public override Point OnPointForPositionRequest(Position position)
        {
            var nativePosition = _nativeMap.Projection.ToScreenLocation(position.ToLatLng());
            return new Point(nativePosition.X, nativePosition.Y);
        }

    }
}