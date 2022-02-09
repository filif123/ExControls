﻿using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using ExControls.Controls;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ExControls.Designers;

internal class ExControlDesigner : ControlDesigner
{
    private DesignerActionListCollection actionList;

    public override DesignerActionListCollection ActionLists
    {
        get
        {
            return actionList ??= new DesignerActionListCollection(new DesignerActionList[] { new ExControlDesignerActionList(this) });
        }
    }

    internal class ExControlDesignerActionList : DesignerActionList
    {
        public ExControlDesignerActionList(ControlDesigner designer) : base(designer.Component)
        {
            Designer = designer;
            Control = (IExControl)designer.Control;
        }

        protected ControlDesigner Designer { get; }
        protected IExControl Control { get; }

        public bool DefaultStyle
        {
            get => Control.DefaultStyle;
            set => TypeDescriptor.GetProperties(Component)[nameof(DefaultStyle)].SetValue(Component, value);
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            return new DesignerActionItemCollection
            {
                new DesignerActionPropertyItem(nameof(DefaultStyle), "Default style", ExCategoryAttribute.CategoryToString(CategoryType.Appearance))
            };
        }
    }
}