namespace SquareColumnReinforcementPicker
{
    public class RebarItem
    {
        public int Dn;
        public double Fn;
        public double Mn;

        /// <summary>
        /// Арматурный стержень
        /// dn - Номинальный диаметр стержня, мм
        /// fn - Номинальная площадь поперечного сечения, см2
        /// mn - Номинальная масса одного погонного метра стержня, кг
        /// </summary>
        /// <param name="dn">Номинальный диаметр стержня</param>
        /// <param name="fn">Номинальная площадь поперечного сечения</param>
        /// <param name="mn">Номинальная масса одного погонного метра стержня</param>
        public RebarItem(int dn, double fn, double mn)
        {
            Dn = dn;
            Fn = fn;
            Mn = mn;
        }
    }
}
