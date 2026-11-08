// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ExControls;

internal sealed class HelpInfo
{
    public const int HlpFile = 1, HlpKeyword = 2, HlpNavigator = 3, HlpObject = 4;

    public HelpInfo(string helpfilepath)
    {
        HelpFilePath = helpfilepath;
        Keyword = "";
        Navigator = HelpNavigator.TableOfContents;
        Param = null;
        Option = HlpFile;
    }

    public HelpInfo(string helpfilepath, string keyword)
    {
        HelpFilePath = helpfilepath;
        Keyword = keyword;
        Navigator = HelpNavigator.TableOfContents;
        Param = null;
        Option = HlpKeyword;
    }

    public HelpInfo(string helpfilepath, HelpNavigator navigator)
    {
        HelpFilePath = helpfilepath;
        Keyword = "";
        Navigator = navigator;
        Param = null;
        Option = HlpNavigator;
    }


    public HelpInfo(string helpfilepath, HelpNavigator navigator, object param)
    {
        HelpFilePath = helpfilepath;
        Keyword = "";
        Navigator = navigator;
        Param = param;
        Option = HlpObject;
    }

    public int Option { get; }

    public string HelpFilePath { get; }

    public string Keyword { get; }

    public HelpNavigator Navigator { get; }

    public object? Param { get; }

    public override string ToString() => "{HelpFilePath=" + HelpFilePath + ", keyword =" + Keyword + ", navigator=" + Navigator + "}";
}
