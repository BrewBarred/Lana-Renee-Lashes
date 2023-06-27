using System.Windows.Forms;

namespace Lana_Renee_Lashes
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        #region CreateAccount_KeyUp Event
        /// <summary>
        /// When any key is lifted, checks if all controls are valid to enable/disable "Create Account" button and displays tool tips to assist user with validation process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)

        {
            try
            {
                // if user was releasing any key other than the "enter key" and currently active control is a textbox
                if (!(e.KeyCode is Keys.Enter) && ActiveControl is TextBox textBox)
                {
                    // if user has selected autofill on
                    if (checkBoxAutoFill.Checked)
                    {
                        // if textbox is empty
                        if (textBox.Text is null or "")
                        {
                            // handles event
                            e.Handled = true;
                            return;

                        } // end if

                        Calculate();

                    } // end if

                } // end if

                // else if user was releasing the "enter" key and currently active  control is not a textbox
                else
                {
                    // if the currently active control is a checkbox
                    if (ActiveControl is CheckBox thisCheckBox)
                    {
                        // checks/unchecks current textbox depending on current state
                        thisCheckBox.Checked = thisCheckBox.Checked = false ? true : false;
                    }
                    // else if the current active control is not a checkbox
                    else
                    {
                        // send a click event to Create Account
                        MessageBox.Show("Calculating...");
                        Calculate();

                    } // end if

                } // end if

            }
            catch
            {
                MessageBox.Show("Please repeat what you did and video it so I can fix it :P");

            } // end try
            #endregion
        }
        private void Calculate()
        {
            KeyPreview = true;
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

        /// <summary>
        /// Shows/Hides personal assistant extras and adds/deducts cost from profits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxPaCosts_CheckedChanged(object sender, System.EventArgs e)
        {
            // if P.A. checkbox is checked
            if (checkBoxPaCosts.Checked)
            {
                // show P.A. controls
                labelPaRate.Show();
                textBoxPaRate.Show();
                labelPaHoursSpent.Show();
                textBoxPaHoursSpent.Show();
                labelPaBoxesPerHour.Show();
                textBoxPaBoxesPerHour.Show();
                textBoxPaRate.Text = "$0.00";
            }
            // else if P.A. checkbox is not checked
            else
            {
                // hide P.A. controls
                labelPaRate.Hide();
                textBoxPaRate.Hide();
                labelPaHoursSpent.Hide();
                textBoxPaHoursSpent.Hide();
                labelPaBoxesPerHour.Hide();
                textBoxPaBoxesPerHour.Hide();
                textBoxPaRate.Text = "$0.00";

            } // end if

        }

        private void checkBoxExtraBoxes_CheckedChanged(object sender, System.EventArgs e)
        {
            // if extra boxes checkbox is checked
            if (checkBoxExtraBoxes.Checked)
            {
                // show extra boxes controls
                labelExtraBoxesAmount.Show();
                textBoxExtraBoxes.Show();
            }
            // else if extra boxes checkbox is not checked
            else
            {
                // show extra boxes controls
                labelExtraBoxesAmount.Hide();
                textBoxExtraBoxes.Hide();

            } // end if
        }

        private void Main_Load(object sender, System.EventArgs e)
        {

        }
    }
    public static class Tools
    {
        #region Tool Tips
        /// <summary>
        /// Displays a tool tip at the current control
        /// </summary>
        /// <param name="control">Current Control</param>
        /// <param name="title">Desired tool tip title</param>
        /// <param name="text">Desired tool tip message</param>
        /// <returns></returns>
        public static Control ShowUsYaTips(this Control control, string title, string text)
        {
            // creates a new instance of the ToolTip method
            var toolTip = new ToolTip
            {
                // sets the tool tip icon
                ToolTipIcon = ToolTipIcon.Warning,
                // sets the tool tip shape to a rectangle instead of a balloon
                IsBalloon = false,
                // enables tool tip even if parent control is not active
                ShowAlways = true,
                // sets the period of time that the tool tip is shown for
                AutoPopDelay = 1600,
                // sets the period of time that the mouse pointer must remain stationary for before the tool tip is shown
                InitialDelay = 0,
                // enables tool tip animation effect
                UseAnimation = true,
                // enables tool tip fading effect
                UseFading = true
            };
            toolTip.SetToolTip(control, text);
            return control;
        }
        /// <summary>
        /// Displays a tool tip at the current control
        /// </summary>
        /// <param name="control">Current Control</param>
        /// <param name="text">Desired tool tip message</param>
        /// <returns></returns>
        public static void ShowUsYaTips(this Control control, string text)
        {
            if (text is null or "")
            {

            }
            var toolTip = new ToolTip
            {
                // sets the tool tip shape to a rectangle instead of a balloon
                IsBalloon = false,
                // enables tool tip even if parent control is not active
                ShowAlways = true,
                // sets the period of time that the tool tip is shown for
                AutoPopDelay = 1600,
                // sets the period of time that the mouse pointer must remain stationary for before the tool tip is shown
                InitialDelay = 0,
                // enables tool tip animation effect
                UseAnimation = true,
                // enables tool tip fading effect
                UseFading = true
            };
            toolTip.SetToolTip(control, text);
        }
        #endregion
    }
}