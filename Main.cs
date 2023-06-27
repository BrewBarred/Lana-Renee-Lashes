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

        /// <summary>
        /// Click me! link to check usd to aud conversion rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelUsdToAud_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.google.com/search?q=usd+to+aud&ei=7yubZKOKEfOK2roPo4qSoAI&ved=0ahUKEwjjvY_Zi-T_AhVzhVYBHSOFBCQQ4dUDCBA&uact=5&oq=usd+to+aud&gs_lcp=Cgxnd3Mtd2l6LXNlcnAQAzINCAAQigUQsQMQgwEQQzIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDoKCAAQRxDWBBCwAzoKCAAQigUQsAMQQzoHCAAQigUQQ0oECEEYAFD7BFiTDmD1DmgBcAF4AIABoAKIAdQKkgEFMC4yLjSYAQCgAQHAAQHIAQo&sclient=gws-wiz-serp";
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }

        /// <summary>
        /// Link to lane renee lashes website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelLanaReneeLashes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "http://lanareneelashes.com";
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }
    }
}
