using System;

namespace ExControls
{
    //TODO WORK IN PROGRESS
    /// <summary>
    ///     Theme for Winforms application and its controls.
    /// </summary>
    internal class ExTheme
    {
        private ExStyle styleNormal;
        private ExStyle styleHover;
        private ExStyle styleSelected;
        private ExStyle styleDisabled;
        private ExStyle styleReadOnly;

        private bool defaultStyle;

        
        /// <summary>
        ///     Gets or sets
        /// </summary>
        public bool DefaultStyle
        {
            get => defaultStyle;
            set
            {
                if (value == defaultStyle)
                    return;
                defaultStyle = value;
                OnDefaultStyleChanged();
            }
        }

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public ExStyle StyleNormal
        {
            get => styleNormal;
            set
            {
                if (value == styleNormal)
                    return;
                styleNormal = value;
                OnStyleNormalChanged();
            }
        }

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public ExStyle StyleHover
        {
            get => styleHover;
            set
            {
                if (value == styleHover)
                    return;
                styleHover = value;
                OnStyleHoverChanged();
            }
        }

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public ExStyle StyleSelected
        {
            get => styleSelected;
            set
            {
                if (value == styleSelected)
                    return;
                styleSelected = value;
                OnStyleSelectedChanged();
            }
        }

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public ExStyle StyleDisabled
        {
            get => styleDisabled;
            set
            {
                if (value == styleDisabled)
                    return;
                styleDisabled = value;
                OnStyleDisabledChanged();
            }
        }

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public ExStyle StyleReadOnly
        {
            get => styleReadOnly;
            set
            {
                if (value == styleReadOnly)
                    return;
                styleReadOnly = value;
                OnStyleReadOnlyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> DefaultStyleChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> StyleNormalChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> StyleHoverChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> StyleSelectedChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> StyleDisabledChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> StyleReadOnlyChanged;


        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDefaultStyleChanged()
        {
            DefaultStyleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStyleNormalChanged()
        {
            StyleNormalChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStyleHoverChanged()
        {
            StyleHoverChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStyleSelectedChanged()
        {
            StyleSelectedChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStyleDisabledChanged()
        {
            StyleDisabledChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnStyleReadOnlyChanged()
        {
            StyleReadOnlyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ExAppTheme : ExTheme
    {
        private bool darkTitleBar;

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public bool DarkTitleBar
        {
            get => darkTitleBar;
            set
            {
                if (value == darkTitleBar)
                    return;
                darkTitleBar = value;
                OnDarkTitleBarChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> DarkTitleBarChanged;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDarkTitleBarChanged()
        {
            DarkTitleBarChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ExScrollableControlTheme : ExTheme
    {
        private bool darkScrollBars;

        /// <summary>
        ///     Gets or sets
        /// </summary>
        public bool DarkScrollBars
        {
            get => darkScrollBars;
            set
            {
                if (value == darkScrollBars)
                    return;
                darkScrollBars = value;
                OnDarkScrollBarChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> DarkScrollBarChanged;

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDarkScrollBarChanged()
        {
            DarkScrollBarChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class ExApplication
    {
        private static ExAppTheme _theme;

        /// <summary>
        /// 
        /// </summary>
        public static ExAppTheme Theme => _theme ??= new ExAppTheme();
    }
}
