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
            _calculator = new MultiTouchCalculator();
            _calculator?.Start();
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

            var cumulativeScale = e.CumulativeManipulation.Scale;
            var cumulativeExpansion = e.CumulativeManipulation.Expansion;
            var cumulativeRotation = e.CumulativeManipulation.Rotation;
            var cumulativeRranslation = e.CumulativeManipulation.Translation;

            CumulativeScaleRun.Text = $"{cumulativeScale.X:0.0000} × {cumulativeScale.Y:0.0000}";
            CumulativeExpansionRun.Text = $"{cumulativeExpansion.X:0.0000} × {cumulativeExpansion.Y:0.0000}";
            CumulativeRotationRun.Text = $"{cumulativeRotation:0.0000}";
            CumulativeTranslationRun.Text = $"{cumulativeRranslation.X:0.0000} × {cumulativeRranslation.Y:0.0000}";

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

        private void ReferBorder_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            _calculator?.Complete();
            _calculator = null;
        }

        private MultiTouchCalculator? _calculator;

        private void ReferPanel_TouchDown(object sender, TouchEventArgs e)
        {
        }

        private void ReferPanel_TouchMove(object sender, TouchEventArgs e)
        {
            if (_calculator is null)
            {
                return;
            }

            var touchPoint = e.GetTouchPoint(RootPanel);
            var delta = _calculator.Move(e.TouchDevice.Id, touchPoint.Position);

            var scale = delta.Scale;
            var expansion = delta.Expansion;
            var rotation = delta.Rotation;
            var translation = delta.Translation;

            SelfScaleRun.Text = $"{scale.X:0.0000} × {scale.Y:0.0000}";
            SelfExpansionRun.Text = $"{expansion.X:0.0000} × {expansion.Y:0.0000}";
            SelfRotationRun.Text = $"{rotation:0.0000}";
            SelfTranslationRun.Text = $"{translation.X:0.0000} × {translation.Y:0.0000}";

            SelfScaleTransform.ScaleX *= scale.X;
            SelfScaleTransform.ScaleY *= scale.Y;
            SelfRotateTransform.Angle += rotation;
            SelfTranslateTransform.X += translation.X;
            SelfTranslateTransform.Y += translation.Y;

            SelfAccumulatedScaleRun.Text = $"{SelfScaleTransform.ScaleX:0.0000} × {SelfScaleTransform.ScaleY:0.0000}";
            SelfAccumulatedExpansionRun.Text = $"未计算";
            SelfAccumulatedRotationRun.Text = $"{SelfRotateTransform.Angle:0.0000}";
            SelfAccumulatedTranslationRun.Text = $"{SelfTranslateTransform.X:0.0000} × {SelfTranslateTransform.Y:0.0000}";
        }

        private void ReferBorder_TouchUp(object sender, TouchEventArgs e)
        {
            _calculator?.Up(e.TouchDevice.Id);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform.ScaleX = 1;
            ScaleTransform.ScaleY = 1;
            RotateTransform.Angle = 0;
            TranslateTransform.X = 0;
            TranslateTransform.Y = 0;

            SelfScaleTransform.ScaleX = 1;
            SelfScaleTransform.ScaleY = 1;
            SelfRotateTransform.Angle = 0;
            SelfTranslateTransform.X = 0;
            SelfTranslateTransform.Y = 0;
        }
    }
}
