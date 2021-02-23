using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ExControls
{
    public partial class ExVScrollBar : ScrollBar
    {
        //Our channel color that we will expose later
        protected Color moChannelColor = Color.Empty;

        //Our images for the scrollbar that we will expose later 
        protected Image moUpArrowImage = null;
        protected Image moDownArrowImage = null;
        protected Image moThumbArrowImage = null;
        protected Image moThumbTopImage = null;
        protected Image moThumbTopSpanImage = null;
        protected Image moThumbBottomImage = null;
        protected Image moThumbBottomSpanImage = null;
        protected Image moThumbMiddleImage = null;

        //Our properties that we will expose later 

        protected int moLargeChange = 10;
        protected int moSmallChange = 1;
        protected int moMinimum = 0;
        protected int moMaximum = 100;
        protected int moValue = 0;

        //Our private variables for internal use 
        private int nClickPoint;
        private int moThumbTop = 0;
        private bool moThumbDown = false;
        private bool moThumbDragging = false;

        //public new event EventHandler Scroll = null;
        //public event EventHandler ValueChanged = null;

        public ExVScrollBar()
        {
            InitializeComponent();
            InitInternal();
        }

        public ExVScrollBar(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            InitInternal();
        }

        private void InitInternal()
        {
            //j
        }

    }
}
