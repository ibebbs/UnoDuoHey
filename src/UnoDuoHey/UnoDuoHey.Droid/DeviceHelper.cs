using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Device.Display;
using Windows.Foundation;

namespace UnoDuoHey.Droid
{
    public class DeviceHelper : IDeviceHelper
    {
        private readonly Activity _activity;
        private readonly ScreenHelper _screenHelper;

        public DeviceHelper(Activity activity)
        {
            _activity = activity;
            _screenHelper = new ScreenHelper();
        }

        public void Initialize()
        {
            //_activity.GetSystemService(Context.SensorService)
            _screenHelper.Initialize(_activity);
        }

        private Android.Graphics.Rect GetWindowRect()
        {
            var windowRect = new Android.Graphics.Rect();
            _activity.WindowManager.DefaultDisplay.GetRectSize(windowRect);
            return windowRect;
        }

        private double PixelsToDip(double px) => px / _activity?.Resources?.DisplayMetrics?.Density ?? 1;

        public IScreenState GetScreenState()
        {
            var pixelDensisty = _activity?.Resources?.DisplayMetrics?.Density ?? 1;

            var rotation = _screenHelper.GetRotation();
            var hinge = _screenHelper.GetHingeBounds();
            var windowRect = GetWindowRect();

            if (IsDualMode)
            {
                return new ScreenState
                {
                    Screen1Visible = true,
                    Screen1Bounds = new Rect(0, 0, PixelsToDip(hinge.Left), PixelsToDip(windowRect.Bottom)),
                    Screen2Visible = true,
                    Screen2Bounds = new Rect(PixelsToDip(hinge.Right), 0, PixelsToDip(windowRect.Right - hinge.Right), PixelsToDip(windowRect.Bottom))
                };
            }
            else
            {
                return new ScreenState
                {
                    Screen1Visible = true,
                    Screen1Bounds = new Rect(0, 0, PixelsToDip(windowRect.Right), PixelsToDip(windowRect.Bottom)),
                    Screen2Visible = false,
                    Screen2Bounds = Rect.Empty
                };
            }
        }

        public bool IsDualScreenDevice
        {
            get { return ScreenHelper.IsDualScreenDevice(_activity); }
        }

        public bool IsDualMode => _screenHelper.IsDualMode;

        public Orientation Orientation
        {
            get
            {
                return _screenHelper.GetRotation() switch
                {
                    SurfaceOrientation.Rotation0 => Orientation.Horizontal,
                    SurfaceOrientation.Rotation180 => Orientation.Horizontal,
                    SurfaceOrientation.Rotation90 => Orientation.Vertical,
                    SurfaceOrientation.Rotation270 => Orientation.Vertical,
                    _ => throw new ArgumentException("Unknown screen orientation!")
                };
            }
        }
    }
}