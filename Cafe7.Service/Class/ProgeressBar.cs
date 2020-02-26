using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Cafe7.Service.Class
{
   public static class ProgressBarExtensions
    {
   
            private static readonly TimeSpan Duration = TimeSpan.FromSeconds(1);

            public static void SetPercent(this ProgressBar progressBar, double percentage)
            {
                
                var animation = new DoubleAnimation(percentage, Duration);
                progressBar.BeginAnimation(RangeBase.ValueProperty, animation);
            }
       
    }
}
