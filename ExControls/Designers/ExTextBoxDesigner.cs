#if NETFRAMEWORK
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
#else
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Actions;
#endif

namespace ExControls.Designers;

internal class ExTextBoxDesigner : DesignerControlBase<ExTextBox>
{
    private DesignerActionListCollection _actionLists;

    public override DesignerActionListCollection ActionLists => _actionLists ??= new DesignerActionListCollection { new ExTextBoxDesignerActionList(ControlHost)};

    /// <summary>Gets the selection rules that indicate the movement capabilities of a component.</summary>
    /// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules" /> values.</returns>
    public override SelectionRules SelectionRules
    {
        get
        {
            if (Control is not ExTextBox control) 
                throw new InvalidOperationException();
            if (control.Multiline) 
                return SelectionRules.AllSizeable | SelectionRules.Moveable;
            return SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Moveable;
        }
    }

    private sealed class ExTextBoxDesignerActionList : DesignerActionListBase<ExTextBox>
    {
        public ExTextBoxDesignerActionList(ExTextBox host) : base(host)
        {
        }

        public bool Enabled
        {
            get => Host.Enabled;
            set
            {
                SetProperty(nameof(Enabled), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public bool ReadOnly
        {
            get => Host.ReadOnly;
            set
            {
                SetProperty(nameof(ReadOnly), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public bool Multiline
        {
            get => Host.Multiline;
            set
            {
                SetProperty(nameof(Multiline), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public bool DefaultStyle
        {
            get => Host.DefaultStyle;
            set
            {
                SetProperty(nameof(DefaultStyle), value);
                DesignerActionService.Refresh(Host);
            }
        }

        public char PasswordChar
        {
            get => Host.PasswordChar;
            set
            {
                SetProperty(nameof(PasswordChar), value);
                DesignerActionService.Refresh(Host);
            }
        }

        /*public void EditItems()
        {
            var editorServiceContext = typeof(ControlDesigner).Assembly.GetTypes()
                .Where(x => x.Name == "EditorServiceContext").FirstOrDefault();
            var editValue = editorServiceContext.GetMethod("EditValue",
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.Public);
            editValue.Invoke(null, new object[] { designer, Component, "Items" });
        }*/

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            var items = new DesignerActionItemCollection
            {
                new DesignerActionHeaderItem("Appearance"),
                new DesignerActionHeaderItem("Behavior"),
                new DesignerActionPropertyItem(nameof(PasswordChar), "Password char:", "Behavior"),
                new DesignerActionPropertyItem(nameof(Enabled), "Enabled", "Appearance"),
                new DesignerActionPropertyItem(nameof(ReadOnly), "Read only", "Appearance"),
                new DesignerActionPropertyItem(nameof(Multiline), "Multiline", "Appearance"),
                new DesignerActionPropertyItem(nameof(DefaultStyle), "Default style", "Appearance")
            };
            return items;
        }
    }
}