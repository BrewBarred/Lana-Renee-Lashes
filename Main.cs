using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static LanaReneeLashes.Tools.Logger;


namespace LanaReneeLashes
{
    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : Form
    {
        #region Declarations

        #region Static Defaults

        ///////
        // Settable defaults
        ////

        // default valid font color
        static Color defaultValidTextColor = SystemColors.WindowText;

        // default valid textbox color
        static Color defaultValidTextboxColor = SystemColors.InactiveCaption;
        // default invalid textbox color
        static Color defaultInvalidTextboxColor = Color.RosyBrown;

        // set price per goody lash
        static decimal setGoodyLashCost = 2.9m;
        // set estimated goody price per unit calculated from a past order
        static decimal setGoodyTotalCostPerUnit = 2.9309m;

        // set price per olivia lash 
        static decimal setOliviaLashCost = 2;
        // set estimated olivia price per unit calculated from a past order
        static decimal setOliviaTotalCostPerUnit = 3.3523m;

        // set retail price per unit
        static decimal setRetailSalePrice = 24.99m;

        // set boxing cost (per box in AUD)
        static decimal setBoxingCost = 0.25m;
        // set estimated boxes packed per hour
        static double setEstBoxesPerHour = 90d;

        // set estimated cost of flatboxes
        static decimal setCostOfFlatBoxes = 0.2632m;
        // set estimated cost of shipping
        static decimal setCostOfShipping = 0.2263m;

        ///////
        // Defaults
        ////

        // default goody costs


        // default goody total
        static decimal defaultGoodyTotal = 0.00m;
        // default goody quantity
        static int defaultGoodyQuantity = 0;


        // default olivia costs


        // default olivia total
        static decimal defaultOliviaTotal = 0.00m;
        // default olivia quantity
        static int defaultOliviaQuantity = 0;


        // default boxing costs


        // default boxing cost string
        static string defaultBoxingCostString = setBoxingCost.ToString() + " cents";
        // default estimated hours to box
        static double defaultEstimatedHoursToBox = 0;
        // default hours spent boxing
        static double defaultHoursSpentBoxing = 0;


        // default PA costs


        // default P.A. hourly rate
        static decimal defaultPaHourlyRate = 25.00m;
        // default P.A. hours spent on this order
        static double defaultPaHoursSpentThisOrder = 0;
        // default estimated P.A. cost
        static decimal defaultEstPaCost = 0.00m;


        // default currency conversion rate


        // default USD to AUS currency conversion rate
        static double defaultUsdToAudRate = 1.57d;


        // default estimates

        // default estimated boxing cost
        static decimal defaultCostOfBoxing = 0.00m;
        // default estimated cost per unit
        static decimal defaultEstCostPerUnit = 0.00m;
        // default estimated profit per unit
        static decimal defaultEstProfitPerUnit = 0.00m;
        // default estimated cost of flatboxes
        static decimal defaultEstCostOfFlatBoxes = 0.00m;
        // default estimated cost of shipping
        static decimal defaultEstCostOfShipping = 0.00m;

        // default total lashes ordered
        static int defaultTotalQuantity = 0;
        // default estimated sales to profit
        static int defaultEstSalesToProfit = 0;
        // default estimated total cost
        static decimal defaultEstTotalCost = 0.00m;
        // default estimated profit
        static decimal defaultEstProfit = 0.00m;
        // default estimated profit less GST
        static decimal defaultEstProfitLessGst = 0.00m;
        // default estimated GST total
        static decimal defaultEstGstTotal = 0.00m;

        #endregion

        #region Constants

        ///////
        // Constants
        ////

        // australian gst multiplier (10%)
        static double GST_MULTIPLIER = 0.1;
        // maximum total 
        const double MAX_VALUE = 2147483647;
        // maximum cost per unit before warning
        const decimal COST_WARNING = 5.5m;
        // max hourly rate
        const int MAX_HOURLY_RATE = 40;
        // min hourly rate
        const int MIN_HOURLY_RATE = 20;

        #endregion

        #region Variables

        // groups all USD currency values into an array for easier conversion to AUD later
        static decimal[] currencyValuesArray = { defaultCostOfBoxing, defaultEstCostPerUnit, defaultEstProfitPerUnit, defaultEstCostOfFlatBoxes, defaultEstCostOfShipping,
                                                 defaultEstTotalCost, defaultEstProfit, defaultEstProfitLessGst, defaultEstGstTotal };

        ///////
        // Goody price break-down
        ////

        // total cost of this goody order
        decimal goodyTotal;
        // quantity of lashes from goody
        int goodyQuantity;
        // price per goody lash
        static decimal goodyEstLashCost;
        // estimated goody price per unit calculated from a past order
        static decimal goodyEstTotalCostPerUnit;


        ///////
        // Olivia price break-down (Boxing company too)
        ////

        // total cost of this olivia order
        decimal oliviaTotal;
        // quantity of lashes from olivia
        int oliviaQuantity;
        // price per olivia lash 
        decimal oliviaEstLashCost;
        // estimated olivia price per unit calculated from a past order
        decimal oliviaEstTotalCostPerUnit;


        ///////
        // Boxing break-down
        ////

        // total cost to pack 1 box
        decimal boxingCost;
        // estimated hours it will take to box this order (based on the const int BOX_PER_HOUR taken from previous data)
        double estHoursToBox;
        // hours spent boxing
        double hoursSpentBoxing;


        ///////
        // Personal assistant break-down
        ////

        // personal assistant hourly rate
        decimal paHourlyRate;
        // estimated total personal assistant cost
        double paHoursSpentThisOrder;


        ///////
        // Currency conversion break-down
        ////

        double usdToAud;


        ///////
        // Estimated figures break-down (Display side)
        ////

        // total estimated boxing cost
        static decimal estCostOfBoxing
        {
            // returns estBoxingCost
            get { return currencyValuesArray[0]; }
            // updates estBoxingCost
            set { currencyValuesArray[0] = value; }

        } // end decimal

        // estimated cost per unit
        static decimal estCostPerUnit
        {
            // returns estCostPerUnit
            get { return currencyValuesArray[1]; }
            // updates estCostPerUnit
            set { currencyValuesArray[1] = value; }

        } // end decimal

        // estimated profit per unit
        static decimal estProfitPerUnit
        {
            // returns estProfitPerUnit
            get { return currencyValuesArray[2]; }
            // updates estProfitPerUnit
            set { currencyValuesArray[2] = value; }

        } // end decimal

        // estimated pa cost
        static decimal estPaCost;
        // rough price per box
        static decimal estCostOfFlatBoxes
        {
            // returns estCostOfFlatBoxes
            get { return currencyValuesArray[3]; }
            // updates estCostOfFlatBoxes
            set { currencyValuesArray[3] = value; }

        } // end decimal

        // rough shipping cost per unit
        static decimal estCostOfShipping
        {
            // returns estCostOfShipping
            get { return currencyValuesArray[4]; }
            // updates estCostOfShipping
            set { currencyValuesArray[4] = value; }

        } // end decimal

        // total lashes ordered
        int totalQuantity;
        // estimated sales required before profit starts
        int estSalesToProfit;
        // estimated total cost
        static decimal estTotalCost
        {
            // returns estTotalCost
            get { return currencyValuesArray[5]; }
            // updates estTotalCost
            set { currencyValuesArray[5] = value; }

        } // end decimal

        // estimated profit
        static decimal estProfit
        {
            // returns estProfit
            get { return currencyValuesArray[6]; }
            // updates estProfit
            set { currencyValuesArray[6] = value; }

        } // end decimal

        // estimated profit less gst
        static decimal estProfitLessGst
        {
            // returns estProfitLessGst
            get { return currencyValuesArray[7]; }
            // updates estProfitLessGst
            set { currencyValuesArray[7] = value; }

        } // end decimal

        // gst to pay
        static decimal estGstTotal
        {
            // returns estGstTotal
            get { return currencyValuesArray[8]; }
            // updates estGstTotal
            set { currencyValuesArray[8] = value; }

        } // end decimal

        #endregion

        // protects user from accidentally looping confirmation by pressing enter to exit error dialogs
        bool spamProtect = true;

        // regex pattern to match priceboxes
        //static public Regex pricePatternString = new Regex(@"^\$?(\d{0,3}|\,|\.){0,8}", RegexOptions.Compiled);
        // variable used to parse price regex
        //string pricePattern = pricePatternString.ToString();
        // regex pattern to match non-priceboxes
        //static public Regex numberPatternString = new Regex(@"^(\d{0,3}|\,|\.){0,8}", RegexOptions.Compiled);
        // variable used to parse text regex
        //string numberPattern = numberPatternString.ToString();
        // regex pattern used to match numeric digits

        // stores current index of mouse cursor
        int oldIndex = 0;

        #endregion
        // CHECK THIS METHOD -- MIGHT BE REDUNDANT########################################################################################
        #region SetCursor()
        /// <summary>
        /// Sets cursor back into it's correct position
        /// </summary>
        private void SetCursor()
        {
            // treats active control as a textbox
            TextBox textBox = ActiveControl as TextBox;

            // if textbox is displaying a "$" symbol or "0"
            if (textBox.Text is "$" || textBox.Text is "0")
            {
                // sets cursor to the end text
                textBox.SelectionStart = 1;
            }
            // else if textbox is displaying $0
            else if (textBox.Text is "$0" || textBox.TextLength is 2)
            {
                // sets cursor to the end of the text
                textBox.SelectionStart = 2;
            }
            // else if textbox is displaying something other than $0 or nothing
            else
            {
                // reverts index from start of string back to last position
                textBox.SelectionStart = oldIndex;

            } // end if

        } // end void
        #endregion

        #region Main()
        /// <summary>
        /// Initializes Application
        /// </summary>
        public Main()
        {
            InitializeComponent();
            // sets up applications default settings and values
            Setup();

        } // end main
        #endregion

        #region Setup()
        /// <summary>
        /// Sets up application defaults
        /// </summary>
        public void Setup()
        {
            // sets height of app to match working area of the current screen
            Height = Screen.GetWorkingArea(this).Height;
            // defaults the app to the top of the screen
            Top = 0;

            // resets all values to their defaults
            ResetAll();

            // focuses cursor onto textBoxGoodyCost
            textBoxGoodyTotal.Focus();
            // sets cursor to the end of the text in textBoxGoodyCost
            textBoxGoodyTotal.SelectionStart = textBoxGoodyTotal.TextLength;

        } // end void
        #endregion

        #region ResetAll()
        /// <summary>
        /// Resets all variables back to their default values
        /// </summary>
        public void ResetAll()
        {

            ///////
            // Resets goody values
            ////

            goodyTotal = defaultGoodyTotal;
            goodyQuantity = defaultGoodyQuantity;
            goodyEstLashCost = setGoodyLashCost;
            goodyEstTotalCostPerUnit = setGoodyTotalCostPerUnit;


            ///////
            // Resets olivia values
            ////

            oliviaTotal = defaultOliviaTotal;
            oliviaQuantity = defaultOliviaQuantity;
            oliviaEstLashCost = setOliviaLashCost;
            oliviaEstTotalCostPerUnit = setOliviaTotalCostPerUnit;


            ///////
            // Resets boxing values
            ////

            boxingCost = setBoxingCost;
            textBoxBoxingCost.Text = defaultBoxingCostString;
            estHoursToBox = defaultEstimatedHoursToBox;
            hoursSpentBoxing = defaultHoursSpentBoxing;


            ///////
            // Resets PA values
            ////

            paHourlyRate = defaultPaHourlyRate;
            paHoursSpentThisOrder = defaultPaHoursSpentThisOrder;


            ///////
            // Resets estimated figures
            ////

            estCostOfBoxing = defaultCostOfBoxing;
            estCostPerUnit = defaultEstCostPerUnit;
            estProfitPerUnit = defaultEstProfitPerUnit;
            estPaCost = defaultEstPaCost;
            estCostOfFlatBoxes = defaultEstCostOfFlatBoxes;
            estCostOfShipping = defaultEstCostOfShipping;

            totalQuantity = defaultTotalQuantity;
            estSalesToProfit = defaultEstSalesToProfit;
            estTotalCost = defaultEstTotalCost;
            estProfit = defaultEstProfit;
            estProfitLessGst = defaultEstProfitLessGst;
            estGstTotal = defaultEstGstTotal;

            ///////
            // Resets USD to AUD conversion rate
            ////

            usdToAud = defaultUsdToAudRate;

        } // end void
        #endregion

        #region Main_KeyUp Event
        /// <summary>
        /// As each key is lifted on this form, validates the user input and calculates totals if all input boxes are valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            // stores valid keys that can be typed into textboxes
            Keys[] validKeyArray = { Keys.Back, Keys.Oemcomma, Keys.OemPeriod, Keys.Decimal, Keys.Left,
                                 Keys.Right, Keys.Up, Keys.Down, Keys.Tab, Keys.Delete, Keys.Oemcomma };

            try
            {

                // if the active control of this form is a textbox
                if (ActiveControl is TextBoxBase)
                {
                    // unlocks the proper textbox properties
                    TextBox textBox = ActiveControl as TextBox;
                    // stores the user input of this textbox as a string
                    string text = textBox.Text;
                    // stores the last pressed character as a string
                    Keys keyPressed = e.KeyCode;
                    // stores the current cursor position
                    oldIndex = textBox.SelectionStart;

                    // if textbox is empty
                    if (text is null or "" or "$")
                    {
                        // resets prices
                        ResetAll();
                        // updates displays
                        UpdateEstimates();
                        // writes info to console
                        Log("Calculations skipped due to \"" + textBox.Name + "\" being a null value");

                        return;

                    }
                    // else if text is invalid
                    else if (!text.IsCurrency())
                    {
                        // shows a tool tip informing user that they can't use that character
                        textBox.ShowUsYaTips("Invalid input detected!");

                        // if the textbox isn't read only
                        if (!textBox.ReadOnly)
                        {
                            // sets textbox backcolor to red to show it is invalid
                            textBox.BackColor = defaultInvalidTextboxColor;

                        } // end if

                    }
                    // else if the user presses a valid key
                    else
                    {
                        // sets textbox back to its default color incase it was previously invalid
                        textBox.BackColor = defaultValidTextboxColor;

                        // if autofill checkbox is checked and user pressed anything except the enter key
                        if (checkBoxAutoFill.Checked && keyPressed != Keys.Enter)
                        {
                            // calculates and updates values
                            FetchInput();
                        }
                        // else if the user pressed the enter key
                        else if (keyPressed == Keys.Enter)
                        {
                            // if autofill checkbox is checked
                            if (checkBoxAutoFill.Checked)
                            {
                                // creates a control object using the sending object
                                Control control = (Control)sender;
                                // sets next textbox as the active control
                                control.SelectNextControl(ActiveControl, true, true, true, true);

                            }
                            // else if auto fill checkbox is not checked
                            else
                            {
                                // calculates and updates values
                                FetchInput();

                            } // end if

                        } // end if

                    } // end if

                }
                // else if user was releasing the "enter" key and currently active  control is not a textbox
                else
                {
                    // if the currently active control is a checkbox
                    if (ActiveControl is CheckBox thisCheckBox && e.KeyData == Keys.Enter)
                    {
                        // if this checkbox is currently checked
                        if (thisCheckBox.Checked)
                        {
                            // unchecks the checkbox
                            thisCheckBox.Checked = false;
                        }
                        // else if this checkbox is currently unchecked
                        else
                        {
                            // checks the checkbox
                            thisCheckBox.Checked = true;

                        } // end if

                    } // end if

                } // end if
            }
            catch (Exception ex)
            {
                // writes an error to the console
                LogError("Failed to process key up event!", ex.Message);

            } // end try

        }
        #endregion

        #region UpdateEstimates()
        /// <summary>
        /// Updates and formats displaying with estimated values
        /// </summary>
        private void UpdateEstimates()
        {
            // updates all estimate displays
            textBoxEstCostOfBoxing.Text = estCostOfBoxing.FormatCurrency();
            textBoxEstHoursToBox.Text = estHoursToBox.FormatHours();
            textBoxEstCostPerUnit.Text = estCostPerUnit.FormatCurrency();
            textBoxEstProfitPerUnit.Text = estProfitPerUnit.FormatCurrency();
            textBoxTotalLashesOrdered.Text = totalQuantity.ToString();
            textBoxEstSalesToProfit.Text = estSalesToProfit.ToString();
            textBoxEstTotalCost.Text = estTotalCost.FormatCurrency();
            textBoxEstProfit.Text = estProfit.FormatCurrency();
            textBoxEstProfitLessGst.Text = estProfitLessGst.FormatCurrency();
            textBoxEstGstTotal.Text = estGstTotal.FormatCurrency();

            // if estimate prices by quantity checkbox is checked
            if (checkBoxEstPricesByQuantity.Checked)
            {
                // updates goody/olivia totals
                textBoxGoodyTotal.Text = goodyTotal.FormatCurrency();
                textBoxOliviaTotal.Text = oliviaTotal.FormatCurrency();

            } // end if

            // if include P.A. costs checkbox is checked
            if (checkBoxPaCosts.Checked)
            {
                // displays estimated P.A. cost
                textBoxEstPaCost.Text = estPaCost.FormatCurrency();

            }
            // else if include P.A. costs checkbox is not checked
            else
            {
                // sets P.A. cost display to "Disabled"
                textBoxEstPaCost.Text = "Disabled";

            }// end if

            // if enable flatbox estimates checkbox is checked
            if (checkBoxEnableFlatBoxEst.Checked)
            {
                // displays estimated cost of flat boxes
                textBoxEstCostOfFlatBoxes.Text = estCostOfFlatBoxes.FormatCurrency();

            }
            // else if enable flatbox estimates checkbox is not checked
            else
            {
                // sets estimated cost of flatboxes to "Disabled"
                textBoxEstCostOfFlatBoxes.Text = "Disabled";

            }// end if

            // if enable shipping estimates checkbox is checked
            if (checkBoxEnableShippingEst.Checked)
            {
                // displays estimated cost of shipping
                textBoxEstCostOfShipping.Text = estCostOfShipping.FormatCurrency();

            }
            // else if enable shipping estimates checkbox is not checked
            else
            {
                // sets estimated cost of shipping to "Disabled"
                textBoxEstCostOfShipping.Text = "Disabled";

            }// end if

        } // end void

        #endregion

        #region FetchInput()
        /// <summary>
        /// Fetches user input from textboxes and parses them into relevant variables
        /// </summary>
        private void FetchInput()
        {
            try
            {
                // stores all display textboxes
                TextBox[] displayBoxes = { textBoxEstCostOfBoxing, textBoxEstCostPerUnit , textBoxEstProfitPerUnit, textBoxEstPaCost, textBoxEstCostOfFlatBoxes,
                                           textBoxEstCostOfShipping, textBoxTotalLashesOrdered, textBoxEstSalesToProfit, textBoxEstTotalCost, textBoxEstProfit,
                                           textBoxEstProfitLessGst, textBoxEstGstTotal};

                // stores all panels with useable controls in them
                Panel[] panelList = { panelGoody, panelOlivia, panelBoxing, panelPa, panelUsdToAud };

                ///////
                /// Parses all values to their respective variables
                /////

                foreach (Panel thisPanel in panelList)
                {
                    // for each control in this form
                    foreach (Control control in thisPanel.Controls)
                    {
                        // if control is not a textbox
                        if (!(control is TextBox))
                        {
                            // continues to next control
                            continue;

                        } // end if

                        // sets control to a proper textbox for referencing
                        TextBox textBox = control as TextBox;
                        // creates a string reference for the name of the textbox being used
                        string textBoxName = textBox.Name;
                        // creates a string reference for the text in the textbox being used
                        string text = textBox.Text;

                        // if the current textbox is used for display only
                        if (displayBoxes.Contains(textBox))
                        {
                            // continues to next control
                            continue;

                        }
                        // else if the current textbox is not a display textbox and the text is not null
                        else if (text != null)
                        {
                            // switches to textbox name
                            switch (textBoxName)
                            {

                                case "textBoxGoodyTotal":
                                    // stores user input to goodyCost variable
                                    goodyTotal = text.ToDecimal();

                                    break;

                                case "textBoxOliviaTotal":
                                    // stores user input to oliviaCost variable
                                    oliviaTotal = text.ToDecimal();

                                    break;

                                case "textBoxGoodyQuantity":
                                    // stores user input to goodyCost variable
                                    goodyQuantity = text.ToInt();

                                    break;

                                case "textBoxOliviaQuantity":
                                    // stores user input to olivia quantity variable
                                    oliviaQuantity = text.ToInt();

                                    break;

                                case "textBoxBoxingCost":
                                    // stores user input to hoursSpentBoxing
                                    boxingCost = text.ToDecimal();

                                    break;

                                case "textBoxEstHoursToBox":
                                    // stores user input to hoursSpentBoxing
                                    estHoursToBox = text.ToDouble();

                                    break;

                                case "textBoxHoursSpentBoxing":
                                    // stores user input to hoursSpentBoxing
                                    hoursSpentBoxing = text.ToDouble();

                                    break;

                                case "textBoxPaHourlyRate":
                                    // if include P.A. costs checkbox is checked
                                    if (checkBoxPaCosts.Checked)
                                    {
                                        // stores user input to paHourlyRate
                                        paHourlyRate = text.ToDecimal();

                                    } // end if

                                    break;

                                case "textBoxPaHoursSpentOnThisOrder":
                                    // if include P.A. costs checkbox is checked
                                    if (checkBoxPaCosts.Checked)
                                    {
                                        // stores user input to paHoursSpentOnThisOrder
                                        paHoursSpentThisOrder = text.ToDouble();

                                    } // end if

                                    break;

                                case "textBoxUsdToAud":
                                    // stores user input to UsdToAud
                                    usdToAud = text.ToDouble();

                                    break;

                                // if no case was matched
                                default:
                                    // logs error
                                    LogError("[" + DateTime.Now + "]" + "Failed to process control: " + textBox.Name);

                                    break;

                            } // end switch 

                        } // end if 

                    } // end for

                } // end for

                // calculates estimates
                Calculate();

            } // end try
            catch (Exception ex)
            {
                // writes error to console
                LogError("Failed to fetch input!", ex.Message);

            } // end try

        } // end void

        #endregion

        #region Calculate()
        /// <summary>
        /// Calculates the estimated values
        /// </summary>
        public void Calculate()
        {

            // calculates total quantity
            totalQuantity = goodyQuantity + oliviaQuantity;

            // if total lashes ordered is greater than 0
            if (totalQuantity > 0)
            {

                ///////
                /// Calculates personal assistant costs
                /////

                // calculates the PA's hourly rate
                estPaCost = paHourlyRate * (decimal)paHoursSpentThisOrder;

                ///////
                /// Calculates other costs
                /////

                // calculates estimated hours to box
                estHoursToBox = totalQuantity / setEstBoxesPerHour - hoursSpentBoxing;

                // if checkbox estimate prices by quantity is checked
                if (checkBoxEstPricesByQuantity.Checked)
                {
                    // sets goody cost to an estimated value based on a past order
                    goodyTotal = goodyQuantity * goodyEstTotalCostPerUnit;
                    // sets olivia cost to an estimated value based on a past order
                    oliviaTotal = oliviaQuantity * oliviaEstTotalCostPerUnit;

                } // end if

                // if goody cost and quantity are not equal to 0 OR olivia cost and quantity are not equal to 0
                if (goodyTotal + goodyQuantity > 0 || oliviaTotal + oliviaQuantity > 0)
                {
                    // calculates estimated boxing cost
                    estCostOfBoxing = totalQuantity * boxingCost;
                    // adds the cost of both orders together plus Pa expenses for an estimated total cost
                    estTotalCost = goodyTotal + oliviaTotal + estPaCost + estCostOfBoxing;
                    // estimates how many hours it will take to box the unpacked lashes
                    estHoursToBox = totalQuantity / setEstBoxesPerHour - hoursSpentBoxing;
                    // estimates the cost per lash set
                    estCostPerUnit = estTotalCost / totalQuantity;
                    // estimated profit per sale
                    estProfitPerUnit = setRetailSalePrice - estCostPerUnit;
                    // estimated cost of flatboxes
                    estCostOfFlatBoxes = totalQuantity * setCostOfFlatBoxes;
                    // estimated cost of shipping
                    estCostOfShipping = totalQuantity * setCostOfShipping;
                    // sales required to profit
                    estSalesToProfit = (int)estTotalCost / (int)estProfitPerUnit + 1;
                    // total profit for this order
                    estProfit = estProfitPerUnit * totalQuantity;
                    // total profit for this order less GST
                    estProfitLessGst = estProfit - (estProfit * (decimal)GST_MULTIPLIER);
                    // gst amount
                    estGstTotal = estProfit * (decimal)GST_MULTIPLIER;

                } // end if

                // if auto
            }
            // else if total quantity is 0
            else
            {
                // resets everything to their default values
                ResetAll();

            } // end if

            // updates display boxes with newly calculated values
            UpdateEstimates();

        } // end void
        #endregion

        #region buttonUsdToAud_Click
        /// <summary>
        /// Converts displayed estimates from USD to AUD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUsdToAud_Click(object sender, EventArgs e)
        {
            // if estimated total cost is 0
            if (estTotalCost is 0)
            {
                // displays error to user
                MessageBox.Show("Cannot convert to AUD until USD estimates have been calculated!");
                return;

            } // end if

            // checks if figures have already been converted to AUD, returns true if they have, else returns false
            bool converted = ((goodyTotal + oliviaTotal + estCostOfBoxing + estPaCost) * (decimal)usdToAud) == textBoxEstTotalCost.Text.ToDecimal();

            // converts usd to aud multiplier to a decimal for easier calculations
            decimal conversionRate = (decimal)usdToAud;

            if (!converted)
            {
                // foreach decimal value in the decimal value array
                for (int i = 0; i < currencyValuesArray.Length - 1; i++)
                {
                    currencyValuesArray[i] *= conversionRate;

                } // end foreach

                // updates displays with new values
                UpdateEstimates();

            } // end if

        } // end void
        #endregion

        #region Settings:

        #region checkBoxAutoFill_CheckedChanged() Event
        /// <summary>
        /// Handles autofill checkbox event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxAutoFill_CheckedChanged(object sender, EventArgs e)
        {
            // if checkbox autofill is checked
            if (checkBoxAutoFill.Checked is true)
            {
                // calculates and updates estimates
                FetchInput();

            }
            // else if checkbox autofill is not checked
            else
            {
                // sets the forms acceptbutton (enter) to the button to avoid a dinging sound when enter is pressed
                AcceptButton = buttonCalculate;

            } // end if

        } // end void
        #endregion

        #region checkBoxPaCosts_CheckChanged Event
        /// <summary>
        /// Enables/Disables personal assistant costs and adds/deducts cost from total cost etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxPaCosts_CheckedChanged(object sender, System.EventArgs e)
        {
            // if P.A. checkbox is checked
            if (checkBoxPaCosts.Checked)
            {
                // enables P.A's hourly rate textbox
                paHourlyRate = defaultPaHourlyRate;
                textBoxPaHourlyRate.Enable(defaultPaHourlyRate.ToString("C2"));

                // enables P.A's hoursSpentBoxing textbox
                paHoursSpentThisOrder = defaultPaHoursSpentThisOrder;
                textBoxPaHoursSpentOnThisOrder.Enable();

            }
            // else if P.A. checkbox is not checked
            else
            {
                // disables P.A's hourly rate textbox
                paHourlyRate = 0;
                textBoxPaHourlyRate.Disable();

                // disables P.A's hours spent boxing textbox
                paHoursSpentThisOrder = 0;
                textBoxPaHoursSpentOnThisOrder.Disable();

            } // end if

            // updates estimates to allow for PA cost
            FetchInput();

        }
        #endregion

        #region checkBoxEstPricesByQuantity_CheckedChanged
        /// <summary>
        /// Handles estimate prices by quantity checkbox event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxEstPricesByQuantity_CheckedChanged(object sender, EventArgs e)
        {
            // if checkbox is checked
            if (checkBoxEstPricesByQuantity.Checked)
            {
                // disables goody/olivia total textboxes to prevent user getting confused
                textBoxGoodyTotal.Disable();
                textBoxOliviaTotal.Disable();
            }
            // else if checkbox is not checked
            else
            {
                // enabled good/olivia total textboxes again
                textBoxGoodyTotal.Enable();
                textBoxOliviaTotal.Enable();
                // sets goody/olivia quantities to zero
                textBoxGoodyQuantity.Text = "0";
                textBoxOliviaQuantity.Text = "0";

            } // end if

            // calculates values
            FetchInput();

        } // end void
        #endregion

        #region checkBoxEnableFlatBoxEst_CheckChanged Event
        /// <summary>
        /// Enables/Disables flat box estimates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxEnableFlatBoxEst_CheckedChanged(object sender, EventArgs e)
        {
            // if enable flatbox estimates checkbox is checked
            if (!checkBoxEnableFlatBoxEst.Checked)
            {
                // disables flatbox estimates
                textBoxEstCostOfFlatBoxes.Text = "Disabled";

            } // end if

            // updates estimates to allow for flatbox cost
            FetchInput();

        } // end void

        #endregion

        #region checkBoxEnableShippingEst_CheckChanged Event
        /// <summary>
        /// Enables/Disables shipping estimates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxEnableShippingEst_CheckedChanged(object sender, EventArgs e)
        {
            // if enable shipping est checkbox is checked
            if (!checkBoxEnableShippingEst.Checked)
            {
                // disables flatbox estimates
                textBoxEstCostOfShipping.Text = "Disabled";

            } // end if

            // updates estimates to allow for shipping costs
            FetchInput();

        } // end void
        #endregion

        #endregion

        #region Misc Events:

        #region textBoxGoodyTotal_Leave() Event
        /// <summary>
        /// Formats textbox to currency value when user leaves textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxGoodyTotal_Leave(object sender, EventArgs e)
        {
            // formats goody total to represent a currency value
            textBoxGoodyTotal.Text = goodyTotal.FormatCurrency();

        } // end void
        #endregion

        #region textBoxOliviaTotal_Leave() Event
        /// <summary>
        /// Formats textbox to currency value when user leaves textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxOliviaTotal_Leave(object sender, EventArgs e)
        {
            // formats olivia total to represent a currency value
            textBoxOliviaTotal.Text = oliviaTotal.FormatCurrency();

        } // end void
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
            // sets checkedCurrency to true
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

        #region labelSubTitle_MouseEnter()
        /// <summary>
        ///changes subtitle color to blue when mouse enters region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelSubTitle_MouseEnter(object sender, EventArgs e)
        {
            //changes subtitle color to blue
            labelSubTitle.ForeColor = Color.CadetBlue;
        }
        #endregion

        #region labelSubTitle_MouseLeave()
        /// <summary>
        ///changes subtitle color back to white when mouse exits region
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelSubTitle_MouseLeave(object sender, EventArgs e)
        {
            //changes subtitle color back to white
            labelSubTitle.ForeColor = Color.White;

        } // end void
        #endregion

        #region labelSubTitle_Click()
        private void labelSubTitle_Click(object sender, EventArgs e)
        {
            // sets website address to currency comparison on google
            string url = "https://www.google.com/search?q=calc&source=hp&ei=wqObZIrLLqCw4-EPq-qA-Ao&iflsig=AOEireoAAAAAZJux0siSA2TGgXYq7dNvvdXoMeRk0TFQ&ved=0ahUKEwiKtKr8_eT_AhUg2DgGHSs1AK8Q4dUDCAs&uact=5&oq=calc&gs_lcp=Cgdnd3Mtd2l6EAMyDQgAEIoFELEDEIMBEEMyBwguEIoFEEMyBwgAEIoFEEMyDQgAEIoFELEDEIMBEEMyDQgAEIoFELEDEIMBEEMyBwgAEIoFEEMyDQgAEIoFELEDEIMBEEMyEQguEIAEELEDEIMBEMcBENEDMgsIABCABBCxAxCDATILCAAQgAQQsQMQgwE6FwgAEIoFEOoCELQCEIoDELcDENQDEOUCOggIABCABBCxAzoLCAAQigUQsQMQgwFQ4gJYmAVg3AVoAXAAeACAAc0BiAHtApIBBTAuMS4xmAEAoAEBsAEK&sclient=gws-wiz";
            // launches website address
            var urlLauncher = System.Diagnostics.Process.Start(url);
        }

        #endregion

        #endregion

        #region textBoxSpentBoxing_Leave() Event
        /// <summary>
        /// Formats text input when focus is lost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxHoursSpentBoxing_Leave(object sender, EventArgs e)
        {
            try
            {
                // if hours spent boxing is greater than 0
                if (hoursSpentBoxing > 0)
                {
                    // sets hours spent boxing text to time format
                    textBoxHoursSpentBoxing.Text = double.Parse(textBoxHoursSpentBoxing.Text).FormatHours();

                }
                // else if hours spent boxing is not greater than 0
                else
                {
                    // sets hours spent boxing and it's textbox text to 0
                    hoursSpentBoxing = 0;
                    textBoxHoursSpentBoxing.Text = "0";

                }// end if
            }
            catch (Exception ex)
            {
                // logs error to console and error log
                LogError("Failed to convert hours spent boxing!", ex.Message);

            } // end try

        } // end void
        #endregion

    } // end class

} // end namespace