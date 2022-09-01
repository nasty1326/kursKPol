using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kozgova.Kurs.Pol
{
    public partial class Form1 : Form
    {
        double min0OutPol; // максимальное значение выживаемости (близость к ответу)
        double sredOutPol; // среднее значение выживаемости
        int numberGenOutPol; // число поколений
        int numberPopOutPol; // размер популяции
        int numberTurnOutPol; // размер турнира 
        double pcrossOutPol; // вероятность скрещивания
        double pmutationOutPol; // вероятность мутации
        int genNowOutPol; // текущее поколение
        int sizeNewOutPol; // число популяции после скрещивания

        const int lchrom = 2; // число генов
        const int maxpop = 500; // максимальный размер популяции
        const double xmax = 2;
        const double xmin = 1;

        public class individualOutPol //особь без пола
        {
            public double[] chrom { get; set; } = new double[lchrom];
            public double fitness { get; set; }  // коэффициент выживаемости (значение функции) 
            public individualOutPol()
            {
                for (int i = 0; i < lchrom; i++)
                {
                    chrom[i] = -1;
                }
                fitness = -1;
            }
        }

        // Создаем массивы для беcполового скрещивания
        individualOutPol[] oldpopOutPol = new individualOutPol[maxpop]; // массив для старого поколения полового скрещивания
        individualOutPol[] newpopOutPol = new individualOutPol[maxpop]; // массив для нового поколения полового скрещивания
        individualOutPol[] intpopOutPol = new individualOutPol[maxpop]; // массив для сортировки поколения для полового скрещивания

        //основная функция Растригина f(x1, x2) = 20 + x1*x1 + x2*x2 − 10(cos 2πx1 + cos2πx2) 
        double objfunc(double x1, double x2)
        {
           return (20 + x1 * x1 + x2 * x2 - 10 * (Math.Cos(2 * Math.PI * x1) + Math.Cos(2 * Math.PI * x2)));
           
        }


        public Form1()
        {
            InitializeComponent();
        }

        public void initpopOutPol()
        {
            Random rnd = new Random();
            for (int i=0; i< numberPopOutPol; i++)
            {
                
                oldpopOutPol[i].chrom[0] = 0 + rnd.NextDouble()*2;                
                oldpopOutPol[i].chrom[1] = 0 + rnd.NextDouble()*2;
                oldpopOutPol[i].fitness = objfunc(oldpopOutPol[i].chrom[0], oldpopOutPol[i].chrom[1]);
            }
        }

        public void statisticsOutPol(individualOutPol[] pop)
        {
            min0OutPol = pop[0].fitness;
            sredOutPol = pop[0].fitness;
            for (int i=1; i < sizeNewOutPol; i++)
            {
                sredOutPol += pop[i].fitness;
                if (pop[i].fitness < min0OutPol)
                {
                    min0OutPol = pop[i].fitness;
                }
            }
            sredOutPol = sredOutPol / sizeNewOutPol;
            chart1.Series[0].Points.AddXY(genNowOutPol, min0OutPol);
            chart1.Series[1].Points.AddXY(genNowOutPol, sredOutPol);
            
        }


        public double mutation (double fit)
        {
            Random rnd = new Random();
            double p = rnd.NextDouble();
            if (p<pmutationOutPol)
            {
                fit = fit - rnd.NextDouble()/10; //0,1-0,05=0,05   0-0,05=-0,05
            }
            return fit;
        }

        public void crossoverOutPol()
        {
            double p;
            
            Random rnd = new Random();
            for (int i=0; i <= sizeNewOutPol; i++)
            {
             
                p = rnd.NextDouble();
                if (p<pcrossOutPol)
                {
                    newpopOutPol[i].chrom[0] = mutation(oldpopOutPol[i].chrom[0]);
                    newpopOutPol[i].chrom[1] = mutation(oldpopOutPol[i].chrom[1]);
                    newpopOutPol[i].fitness = objfunc(newpopOutPol[i].chrom[0], newpopOutPol[i].chrom[1]);
                }
                else
                {
                    newpopOutPol[i] = oldpopOutPol[i];
                    
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < maxpop; i++)
            {
                oldpopOutPol[i] = new individualOutPol();//старое поколение популяции
                newpopOutPol[i] = new individualOutPol();// новое поколение популяции
                intpopOutPol[i] = new individualOutPol();// полонение для сортировки 
            }

            numberGenOutPol = (int)numOutGen.Value;
            numberPopOutPol = (int)numOutPop.Value;
            numberTurnOutPol = (int)numOutTurn.Value;
            pcrossOutPol = (double)numOutCross.Value;
            pmutationOutPol = (double)numOutMut.Value;
            sizeNewOutPol = numberPopOutPol;

            initpopOutPol(); //создаем первое поколение популяции
           
            genNowOutPol = 1;
            statisticsOutPol(oldpopOutPol);

            do
            {
                genNowOutPol++;

               
                crossoverOutPol();
                
                statisticsOutPol(newpopOutPol); // находим больший коэффициент выживаемости у поколения

                for (int i = 0; i < maxpop; i++) // новое поколение превращаем в старое 
                { oldpopOutPol[i] = newpopOutPol[i]; }
                

            } while (genNowOutPol < numberGenOutPol); // сколько раз было задано 

            
        }
    }
}
