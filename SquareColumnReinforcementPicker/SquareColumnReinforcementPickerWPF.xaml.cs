using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SquareColumnReinforcementPicker
{
    public partial class SquareColumnReinforcementPickerWPF : Window
    {
        public SquareColumnReinforcementPickerWPF()
        {
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void SquareColumnReinforcementPickerWPF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                DialogResult = true;
                Close();
            }

            else if (e.Key == Key.Escape)
            {
                DialogResult = false;
                Close();
            }
        }

        private void button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            List<RebarItem> rebarList = new List<RebarItem>()
            {
                new RebarItem(16, 2.011, 1.578),
                new RebarItem(18, 2.545, 1.998),
                new RebarItem(20, 3.142, 2.466),
                new RebarItem(22, 3.801, 2.984),
                new RebarItem(25, 4.909, 3.853),
                new RebarItem(28, 6.158, 4.834),
                new RebarItem(32, 8.042, 6.313),
                new RebarItem(36, 10.179, 7.990),
                new RebarItem(40, 12.566, 9.865)
            };

            int populationSize = 1000;
            int maxIterations = 1000;
            double Ft;
            double.TryParse(textBox_FtText.Text, out Ft);

            double columnSection;
            double.TryParse(textBox_ColumnSectionText.Text, out columnSection);

            double coverLayer;
            double.TryParse(textBox_CoverLayerText.Text, out coverLayer);

            int preferredBarsCnt;
            string preferredBarsCntStr = (groupBox_PreferredBarsCnt.Content as Grid)
                .Children.OfType<RadioButton>()
                .FirstOrDefault(rb => rb.IsChecked.Value == true)
                .Name;
            int.TryParse(preferredBarsCntStr.Replace("rbt_",""), out preferredBarsCnt);

            List<ReinforcementItem> population = new List<ReinforcementItem>();

            Random rand = new Random();

            for (int i = 0; i < populationSize; i++)
            {
                RebarItem angleBarsItem = rebarList[rand.Next(rebarList.Count)];

                RebarItem faceBarsItem = rebarList[rand.Next(rebarList.Count)];
                int faceBarsCnt = rand.Next(0, 4);
                population.Add(new ReinforcementItem(angleBarsItem, faceBarsItem, faceBarsCnt));
            }

            List<ReinforcementItem> populationPreferredBarsCnt = population.ToList();

            // Основной цикл алгоритма
            for (int i = 0; i < maxIterations; i++)
            {
                // Сортировка популяции
                SortPopulation(population, Ft, columnSection, coverLayer);

                // Выбор лучших особей
                List<ReinforcementItem> newPopulation = new List<ReinforcementItem>();
                for (int j = 0; j < populationSize / 2; j++)
                {
                    ReinforcementItem parent1 = population[rand.Next(populationSize / 2)];
                    ReinforcementItem parent2 = population[rand.Next(populationSize / 2)];
                    ReinforcementItem child1 = new ReinforcementItem(parent1.AngleBarsItem, parent2.FaceBarsItem, parent2.FaceBarsCnt);
                    ReinforcementItem child2 = new ReinforcementItem(parent2.AngleBarsItem, parent1.FaceBarsItem, parent1.FaceBarsCnt);
                    newPopulation.Add(child1);
                    newPopulation.Add(child2);
                }

                // Мутация новой популяции
                for (int j = 0; j < populationSize / 10; j++)
                {
                    ReinforcementItem item = newPopulation[rand.Next(populationSize)];
                    item.FaceBarsCnt = rand.Next(0, 4);
                }

                population = newPopulation;
            }
            // Возвращение лучшей особи
            SortPopulation(population, Ft, columnSection, coverLayer);
            var targetReinforcementItem = population[0];
            double f = targetReinforcementItem.AngleBarsItem.Fn * 2 + targetReinforcementItem.FaceBarsItem.Fn * targetReinforcementItem.FaceBarsCnt;
            double m = targetReinforcementItem.AngleBarsItem.Mn * 4 + targetReinforcementItem.FaceBarsItem.Mn * targetReinforcementItem.FaceBarsCnt * 2;
            
            label_V1_AngleBarsItemDiamText.Content = targetReinforcementItem.AngleBarsItem.Dn.ToString();
            if(targetReinforcementItem.FaceBarsCnt != 0)
            {
                label_V1_FaceBarsCntText.Content = targetReinforcementItem.FaceBarsCnt.ToString();
                label_V1_FaceBarsDiamText.Content = targetReinforcementItem.FaceBarsItem.Dn.ToString();
            }
            else
            {
                label_V1_FaceBarsCntText.Content = "-";
                label_V1_FaceBarsDiamText.Content = "-";
            }

            label_V1_FrText.Content = Math.Round(f, 3);
            label_V1_MrText.Content = Math.Round(m, 2);

            // Основной цикл алгоритма для огранниченного колличества стержней
            for (int i = 0; i < maxIterations; i++)
            {
                // Сортировка популяции
                SortPopulationPreferredBarsCnt(populationPreferredBarsCnt, Ft, columnSection, coverLayer, preferredBarsCnt);

                // Выбор лучших особей
                List<ReinforcementItem> newPopulation = new List<ReinforcementItem>();
                for (int j = 0; j < populationSize / 2; j++)
                {
                    ReinforcementItem parent1 = populationPreferredBarsCnt[rand.Next(populationSize / 2)];
                    ReinforcementItem parent2 = populationPreferredBarsCnt[rand.Next(populationSize / 2)];
                    ReinforcementItem child1 = new ReinforcementItem(parent1.AngleBarsItem, parent2.FaceBarsItem, parent2.FaceBarsCnt);
                    ReinforcementItem child2 = new ReinforcementItem(parent2.AngleBarsItem, parent1.FaceBarsItem, parent1.FaceBarsCnt);
                    newPopulation.Add(child1);
                    newPopulation.Add(child2);
                }

                // Мутация новой популяции
                for (int j = 0; j < populationSize / 10; j++)
                {
                    ReinforcementItem item = newPopulation[rand.Next(populationSize)];
                    item.FaceBarsCnt = rand.Next(0, 4);
                }

                populationPreferredBarsCnt = newPopulation;
            }
            // Возвращение лучшей особи при заданном колличестве стержней
            SortPopulationPreferredBarsCnt(populationPreferredBarsCnt, Ft, columnSection, coverLayer, preferredBarsCnt);
            var targetReinforcementItemPreferredBarsCnt = populationPreferredBarsCnt[0];
            double fPreferredBarsCnt = targetReinforcementItemPreferredBarsCnt.AngleBarsItem.Fn * 2 + targetReinforcementItemPreferredBarsCnt.FaceBarsItem.Fn * targetReinforcementItemPreferredBarsCnt.FaceBarsCnt;
            double mPreferredBarsCnt = targetReinforcementItemPreferredBarsCnt.AngleBarsItem.Mn * 4 + targetReinforcementItemPreferredBarsCnt.FaceBarsItem.Mn * targetReinforcementItemPreferredBarsCnt.FaceBarsCnt * 2;

            label_V2_AngleBarsItemDiamText.Content = targetReinforcementItemPreferredBarsCnt.AngleBarsItem.Dn.ToString();
            if (targetReinforcementItemPreferredBarsCnt.FaceBarsCnt != 0)
            {
                label_V2_FaceBarsCntText.Content = targetReinforcementItemPreferredBarsCnt.FaceBarsCnt.ToString();
                label_V2_FaceBarsDiamText.Content = targetReinforcementItemPreferredBarsCnt.FaceBarsItem.Dn.ToString();
            }
            else
            {
                label_V2_FaceBarsCntText.Content = "-";
                label_V2_FaceBarsDiamText.Content = "-";
            }

            label_V2_FrText.Content = Math.Round(fPreferredBarsCnt, 3);
            label_V2_MrText.Content = Math.Round(mPreferredBarsCnt, 2);
        }
        //Функция приспособленности
        public static double Fitness(ReinforcementItem item, double ft, double columnSection, double coverLayer)
        {
            double m = item.AngleBarsItem.Mn * 4 + item.FaceBarsItem.Mn * item.FaceBarsCnt * 2;
            double f = item.AngleBarsItem.Fn * 2 + item.FaceBarsItem.Fn * item.FaceBarsCnt;

            double step = ((columnSection - coverLayer * 2) - item.AngleBarsItem.Dn) / 1 + item.FaceBarsCnt;
            if (step < 400) return 0;

            double rDist = (columnSection - coverLayer * 2) - item.AngleBarsItem.Dn * 2 - item.FaceBarsItem.Dn * item.FaceBarsCnt;
            if (rDist < 50) return 0;

            if (f > ft)
            {
                if (item.AngleBarsItem.Dn >= item.FaceBarsItem.Dn)
                {
                    return 1.0 / (m + 1);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        //Функция приспособленности при заданном колличестве стержней
        public static double FitnessPreferredBarsCnt(ReinforcementItem item, double ft, double columnSection, double coverLayer, int preferredBarsCnt)
        {
            double m = item.AngleBarsItem.Mn * 4 + item.FaceBarsItem.Mn * item.FaceBarsCnt * 2;
            double f = item.AngleBarsItem.Fn * 2 + item.FaceBarsItem.Fn * item.FaceBarsCnt;

            double step = ((columnSection - coverLayer * 2) - item.AngleBarsItem.Dn) / 1 + item.FaceBarsCnt;
            if (step < 400) return 0;

            double rDist = (columnSection - coverLayer * 2) - item.AngleBarsItem.Dn * 2 - item.FaceBarsItem.Dn * item.FaceBarsCnt;
            if (rDist < 50) return 0;

            if (item.FaceBarsCnt + 2 != preferredBarsCnt) return 0;

            if (f > ft)
            {
                if (item.AngleBarsItem.Dn >= item.FaceBarsItem.Dn)
                {
                    return 1.0 / (m + 1);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        // Сортировка популяции по приспособленности
        public static void SortPopulation(List<ReinforcementItem> population, double ft, double columnSection, double coverLayer)
        {
            population.Sort((x, y) => Fitness(y, ft, columnSection, coverLayer).CompareTo(Fitness(x, ft, columnSection, coverLayer)));
        }
        // Сортировка популяции при заданном колличестве стержней
        public static void SortPopulationPreferredBarsCnt(List<ReinforcementItem> population, double ft, double columnSection, double coverLayer, int preferredBarsCnt)
        {
            population.Sort((x, y) => FitnessPreferredBarsCnt(y, ft, columnSection, coverLayer, preferredBarsCnt).CompareTo(FitnessPreferredBarsCnt(x, ft, columnSection, coverLayer, preferredBarsCnt)));
        }
    }
}
