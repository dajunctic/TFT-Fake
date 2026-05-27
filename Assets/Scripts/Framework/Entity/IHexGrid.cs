namespace Dajunctic
{
    public interface IHexMovable
    {
        IHexGrid GetHexGrid();
    }

    public interface IHexGrid
    {
        object GetAllMoveableHexes();
    }
}
