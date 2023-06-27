using System;
using System.Windows.Forms;

namespace Lana_Renee_Lashes
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // maximum cost per unit before warning
            const double COST_WARNING = 4.5;
            double goodyLashPrice = 2.9;
            double estGoodyShipPricePerUnit = 0.0309;

            double oliviaLashPrice = 2;
            double boxPriceFlat = 0;
            double boxPricePacked = 0;
            // estimate shipping cost per unit
            decimal estOliviaShipPricePerUnit = 0.2263m;
            // estimate box cost per unit
            decimal estBoxPrice = 0.2632m;

            double retailPrice = 24.99;

            double gstMultiplier = 0.1;

            double estAudMultiplier = 1.06;
            decimal pA_HourlyRate = 25.00m;
            double pA_EstBoxesPerHour = 53.33;

        }
    }
}
