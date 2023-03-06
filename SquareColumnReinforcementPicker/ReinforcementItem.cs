namespace SquareColumnReinforcementPicker
{
    public class ReinforcementItem
    {
        public RebarItem AngleBarsItem { get; set; }

        public RebarItem FaceBarsItem { get; set; }
        public int FaceBarsCnt { get; set; }

        public ReinforcementItem(RebarItem angleBarsItem, RebarItem faceBarsItem, int faceBarsCnt)
        {
            AngleBarsItem = angleBarsItem;
            FaceBarsItem = faceBarsItem;
            FaceBarsCnt = faceBarsCnt;
        }
    }
}
