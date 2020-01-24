using Windows.Foundation;

namespace UnoDuoHey
{
    public interface IScreenState
    {
        bool Screen1Visible { get; }

        Rect Screen1Bounds { get; }

        bool Screen2Visible { get; }

        Rect Screen2Bounds { get; }
    }

    public class ScreenState : IScreenState
    {
        public bool Screen1Visible { get; set; }

        public Rect Screen1Bounds { get; set; }

        public bool Screen2Visible { get; set; }

        public Rect Screen2Bounds { get; set; }
    }

    public interface IDeviceHelper
    {
        bool IsDualScreenDevice { get; }

        bool IsDualMode { get; }

        IScreenState GetScreenState();
    }

    public static class DeviceHelper
    {
        public static IDeviceHelper Instance { get; set; }
    }
}
