using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Windows.UI.Xaml;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.Foundation;

namespace UnoDuoHey
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly MVx.Observable.Property<bool> _isDualMode;
        private readonly MVx.Observable.Property<string> _modeDescription;
        private readonly MVx.Observable.Property<bool> _screen1Visible;
        private readonly MVx.Observable.Property<Rect> _screen1Bounds;
        private readonly MVx.Observable.Property<bool> _screen2Visible;
        private readonly MVx.Observable.Property<Rect> _screen2Bounds;

        private IDisposable _behaviours;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            _isDualMode = new MVx.Observable.Property<bool>(false, nameof(IsDualMode), args => PropertyChanged?.Invoke(this, args));
            _modeDescription = new MVx.Observable.Property<string>(string.Empty, nameof(ModeDescription), args => PropertyChanged?.Invoke(this, args));
            _screen1Visible = new MVx.Observable.Property<bool>(true, nameof(Screen1Visible), args => PropertyChanged?.Invoke(this, args));
            _screen1Bounds = new MVx.Observable.Property<Rect>(Rect.Empty, nameof(Screen1Bounds), args => PropertyChanged?.Invoke(this, args));
            _screen2Visible = new MVx.Observable.Property<bool>(false, nameof(Screen2Visible), args => PropertyChanged?.Invoke(this, args));
            _screen2Bounds = new MVx.Observable.Property<Rect>(Rect.Empty, nameof(Screen2Bounds), args => PropertyChanged?.Invoke(this, args));
        }

        private IDisposable ShouldUpdateIsDualModelWhenSizeChanges(IObservable<SizeChangedEventArgs> sizeChanges)
        {
            return sizeChanges
                .Select(_ => DeviceHelper.Instance.IsDualMode)
                .Subscribe(_isDualMode);
        }

        private IDisposable ShouldUpdateModeDescriptionWhenIsDualModeChanges()
        {
            return _isDualMode
                .Select(b => b ? "Running in Dual Screen Mode" : "Running in Single Screen Mode")
                .Subscribe(_modeDescription);
        }

        private IDisposable ShouldUpdateScreenStatesWhenSizeChanges(IObservable<SizeChangedEventArgs> sizeChanges)
        {
            var screenStates = sizeChanges
                .Select(_ => DeviceHelper.Instance.GetScreenState())
                .Publish();

            return new CompositeDisposable(
                screenStates.Select(s => s.Screen1Visible).Subscribe(_screen1Visible),
                screenStates.Select(s => s.Screen1Bounds).Subscribe(_screen1Bounds),
                screenStates.Select(s => s.Screen2Visible).Subscribe(_screen2Visible),
                screenStates.Select(s => s.Screen2Bounds).Subscribe(_screen2Bounds),
                screenStates.Connect()
            );            
        }

        public void Activate(IObservable<SizeChangedEventArgs> sizeChanges)
        {
            _behaviours = new CompositeDisposable(
                ShouldUpdateIsDualModelWhenSizeChanges(sizeChanges),
                ShouldUpdateModeDescriptionWhenIsDualModeChanges(),
                ShouldUpdateScreenStatesWhenSizeChanges(sizeChanges)
            );
        }

        public void Deactivate()
        {
            if (_behaviours != null)
            {
                _behaviours.Dispose();
                _behaviours = null;
            }
        }

        public string DeviceType
        {
            get
            {
                return DeviceHelper.Instance.IsDualScreenDevice
                    ? "I'm running on a Duo!"
                    : "I'm running on boring old Android :0(";
            }
        }

        public bool IsDualMode => _isDualMode.Get();

        public string ModeDescription => _modeDescription.Get();

        public bool Screen1Visible => _screen1Visible.Get();
        public Rect Screen1Bounds => _screen1Bounds.Get();
        public bool Screen2Visible => _screen2Visible.Get();
        public Rect Screen2Bounds => _screen2Bounds.Get();
    }
}
