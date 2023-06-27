using System.Windows.Forms;

namespace Lana_Renee_Lashes
{
    public partial class Main : Form
    {
        #region Declarations

        // personal assistant estimated amount of boxes packed per hour
        const double PA_BOXES_PER_HOUR = 53.33;
        // maximum cost per unit before warning
        const double COST_WARNING = 4.5;
        // max hourly rate
        const int MAX_RATE = 40;
        // min hourly rate
        const int MIN_RATE = 20;

        // total cost of this goody order
        decimal goodyCost = 0m;
        // quantity of lashes from goody
        int goodyQuantity = 0;
        // price per goody lash
        decimal goodyLashPrice = 2.9m;
        // estimated goody shipping price per unit
        decimal goodyPricePerUnit = 0.0309m;

        // total cost of this olivia order
        decimal oliviaCost = 0m;
        // quantity of lashes from olivia
        int oliviaQuantity = 0;
        // price per olivia lash 
        decimal oliviaLashPrice = 2;
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
        // hours spent boxing
        double pA_HoursSpentboxing = 0;
        // estimated hours it will take to box this order (based on the const int BOX_PER_HOUR taken from previous data)
        double pA_EstimatedHoursToBox = 0;
        // estimated total personal assistant cost
        decimal pA_EstimatedCost = 0m;

        // estimated cost per unit
        decimal estCostPerUnit = 0m;
        // estimated profit per unit
        decimal estProfitPerUnit = 0m;
        // estimated sales required before profit starts
        decimal estSalesToProfit = 0m;
        // total lashes ordered
        int totalLashesOrdered = 0;
        // estimated total cost
        decimal estTotalCost = 0m;
        // estimated profit
        decimal estProfit = 0m;
        // estimated profit less gst
        decimal estProfitLessGst = 0m;
        // gst to pay
        decimal gstToPay = 0m;


        #endregion

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

                        //calculates our values and displays to checkbox
                        Calculupdate();

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
                        Calculupdate();

                    } // end if

                } // end if

            }
            catch
            {
                MessageBox.Show("Please repeat what you did and video it so I can fix it :P");

            } // end try

        }
        #endregion

        #region Calculupdate()
        /// <summary>
        /// Calculates our new values and updates the textboxes with results
        /// </summary>
        private void Calculupdate()
        {
            try
            {
                // stores cost of goody order
                goodyCost = decimal.Parse(textBoxGoodyCost.Text);
                // stores number of goody lashes
                goodyQuantity = int.Parse(textBoxGoodyQuantity.Text);

                // stores cost of olivia order
                oliviaCost = decimal.Parse(textBoxOliviaCost.Text);
                // stores quantity of olivia lashes
                oliviaQuantity = int.Parse(textBoxOliviaQuantity.Text);

                // if personal assistant checkbox is checked AND P.A's hourly rate is not null
                if (checkBoxPaCosts.Checked && textBoxPaHourlyRate.Text != null)
                {
                    // sets pa hourly rate to textbox contents
                    pA_HourlyRate = decimal.Parse(textBoxPaHourlyRate.Text);
                    textBoxPaHourlyRate.Text = pA_HourlyRate.ToString("c2");

                    // if P.A. hourly greater than max rate OR less than min rate
                    if (pA_HourlyRate > MAX_RATE || pA_HourlyRate < MIN_RATE)
                    {
                        // writes error message to user
                        MessageBox.Show("Please ensure hourly rate is valid!");

                    }
                    else if (pA_HoursSpentboxing == 0)
                    {
                        // writes error message to user
                        MessageBox.Show("Please ensure you have entered a valid number of hours for the P.A.");

                    }
                    else
                    {
                        // sets hourly rate to hourly rate * hours spent boxing
                        pA_EstimatedCost = pA_HourlyRate *= (decimal)pA_HoursSpentboxing;
                        // updates textbox with new hourly rate in decimal form
                        textBoxPa.Text = (pA_EstimatedCops.ToString("c2"));

                    } // end if



                } // end if

            }
            catch
            {
                MessageBox.Show("Take a video of whats happening with what values and in which textboxes plus what you are trying to achieve so I can fix it");

            } // end try

        }
        #endregion

        #region checkBoxPaCosts_CheckChanged Event
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
                labelPaHourlyRate.Show();
                textBoxPaHourlyRate.Show();
                labelPaHoursSpent.Show();
                textBoxPaHoursSpent.Show();
                labelPaHoursToBox.Show();
                textBoxPaBoxesPerHour.Show();
            }
            // else if P.A. checkbox is not checked
            else
            {
                // hide P.A. controls
                labelPaHourlyRate.Hide();
                textBoxPaHourlyRate.Hide();
                labelPaHoursSpent.Hide();
                textBoxPaHoursSpent.Hide();
                labelPaHoursToBox.Hide();
                textBoxPaBoxesPerHour.Hide();
                pA_HourlyRate = 0;

            } // end if

        }
        #endregion

        #region checkBoxPaCosts_CheckChanged Event
        /// <summary>
        /// Shows/Hides extra boxes controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion

        #region linkLabelUsdToAud_LinkClicked Event
        /// <summary>
        /// Click me! link to check usd to aud conversion rate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelUsdToAud_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // sets website address to currency comparison on google
            string url = "https://www.google.com/search?q=usd+to+aud&ei=7yubZKOKEfOK2roPo4qSoAI&ved=0ahUKEwjjvY_Zi-T_AhVzhVYBHSOFBCQQ4dUDCBA&uact=5&oq=usd+to+aud&gs_lcp=Cgxnd3Mtd2l6LXNlcnAQAzINCAAQigUQsQMQgwEQQzIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDIFCAAQgAQyBQgAEIAEMgUIABCABDoKCAAQRxDWBBCwAzoKCAAQigUQsAMQQzoHCAAQigUQQ0oECEEYAFD7BFiTDmD1DmgBcAF4AIABoAKIAdQKkgEFMC4yLjSYAQCgAQHAAQHIAQo&sclient=gws-wiz-serp";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }
        #endregion 

        #region linkLabelLanaReneeLashes_LinkClicked Event
        /// <summary>
        /// Link to lane renee lashes website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelLanaReneeLashes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // sets website address to lana renee lashes home
            string url = "http://lanareneelashes.com";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }
        #endregion
        private void labelEta_Click(object sender, System.EventArgs e)
        {
            // sets website address
            string url = "https://www.google.com/search?q=calculator&source=hp&ei=k2abZJOsDeebjuMP6beCmAo&iflsig=AOEireoAAAAAZJt0o4y_-7uv_nA7Dkpo22FPs-_8ZV1r&ved=0ahUKEwjTwa_Pw-T_AhXnjWMGHembAKMQ4dUDCAs&uact=5&oq=calculator&gs_lcp=Cgdnd3Mtd2l6EAMyCwgAEIAEELEDEIMBMgsIABCABBCxAxCDATILCAAQgAQQsQMQgwEyCwgAEIAEELEDEIMBMggIABCABBCxAzIICAAQgAQQsQMyCAgAEIAEELEDMgUIABCABDILCAAQgAQQsQMQgwEyBQgAEIAEOhEILhCABBCxAxCDARDHARDRAzoLCAAQigUQsQMQgwE6EQguEIoFELEDEIMBEMcBENEDOhEILhCKBRCxAxCDARDHARCvAToECAAQAzoLCC4QgAQQxwEQ0QM6CwguEIoFELEDEIMBOgsILhCABBCxAxCDAVAAWPAGYLYHaABwAHgAgAHQAYgBwwqSAQUwLjcuMZgBAKABAQ&sclient=gws-wiz";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
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