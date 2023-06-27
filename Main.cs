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
            // price per goody lash
            double goodyLashPrice = 2.9;
            // estimated goody shipping price per unit
            double estGoodyShipPricePerUnit = 0.0309;

            // price per olivia lash 
            double oliviaLashPrice = 2;
            // price per flatpacked box (unknown)
            decimal boxPriceFlat = 0;
            // price per packed box (unknown)
            decimal boxPricePacked = 0;
            // estimated shipping cost per unit
            decimal estOliviaShipPricePerUnit = 0.2263m;
            // estimated box cost per unit

            decimal estBoxPrice = 0.2632m;
            // retail price per unit
            decimal retailPrice = 24.99m;
            // australian gst multiplier (10%)
            double gstMultiplier = 0.1;
            // est usd to aud multiplier - this should only be a guide
            double estUsdToAusMultiplier = 1.5;
            // personal assistant hourly rate

            decimal pA_HourlyRate = 25.00m;
            // personal assistant estimated amount of boxes packed per hour
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
