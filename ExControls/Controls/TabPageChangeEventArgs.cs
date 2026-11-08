// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace ExControls;

/// <summary>
/// Event arguments for the <see cref="ExTabControl.SelectedIndexChanging"/> event.
/// </summary>
public class TabPageChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets whether the event should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TabPageChangeEventArgs" /> class.
    /// </summary>
    /// <param name="currentTab">the selected tab page; <see langword="null" /> if no tab page is selected</param>
    /// <param name="newTab">the tab page to be selected</param>
    public TabPageChangeEventArgs(TabPage? currentTab, TabPage newTab)
    {
        CurrentTab = currentTab;
        NewTab = newTab;
    }

    /// <summary>
    /// Gets the current selected Tab.
    /// </summary>
    public TabPage? CurrentTab { get; }


    /// <summary>
    /// Gets the Tab that is to become current.
    /// </summary>
    public TabPage NewTab { get; }
}
