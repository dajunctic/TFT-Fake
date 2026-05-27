using System.Collections.Generic;

namespace Dajunctic
{
    public interface IGameBoard
    {
        List<IChampionUnit> GetAllUnits();
        IChampionUnit GetUnitAt(IHexPosition pos);
        void MoveUnit(IChampionUnit unit, IHexPosition newPos);
    }
}
