using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using static SkiaSharp.HarfBuzz.SKShaper;
using System.Globalization;

namespace _3Lab
{
    public partial class Form1 : Form
    {
        private static readonly SKColor s_blue = new(25, 118, 210);
        private static readonly SKColor s_red = new(229, 57, 53);
        private static readonly SKColor s_yellow = new(198, 167, 0);

        public Form1()
        {
            InitializeComponent();
        }

        private void calculateFirstPart_Click(object sender, EventArgs e)
        {
            var result = CalculatePartOne((double)nachalnaC.Value, (double)step1.Value,
                (double)obimApp.Value, (double)obemRashod.Value, (double)coeffM.Value,
                (int)yacheikiNum.Value, (double)inputA.Value);

            dataGridView2.Rows.Clear();
            foreach (var item in result)
            {
                dataGridView2.Rows.Add(item.Time, item.Concentration);
            }

            cartesianChart1.Series = new List<LineSeries<double>>{new LineSeries<double>
            {
                Values = result.Select(r => r.Concentration),
                Fill = null,
                Name = "CA",
                GeometryStroke = new SolidColorPaint(s_red, 2),
                Stroke = new SolidColorPaint(s_red, 2),
            } };

        }

        private void calculateSecondPart_Click(object sender, EventArgs e)
        {
            var result = NumericMethod((double)startConcentrA.Value, (double)startConcentrB.Value,
                (double)startConcentrC.Value, (double)step.Value, (double)timee.Value,
                (double)machineVolume.Value, (double)machineAmount.Value, (double)rashod.Value,
                (double)speedConst.Value, (double)inputA2.Value);

            dataGridView1.Rows.Clear();
            foreach (var item in result)
            {
                dataGridView1.Rows.Add(item.T, item.CA, item.CB, item.CC);
            }

            cartesianChart2.Series = new List<LineSeries<double>>{new LineSeries<double>
            {
                Values = result.Select(r => r.CA),
                Fill = null,
                Name = "CA",
                GeometryStroke = new SolidColorPaint(s_red, 2),
                Stroke = new SolidColorPaint(s_red, 2),
            },
            new LineSeries<double>
            {
                Values = result.Select(r => r.CB),
                Fill = null,
                Name = "CB",
                GeometryStroke = new SolidColorPaint(s_blue, 2),
                Stroke = new SolidColorPaint(s_blue, 2),
            },
            new LineSeries<double>
            {
                Values = result.Select(r => r.CC),
                Fill = null,
                Name = "CC",
                GeometryStroke = new SolidColorPaint(s_yellow, 2),
                Stroke = new SolidColorPaint(s_yellow, 2),
            }
            };
        }

        private static int Factorial(int n)
        {
            int num = 1;
            for (int index = 1; index <= n; index++)
                num *= index;
            return num;
        }

        private static ICollection<PointTimeToConcenration> CalculatePartOne(
            double cin,
            double step,
            double v,
            double g,
            double m,
            int n,
            double startA)
        {
            if (n <= 0)
                throw new ArgumentException("N должно быть больше 0");
            List<PointTimeToConcenration> partOne = new List<PointTimeToConcenration>();

            double num1 = 0;

            double a = 0.0;
            double s = 0.0;
            var t = 0.0;
            while (s <= m * v / g)
            {
                t = s;
                for (int index2 = 1; index2 <= n; ++index2)
                    num1 += 1.0 / Factorial(index2 - 1) * Math.Pow((double)(t / v / g), (double)(index2 - 1));
                a = cin - num1 * Math.Exp((double)(-t / v / g)) * cin;

                partOne.Add(new PointTimeToConcenration()
                {
                    Concentration = a,
                    Time = s
                });
                num1 = 0;
                s += step;
            }
            double coutput = a;
            double num2 = 1.0;
            while (Math.Round(a, 2) != 0.0)
            {
                t = num2;
                for (int index3 = 1; index3 <= n; ++index3)
                    num1 += 1.0 / Factorial(index3 - 1) * Math.Pow((double)(t / v / g), (double)(index3 - 1));
                a = num1 * Math.Exp((double)(-t / v / g)) * coutput;

                partOne.Add(new PointTimeToConcenration()
                {
                    Concentration = a,
                    Time = num2 + m * v / g
                });

                num1 = 0.0;
                num2 += step;
            }

            return partOne;
        }

        public static ICollection<OutputPartTwoModel> NumericMethod(
            double CA1,
            double CB1,
            double CC1,
            double Step,
            double t1,
            double V,
            double N,
            double G,
            double CAInput,
            double K)
        {
            K /= 60;
            double num1 = V / (N * G);
            double num2 = CB1;
            double num3 = CC1;
            List<OutputPartTwoModel> outputPartTwoModelList = new List<OutputPartTwoModel>
            {
                new OutputPartTwoModel()
                {
                    T = 0.0,
                    CA = CA1,
                    CB = CB1,
                    CC = CC1
                }
            };

            for (double num4 = Step; num4 <= t1; num4 += Step)
            {
                CA1 += Step * (1.0 / num1 * (CAInput - CA1) - K * CA1);
                CB1 += Step * (1.0 / num1 * (num2 - CB1) + 2.0 * K * CA1);
                CC1 += Step * (1.0 / num1 * (num3 - CC1) + K * CA1);

                outputPartTwoModelList.Add(new OutputPartTwoModel()
                {
                    T = Math.Round(num4, 2),
                    CA = Math.Round(CA1, 3),
                    CB = Math.Round(CB1, 3),
                    CC = Math.Round(CC1, 3)
                });
            }
            return outputPartTwoModelList;
        }
    }
}