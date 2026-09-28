using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YehorovaLR9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Програма для обчислення площі паралелограма \n(3 методи).\n\n" +
                "Розробила: студентка групи 7.F1.25\n" +
                "Єгорова В.С.",
                "Про програму",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- ОБРОБНИКИ ПОДІЙ ОБЧИСЛЕННЯ (CLICK) ---

        // Метод 1: Через основу та висоту
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double sideA = Convert.ToDouble(textBox1.Text);
                double height = Convert.ToDouble(textBox2.Text);

                if (sideA <= 0 || height <= 0)
                {
                    label4.ForeColor = Color.Red;
                    label4.Text = "Помилка: Значення мають бути > 0!";
                    return;
                }

                double area = sideA * height;
                label4.ForeColor = Color.Black;
                label4.Text = String.Format("Площа паралелограма S = {0:F2}", area);
            }
            catch (Exception)
            {
                label4.ForeColor = Color.Red;
                label4.Text = "Помилка: Введено некоректне число!";
            }
        }

        // Метод 2: Через дві сторони та кут між ними
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double sideA = Convert.ToDouble(textBox3.Text);
                double sideB = Convert.ToDouble(textBox4.Text);
                double angleDeg = Convert.ToDouble(textBox5.Text);

                if (sideA <= 0 || sideB <= 0 || angleDeg <= 0 || angleDeg >= 180)
                {
                    label5.ForeColor = Color.Red;
                    label5.Text = "Помилка: Сторони > 0, кут від 0 до 180!";
                    return;
                }

                double angleRad = angleDeg * Math.PI / 180.0;
                double area = sideA * sideB * Math.Sin(angleRad);
                label5.ForeColor = Color.Black;
                label5.Text = String.Format("Площа паралелограма S = {0:F2}", area);
            }
            catch (Exception)
            {
                label5.ForeColor = Color.Red;
                label5.Text = "Помилка: Введено некоректне число!";
            }
        }

        // Метод 3: Через діагоналі та кут
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                double d1 = Convert.ToDouble(textBox6.Text);
                double d2 = Convert.ToDouble(textBox7.Text);
                double angleDeg = Convert.ToDouble(textBox8.Text);

                if (d1 <= 0 || d2 <= 0 || angleDeg <= 0 || angleDeg >= 180)
                {
                    label9.ForeColor = Color.Red;
                    label9.Text = "Помилка: Діагоналі > 0, кут від 0 до 180!";
                    return;
                }

                double angleRad = angleDeg * Math.PI / 180.0;
                double area = 0.5 * d1 * d2 * Math.Sin(angleRad);
                label9.ForeColor = Color.Black;
                label9.Text = String.Format("Площа паралелограма S = {0:F2}", area);
            }
            catch (Exception)
            {
                label9.ForeColor = Color.Red;
                label9.Text = "Помилка: Введено некоректне число!";
            }
        }
    }
}
