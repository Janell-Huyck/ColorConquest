namespace ColorConquest.Views;

public class HomePageContext
{
    public ColorConquest.Core.ViewModels.HomeViewModel Main { get; }
    public ColorConquest.Core.ViewModels.HomeAnimationViewModel Animation { get; }
    public HomePageContext(ColorConquest.Core.ViewModels.HomeViewModel main, ColorConquest.Core.ViewModels.HomeAnimationViewModel animation)
    {
        Main = main;
        Animation = animation;
    }
}
