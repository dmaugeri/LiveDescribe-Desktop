﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LiveDescribe.Controls
{
    public class AudioCanvas : Canvas
    {

        public enum SpacesActionState { None, Dragging, ResizingEndOfSpace, ResizingBeginningOfSpace };

        public static readonly DependencyProperty VideoDurationProperty =
          DependencyProperty.Register("VideoDuration", typeof(double), typeof(SpaceControl));

        public AudioCanvas() { }

        public double VideoDuration
        {
            get { return (double)GetValue(VideoDurationProperty); }
            set { SetValue(VideoDurationProperty, value); } 
        }

        public SpacesActionState CurrentSpaceActionState { get; set; }
    }
}