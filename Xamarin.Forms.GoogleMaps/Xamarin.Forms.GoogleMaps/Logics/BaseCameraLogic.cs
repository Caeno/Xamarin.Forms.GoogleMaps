﻿using Xamarin.Forms.GoogleMaps.Internals;

namespace Xamarin.Forms.GoogleMaps.Logics
{
    internal abstract class BaseCameraLogic<TNativeMap> : IMapRequestDelegate
    {
        protected Map _map;
        protected TNativeMap _nativeMap;

        public float ScaledDensity { get; internal set; }

        public virtual void Register(Map map, TNativeMap nativeMap)
        {
            _map = map;
            _nativeMap = nativeMap;

            _map.OnMoveToRegion = OnMoveToRegionRequest;
            _map.OnMoveCamera = OnMoveCameraRequest;
            _map.OnAnimateCamera = OnAnimateCameraRequest;
            _map.OnMoveToUserLocation = OnMoveToUserLocationRequest;
            _map.OnAnimateToViewAngle = OnAnimateToViewAngleRequest;
            _map.OnPointForPosition = OnPointForPositionRequest;
        }

        public virtual void Unregister()
        {
            if (_map != null)
            {
                _map.OnAnimateCamera = null;
                _map.OnMoveCamera = null;
                _map.OnMoveToRegion = null;
                _map.OnMoveToUserLocation = null;
                _map.OnAnimateToViewAngle = null;
                _map.OnPointForPosition = null;
            }
        }

        public abstract void OnMoveToRegionRequest(MoveToRegionMessage m);
        public abstract void OnMoveCameraRequest(CameraUpdateMessage m);
        public abstract void OnAnimateCameraRequest(CameraUpdateMessage m);
        public abstract bool OnMoveToUserLocationRequest();
        public abstract void OnAnimateToViewAngleRequest(double a);
        public abstract Point OnPointForPositionRequest(Position position);
    }
}