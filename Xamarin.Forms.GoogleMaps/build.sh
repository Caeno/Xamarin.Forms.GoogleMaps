#!/bin/bash
msbuild Xamarin.Forms.GoogleMaps.sln /t:Clean;Build /p:Configuration=Release
mono nuget pack
