using GalaSoft.MvvmLight.Command;
using MainApp.ViewModel.BaseClasses;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Ink;
using System.Windows.Input;


namespace MainApp.ViewModel
{
    using R = Properties.Resources;

    class MainViewModel : ViewModelBase
    {
        private static Bitmap bitmap;
        private Graphics graphics;
        private bool mouseDown;

        public MainViewModel()
        {
            bitmap = new Bitmap(280, 280);
            graphics = Graphics.FromImage(bitmap);
            mouseDown = false;
            Methods = new List<IMethod>();

            foreach (string file in Directory.GetFiles(R.Networks))
            {
                Methods.Add(new NeuralMethod(file));
            }
            foreach (string file in Directory.GetFiles(R.Knns))
            {
                Methods.Add(new KnnMethod(file));
            }
            foreach (string file in Directory.GetFiles(R.KnnsAug))
            {
                Methods.Add(new KnnAugMethod(file));
            }

            currentMethod = Methods[0];
            Strokes = new StrokeCollection();
        }

        private StrokeCollection strokes;
        public StrokeCollection Strokes
        {
            get => strokes;
            set => SetProperty(ref strokes, value);
        }

        private double? output;
        public double? Output
        {
            get => output;
            set => SetProperty(ref output, value);
        }

        public List<IMethod> Methods { get; }

        private IMethod currentMethod;
        public IMethod CurrentMethod
        {
            get => currentMethod;
            set => SetProperty(ref currentMethod, value);
        }

        private ICommand drawing;
        public ICommand Drawing
        {
            get
            {
                if (drawing == null)
                {
                    drawing = new RelayCommand<System.Windows.Point>(point => {
                        if (mouseDown == true)
                        {
                            graphics.FillEllipse(new SolidBrush(Color.Black), (float)point.X, (float)point.Y, 10, 10);
                        }
                    });
                }
                return drawing;
            }
        }

        private ICommand mouseDownEvent;
        public ICommand MouseDownEvent
        {
            get
            {
                if (mouseDownEvent == null)
                {
                    mouseDownEvent = new RelayCommand(() => mouseDown = true);
                }
                return mouseDownEvent;
            }
        }

        private ICommand mouseUpEvent;
        public ICommand MouseUpEvent
        {
            get
            {
                if (mouseUpEvent == null)
                {
                    mouseUpEvent = new RelayCommand(() => mouseDown = false);
                }
                return mouseUpEvent;
            }
        }

        private ICommand calculate;
        public ICommand Calculate
        {
            get
            {
                if (calculate == null)
                {
                    calculate = new RelayCommand(async () => Output = await CurrentMethod.Calculate(BitmapOperations.GetInput(bitmap)));
                }
                return calculate;
            }
        }

        private ICommand clear;
        public ICommand Clear
        {
            get
            {
                if (clear == null)
                {
                    clear = new RelayCommand(() => { Strokes = new StrokeCollection(); graphics.Clear(Color.Empty); Output = null; });
                }
                return clear;
            }
        }
    }
}
