using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Walterlv.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DebugBorder_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = RootPanel;
        }

        private void DebugBorder_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
        }

        private void DebugBorder_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var scale = e.DeltaManipulation.Scale;
            var expansion = e.DeltaManipulation.Expansion;
            var rotation = e.DeltaManipulation.Rotation;
            var translation = e.DeltaManipulation.Translation;

            ScaleRun.Text = $"{scale.X:0.0000} × {scale.Y:0.0000}";
            ExpansionRun.Text = $"{expansion.X:0.0000} × {expansion.Y:0.0000}";
            RotationRun.Text = $"{rotation:0.0000}";
            TranslationRun.Text = $"{translation.X:0.0000} × {translation.Y:0.0000}";

            ScaleTransform.ScaleX *= scale.X;
            ScaleTransform.ScaleY *= scale.Y;
            RotateTransform.Angle += rotation;
            TranslateTransform.X += translation.X;
            TranslateTransform.Y += translation.Y;

            AccumulatedScaleRun.Text = $"{ScaleTransform.ScaleX:0.0000} × {ScaleTransform.ScaleY:0.0000}";
            AccumulatedExpansionRun.Text = $"未计算";
            AccumulatedRotationRun.Text = $"{RotateTransform.Angle:0.0000}";
            AccumulatedTranslationRun.Text = $"{TranslateTransform.X:0.0000} × {TranslateTransform.Y:0.0000}";
        }
    }
}
