
using Packages.CIty.Views.BuildingsInfoView.Scripts;

namespace City.GameControl
{
    public class FieldControl
    {
        private static int FieldXSize = 12;
        private static int FieldYSize = 12;
        
        private bool[,] _fieldInfo = new bool[12,12];

        public void InitField()
        {
            _fieldInfo = new bool[12,12];
        }
        
        public bool CanBuild(BuildingInfoModel building, int xPosition, int yPosition)
        {
            for (int x = 0; x < building.SizeX; x++)
            {
                for (int y = 0; y < building.SizeY; y++)
                {
                    int xCheck = x + xPosition;
                    int yCheck = y + yPosition;
                    
                    if (xCheck >= FieldXSize ||
                        yCheck >= FieldYSize ||
                        _fieldInfo[xCheck, yCheck])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool  OnBuild(BuildingInfoModel building, int xPosition, int yPosition)
        {
            if (!CanBuild(building, xPosition, yPosition))
                return false;
            
            
            for (int x = 0; x < building.SizeX; x++)
            {
                for (int y = 0; y < building.SizeY; y++)
                {
                    int xBuild = x + xPosition;
                    int yBuild = y + yPosition;
                    _fieldInfo[xBuild, yBuild] = true;
                }
            }

            return true;
        }

    }
}