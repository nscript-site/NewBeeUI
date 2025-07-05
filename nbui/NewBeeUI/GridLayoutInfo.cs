namespace NewBeeUI;

public struct GridLayoutInfo
{
    public double ContentWidth { get; set; }
    public double ContentHeight { get; set; }
    public double ContainerWidth { get; set; }  
    public double ContainerHeight { get; set; }

    public double ItemHeight { get; set; }
    public double VSpace { get; set; }

    public int ItemsPerRow { get; set; }
    public int ItemsPerColumn { get; set; }

    public int ItemsPerPage => ItemsPerRow * ItemsPerColumn;

    public bool IsContentFullDisplay
    {
        get
        {
            return ContentHeight <= ContainerHeight;
        }
    }
}
