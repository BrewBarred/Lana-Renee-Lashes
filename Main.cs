using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Lana_Renee_Lashes.Tools.Logger;


namespace Lana_Renee_Lashes
{
    public partial class Main : Form
    {
        #region Declarations

        #region Static Default Values

        ///////
        // Adjustable defaults
        ////

        // price per goody lash
        static decimal defaultGoodyLashCost = 2.9m;
        // estimated goody price per unit calculated from a past order
        static decimal defaultGoodyTotalCostPerUnit = 2.9309m;

        // default price per olivia lash 
        static decimal defaultOliviaLashCost = 2;
        // estimated olivia price per unit calculated from a past order
        static decimal defaultOliviaTotalCostPerUnit = 3.3523m;

        // default boxing cost
        static decimal defaultBoxingCost = 0.25m;
        // default estimated boxes packed per hour
        static double defaultEstBoxesPerHour = 90d;
        // default retail price per unit
        static decimal retailSalePrice = 24.99m;

        ///////
        // Defaults
        ////
        ///

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
        static string defaultBoxingCostString = defaultBoxingCost.ToString() + " cents";
        // default estimated hours to box
        static double defaultEstimatedHoursToBox = 0;
        // default hours spent boxing
        static double defaultHoursSpentBoxing = 0;


        // default PA costs


        // default P.A. hourly rate
        static decimal defaultPaHourlyRate = 25.00m;
        // default P.A. hours spent on this order
        static double defaultPaHoursSpentThisOrder = 0;


        // default currency conversion rate


        // default USD to AUS currency conversion rate
        static double defaultUsdToAudRate = 1.57d;


        // default estimates

        // default estimated boxing cost
        static decimal defaultEstBoxingCost = 0.00m;
        // default estimated cost per unit
        static decimal defaultEstCostPerUnit = 0.00m;
        // default estimated profit per unit
        static decimal defaultEstProfitPerUnit = 0.00m;
        // default estimated cost of flatboxes
        static decimal defaultEstFlatBoxCost = 0.2632m;
        // default estimated cost of shipping
        static decimal defaultEstShippingCost = 0.2263m;

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
        static double GST_AMOUNT = 0.1;
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


        ///////
        // Goody price break-down
        ////

        // total cost of this goody order
        decimal goodyTotal = defaultGoodyTotal;
        // quantity of lashes from goody
        int goodyQuantity = defaultGoodyQuantity;
        // price per goody lash
        static decimal goodyLashCost = defaultGoodyLashCost;
        // estimated goody price per unit calculated from a past order
        static decimal goodyTotalCostPerUnit = defaultGoodyTotalCostPerUnit;


        ///////
        // Olivia price break-down (Boxing company too)
        ////

        // total cost of this olivia order
        decimal oliviaTotal = defaultOliviaTotal;
        // quantity of lashes from olivia
        int oliviaQuantity = defaultOliviaQuantity;
        // price per olivia lash 
        decimal oliviaLashCost = defaultOliviaLashCost;
        // estimated olivia price per unit calculated from a past order
        decimal oliviaTotalCostPerUnit = defaultOliviaTotalCostPerUnit;


        ///////
        // Boxing break-down
        ////

        // total cost to pack 1 box
        decimal boxingCost = defaultBoxingCost;
        // estimated hours it will take to box this order (based on the const int BOX_PER_HOUR taken from previous data)
        double estimatedHoursToBox = defaultEstimatedHoursToBox;
        // hours spent boxing
        double hoursSpentBoxing = defaultHoursSpentBoxing;


        ///////
        // Personal assistant break-down
        ////

        // personal assistant hourly rate
        decimal paHourlyRate = defaultPaHourlyRate;
        // estimated total personal assistant cost
        double paHoursSpentThisOrder = defaultPaHoursSpentThisOrder;


        ///////
        // Estimated figures break-down
        ////

        // total lashes ordered
        int totalQuantity = defaultTotalQuantity;
        // estimated cost per unit
        decimal estCostPerUnit = defaultEstCostPerUnit;
        // estimated profit per unit
        decimal estProfitPerUnit = defaultEstProfitPerUnit;
        // estimated sales required before profit starts
        int estSalesToProfit = defaultEstSalesToProfit;
        // estimated total cost
        decimal estTotalCost = defaultEstTotalCost;
        // estimated profit
        decimal estProfit = defaultEstProfit;
        // estimated profit less gst
        decimal estProfitLessGst = defaultEstProfitLessGst;
        // australian gst multiplier (10%)
        double gstMultiplier = GST_AMOUNT;
        // gst to pay
        decimal estGstTotal = defaultEstGstTotal;
        // rough price per box
        decimal estFlatBoxCost = defaultEstFlatBoxCost;
        // rough shipping cost per unit
        decimal estShippingCost = defaultEstShippingCost;

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
        static public Regex numberDigitPattern = new Regex(@"\d", RegexOptions.Compiled);
        // variable used to parse valid keys for textboxes
        string numberDigit = numberDigitPattern.ToString();

        // stores valid keys that can be typed into textboxes
        Keys[] validKeyArray = { Keys.Back, Keys.Oemcomma, Keys.OemPeriod, Keys.Decimal, Keys.ShiftKey, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Tab, Keys.Delete, Keys.Oemcomma };
        // stores last character removed by filter
        Keys lastCharRemoved;

        // stores current index of mouse cursor
        int oldIndex = 0;


        #endregion

        public Main()
        {
            InitializeComponent();
            // sets up application defaults
            Setup();
        }

        #region Setup()
        /// <summary>
        /// Sets up application defaults
        /// </summary>
        public void Setup()
        {
            // sets height of app to match working area of the current screen
            this.Height = Screen.GetWorkingArea(this).Height;
            // defaults the app to the top of the screen
            this.Top = 0;
            // resets all values to their defaults
            ResetAll();
            // focuses cursor onto textBoxGoodyCost
            textBoxGoodyCost.Focus();
            // sets cursor to the end of the text in textBoxGoodyCost
            textBoxGoodyCost.SelectionStart = textBoxGoodyCost.TextLength;

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
            goodyLashCost = defaultGoodyLashCost;
            goodyTotalCostPerUnit = defaultGoodyTotalCostPerUnit;


            ///////
            // Resets olivia values
            ////

            oliviaTotal = defaultOliviaTotal;
            oliviaQuantity = defaultOliviaQuantity;
            oliviaLashCost = defaultOliviaLashCost;
            oliviaTotalCostPerUnit = defaultOliviaTotalCostPerUnit;


            ///////
            // Resets boxing values
            ////

            boxingCost = defaultBoxingCost;
            estimatedHoursToBox = defaultEstimatedHoursToBox;
            hoursSpentBoxing = defaultHoursSpentBoxing;


            ///////
            // Resets PA values
            ////

            paHourlyRate = defaultPaHourlyRate;
            paHoursSpentThisOrder = defaultPaHoursSpentThisOrder;


            ///////
            // Resets estimated figures
            ////

            totalQuantity = defaultTotalQuantity;
            estCostPerUnit = defaultEstCostPerUnit;
            estProfitPerUnit = defaultEstProfitPerUnit;
            estSalesToProfit = defaultEstSalesToProfit;
            estTotalCost = defaultEstTotalCost;
            estProfit = defaultEstProfit;
            estProfitLessGst = defaultEstProfitLessGst;
            gstMultiplier = GST_AMOUNT;
            estGstTotal = defaultEstGstTotal;
            estFlatBoxCost = defaultEstFlatBoxCost;
            estShippingCost = defaultEstShippingCost;

        } // end void
        #endregion

        #region Main_KeyUp Event
        /// <summary>
        /// When any key is lifted, checks if all controls are valid to enable/disable "Create Account" button and displays tool tips to assist user with validation process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {

                // if the active control of this form is a textbox
                if (ActiveControl is TextBoxBase)
                {

                    // unlocks the proper textbox properties
                    TextBox textBox = ActiveControl as TextBox;
                    // creates a string reference of the textbox being used atm
                    string text = textBox.Text;
                    // creates a string reference of the char being used atm
                    string keyPressed = e.KeyCode.ToString();


                    // stores current index
                    oldIndex = textBox.SelectionStart;

                    // if textbox is empty
                    if (text is null or "" or "$")
                    {
                        ResetAll();
                        // resets prices
                        ClearDisplays();
                        // writes info to console
                        Log("Event handled due to null or empty textbox");
                        // breaks out of event
                        Calculupdate();

                    } // end if

                    // else if user was releasing any key other than the "enter key" and currently active control is a textbox
                    else if (!(e.KeyCode is Keys.Enter))
                    {


                        // if pressed key is a valid key defined by regex and keystrokes array
                        if (!(Regex.IsMatch(keyPressed, numberDigit) || validKeyArray.Contains(e.KeyCode)))
                        {
                            // if the user enters the same invalid character twice in a row
                            if (lastCharRemoved == e.KeyCode)
                            {
                                // informs the user that they may not use that character
                                MessageBox.Show("\"" + e.KeyCode + "\" is not a valid input!");

                            } // end if

                            // replaces keypress with nothing
                            textBox.Text = text.Remove(oldIndex - 1, 1);
                            // stores this character as the last removed character
                            lastCharRemoved = e.KeyCode;
                            return;

                        } // end if

                        // if user has selected autofill on
                        if (checkBoxAutoFill.Checked)
                        {
                            // calculates and updates values
                            Calculupdate();

                        } // end if

                    } // end if

                    SetCursor();

                } // end if

                // else if user was releasing the "enter" key and currently active  control is not a textbox
                else
                {
                    // if user releases enter once in a row
                    if (e.KeyCode == Keys.Enter && spamProtect == false && e.KeyCode != Keys.Back)
                    {
                        //sets spam protection to true to prevent double entering while trying to exit an error dialog
                        spamProtect = true;
                    }
                    // else if user presses twice in a row
                    else
                    {
                        // breaks out to prevent looping
                        spamProtect = false;
                        return;

                    } // end if

                    // if the currently active control is a checkbox
                    if (ActiveControl is CheckBox thisCheckBox)
                    {
                        // checks/unchecks current textbox depending on current state
                        thisCheckBox.Checked = thisCheckBox.Checked = false ? true : false;
                    }
                    // else if the current active control is not a checkbox
                    else
                    {
                        // calculates new textbox values and updates textboxes
                        Calculupdate();

                    } // end if

                } // end if
            }
            catch
            {
                LogError("PHere?");

            } // end try

        }
        #endregion

        #region SetCursor()
        /// <summary>
        /// Sets cursor back into it's correct position
        /// </summary>

        private void SetCursor()
        {
            TextBox textBox = ActiveControl as TextBox;

            // if textbox is displaying $0
            if (textBox.Text is "$0")
            {
                // sets cursor to the end
                textBox.SelectionStart = 2;
            }
            // else if textbox is display a $ symbol
            else if (textBox.Text is "$")
            {
                // sets cursor to the end
                textBox.SelectionStart = 1;
            }
            // if textbox text length is equal to 2
            else if (textBox.TextLength is 2)
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
        }
        #endregion

        #region ClearDisplays()

        /// <summary>
        /// Clears all display boxes
        /// </summary>
        private void ClearDisplays()
        {
            // clears all display boxes
            textBoxRoughBoxCost.Text = "$0" + defaultEstFlatBoxCost.ToString("#.00");
            textBoxRoughShippingCost.Text = "$0" + defaultEstShippingCost.ToString("#.00");
            textBoxEstTotalPaCost.Text = "$0";
            textBoxEstCostPerUnit.Text = "$0";
            textBoxEstProfitPerUnit.Text = "$0";
            textBoxEstSalesToProfit.Text = "0";
            textBoxTotalQuantity.Text = "0";
            textBoxEstTotalCost.Text = "$0";
            textBoxEstProfit.Text = "$0";
            textBoxEstProfitLessGst.Text = "$0";
            textBoxEstGstToPay.Text = "$0";


        } // end void

        #endregion

        #region Calculupdate()
        /// <summary>
        /// Calculates our new values and updates the textboxes with results
        /// </summary>
        private void Calculupdate()
        {
            try
            {

                TextBox[] displayBoxes = { textBoxEstTotalPaCost, textBoxEstCostPerUnit , textBoxEstProfitPerUnit, textBoxEstSalesToProfit,
                                         textBoxTotalQuantity, textBoxEstTotalCost, textBoxEstProfit, textBoxEstProfitLessGst, textBoxEstGstToPay };

                Panel[] panelList = { panelGoody, panelOlivia, panelPa, panelUsdToAud };

                ///////
                /// Parses all values to their respective variables
                /////

                foreach (Panel panel in panelList)
                {
                    Panel thisPanel = panel as Panel;

                    //MessageBox.Show(thisPanel.Name.ToString());

                    // for each control in this form
                    foreach (Control control in thisPanel.Controls)
                    {
                        // if control is not a textbox
                        if (!(control is TextBox))
                        {
                            // continues to next control
                            continue;
                        }


                        // sets control to a proper textbox for referencing
                        TextBox textBox = control as TextBox;
                        // creates a string reference for the name of the textbox being used
                        string textBoxName = textBox.Name;
                        // creates a string reference for the text in the textbox being used
                        string text = textBox.Text;

                        //MessageBox.Show(textBox.ToString());

                        // if the current textbox is used for display only
                        if (displayBoxes.Contains(textBox))
                        {
                            // continues to next control
                            continue;

                        } // end if

                        if (!(text is null))
                        {
                            // if the text has a $ symbol in it
                            if (text.Contains("$"))
                            {
                                // replaces it with nothing to avoid calculation errors
                                text = text.Replace("$", "");

                            } // end if

                            // if the text is empty
                            if (text is "")
                            {
                                // sets the text value to 0
                                text = "0";

                            } // end if

                            switch (textBoxName)
                            {

                                case "textBoxGoodyCost":
                                    // stores user input to goodyCost variable
                                    goodyTotal = double.Parse(text);

                                    break;

                                case "textBoxOliviaCost":
                                    // stores user input to oliviaCost variable
                                    oliviaTotal = double.Parse(text);

                                    break;

                                case "textBoxGoodyQuantity":
                                    // stores user input to goodyCost variable
                                    goodyQuantity = int.Parse(text);

                                    break;

                                case "textBoxOliviaQuantity":
                                    // stores user input to olivia quantity variable
                                    oliviaQuantity = int.Parse(text);

                                    break;

                                case "textBoxPaHourlyRate":
                                    // stores user input to textBoxPaHourlyRate
                                    pA_HourlyRate = double.Parse(text);

                                    break;

                                case "textBoxPaHoursSpentBoxing":
                                    // stores user input to textBox spent boxing
                                    hoursSpentBoxing = double.Parse(text);

                                    break;

                                case "textBoxPaHoursToBox":
                                    // stores user input into boxes per hour variable
                                    pA_EstimatedHoursToBox = double.Parse(text);

                                    break;

                                case "textBoxRoughBoxCost":
                                    // stores user input to rough box price variable
                                    roughBoxCost = decimal.Parse(text);

                                    break;

                                case "textBoxRoughShippingCost":
                                    // stores user input to rough shipping cost variable
                                    estShippingCost = decimal.Parse(text);

                                    break;

                                case "textBoxUsdToAud":
                                    // stores user input usd to aud conversion rate (default 1.5)
                                    estUsdToAusMultiplier = double.Parse(text);

                                    break;

                                // if no case was matched
                                default:
                                    // logs error
                                    LogError("[" + DateTime.Now + "]" + "Failed to process control: " + textBox);

                                    break;

                            } // end switch 

                        } // end if 

                    } // end for

                } // end for

                // calculates total quantity since this is needed for most calculations
                totalQuantity = goodyQuantity + oliviaQuantity;

                ///////
                /// Calculate personal assistant costs
                /////

                // if personal assistant checkbox is checked AND P.A's hourly rate is not null
                if (checkBoxPaCosts.Checked)
                {
                    // if P.A. hourly rate is greater than max rate OR less than min rate
                    if (pA_HourlyRate > MAX_HOURLY_RATE || pA_HourlyRate < MIN_HOURLY_RATE)
                    {
                        // writes error message to user
                        textBoxPaHourlyRate.ShowUsYaTips("Please ensure hourly rate is valid!");

                    }

                    // roughly estimates amount of hours it will take to box this order
                    pA_EstimatedHoursToBox = goodyQuantity / pA_BoxesPerHour;
                    // subtracts hours spent boxing from estimated hours to box
                    pA_EstimatedHoursToBox -= hoursSpentBoxing;
                    // sets personal assistants estimated cost to hourly rate * estimated hours to box
                    pA_EstimatedCost = decimal.Parse((pA_HourlyRate * pA_EstimatedHoursToBox).ToString());

                    ///////
                    /// Calculate other costs
                    /////

                    // if checkbox estimate prices by quantity is checked
                    if (checkBoxEstPricesByQuantity.Checked)
                    {
                        // sets goody cost to an estimated value based on a past order
                        goodyTotal = goodyQuantity * (double)defaultGoodyCost;
                        // sets olivia cost to an estimated value based on a past order
                        oliviaTotal = oliviaQuantity * (double)oliviaTotalCostPerUnit;

                    } // end if


                } // end if

                // if goody cost and quantity are not equal to 0 OR olivia cost and quantity are not equal to 0
                if (goodyTotal + goodyQuantity > 0 || oliviaTotal + oliviaQuantity > 0)
                {
                    // adds the cost of both orders together for a total cost
                    estTotalCost = decimal.Parse((goodyTotal + oliviaTotal + (double)pA_EstimatedCost).ToString());
                    // estimates the cost per lash set
                    estCostPerUnit = estTotalCost / totalQuantity;
                    // estimated profit per sale
                    estProfitPerUnit = retailSalePrice - estCostPerUnit;
                    // sales required to profit
                    estSalesToProfit = (int)estTotalCost / (int)estProfitPerUnit + 1;
                    // adds quantities of both orders together for a total quantity
                    totalQuantity = goodyQuantity + oliviaQuantity;
                    // total profit margin this order
                    estProfit = estProfitPerUnit * totalQuantity;
                    // total profit margin this order minus gst
                    estProfitLessGst = estProfit - estProfit * (decimal)gstMultiplier;
                    // gst amount
                    estGstTotal = estProfit * (decimal)gstMultiplier;

                    // rough box cost
                    roughBoxCost = totalQuantity * defaultEstFlatBoxCost;
                    // rough shipping cost
                    estShippingCost = totalQuantity * defaultEstShippingCost;


                } // end if


                Display();

            } // end try
            catch (Exception ex)
            {
                // writes error to console
                LogError(ex.Message);

            } // end try

        }

        #endregion

        #region Display()

        private void Display()
        {
            try
            {
                ///////
                /// Displays monetary values to respective textboxes in correct currency
                /////

                if (goodyTotal != 0)
                {
                    // displays goody cost in USD
                    textBoxGoodyCost.Text = "$" + goodyTotal.ToString("#.##");

                } // end if

                if (oliviaTotal != 0)
                {
                    // displays olivia cost in USD
                    textBoxOliviaCost.Text = "$" + oliviaTotal.ToString("#.##");

                } // end if

                // displays estimated total cost in USD
                textBoxEstTotalCost.Text = "$" + (goodyTotal + oliviaTotal).ToString();

                // if roughBoxCost is greater than $1
                if (roughBoxCost > 1)
                {
                    // displays rough box cost in USD to 2.d.p
                    textBoxRoughBoxCost.Text = "$" + roughBoxCost.ToString("#.00");
                }
                // else if roughBoxCost is less than $1
                else
                {
                    // displays rough box cost in USD to 2.d.p
                    textBoxRoughBoxCost.Text = "$0" + roughBoxCost.ToString("#.00");

                } // end if

                // if roughShippingCost is greater than $1
                if (estShippingCost > 1)
                {
                    // displays roughShippingCost in USD to 2.d.p.
                    textBoxRoughShippingCost.Text = "$" + estShippingCost.ToString("#.00");
                }
                // else if roughShippingCost is less than $1
                else
                {
                    // displays roughShippingCost in USD to 2.d.p
                    textBoxRoughShippingCost.Text = "$0" + estShippingCost.ToString("#.00");

                } // end if

                // if P.A's hours spent boxing AND est hours to box is both empty
                if (hoursSpentBoxing + pA_EstimatedHoursToBox > 0)
                {
                    // display hours spent boxing to 1 d.p.
                    textBoxPaHoursSpentBoxing.Text = hoursSpentBoxing.ToString("#.#");
                    // display pa's estimated hours to box to 1 d.p.
                    textBoxPaEstHoursToBox.Text = pA_EstimatedHoursToBox.ToString("#.#");
                }
                // display estimated p.a. cost
                textBoxEstTotalPaCost.Text = pA_EstimatedCost.ToString("c2");
                // updates estimated cost per unit
                textBoxEstCostPerUnit.Text = estCostPerUnit.ToString("c2");
                // updates estimated profit per unit
                textBoxEstProfitPerUnit.Text = (estProfitPerUnit.ToString("c2"));
                // updates estimated sales until we start to see profit to 0 d.p.
                textBoxEstSalesToProfit.Text = estSalesToProfit.ToString();
                // updates total quantity
                textBoxTotalQuantity.Text = totalQuantity.ToString();
                // updates estimated total cost
                textBoxEstTotalCost.Text = estTotalCost.ToString("c2");
                // updates estimated profit
                textBoxEstProfit.Text = estProfit.ToString("c2");
                // updates profit less gst
                textBoxEstProfitLessGst.Text = estProfitLessGst.ToString("c2");
                // updates gst to pay
                textBoxEstGstToPay.Text = estGstTotal.ToString("c2");

                // if active control is a textbox
                if (ActiveControl is TextBox)
                {
                    // creates a reference variable to the active textbox
                    TextBox textBox = ActiveControl as TextBox;

                    // 
                    if (textBox.ReadOnly)
                    {
                        return;

                    } // end if

                    // if textbox text is $0
                    if (textBox.Text is "$0")
                    {
                        // sets cursor to 2
                        textBox.SelectionStart = 2;

                    } // end if

                } // end if
            }
            catch (Exception ex)
            {
                // writes error to console
                LogError(ex.Message);
            }
        }

        #endregion

        #region buttonUsdToAud_Click

        private void buttonUsdToAud_Click(object sender, EventArgs e)
        {
            // displays goody cost in AUD
            textBoxGoodyCost.Text = (goodyTotal * estUsdToAusMultiplier).ToString("c2");
            // displays olivia cost in AUD
            textBoxOliviaCost.Text = (oliviaTotal * estUsdToAusMultiplier).ToString("c2");
            // displays rough box cost in AUD
            textBoxRoughBoxCost.Text = (roughBoxCost * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays rough shipping cost in AUD
            textBoxRoughShippingCost.Text = (estShippingCost * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated cost per unit in AUD
            textBoxEstCostPerUnit.Text = (estCostPerUnit * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated profit per unit in AUD
            textBoxEstProfitPerUnit.Text = (estProfitPerUnit * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated sales to profit in AUD
            textBoxEstSalesToProfit.Text = (estSalesToProfit * (decimal)estUsdToAusMultiplier).ToString();
            // displays estimated total cost in AUD
            textBoxEstTotalCost.Text = ((goodyTotal + oliviaTotal) * estUsdToAusMultiplier).ToString("c2");
            // displays estimated profit in AUD
            textBoxEstProfit.Text = (estProfit * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated profit less GST in AUD
            textBoxEstProfitLessGst.Text = (estProfitLessGst * (decimal)estUsdToAusMultiplier).ToString("c2");
            // displays estimated GST to pay in AUD
            textBoxEstGstToPay.Text = (estGstTotal * (decimal)estUsdToAusMultiplier).ToString("c2");
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

                // enables textboxBoxingCost


                // enables P.A's hourly rate textbox
                pA_HourlyRate = (double)defaultPaHourlyRate;
                textBoxPaHourlyRate.Enable(defaultPaHourlyRate.ToString("C2"));

                // enables P.A's estHoursToBox textbox
                textBoxPaEstHoursToBox.Enable();

                // enables P.A's hoursSpentBoxing textbox
                textBoxPaHoursSpentBoxing.Enable();

            }
            // else if P.A. checkbox is not checked
            else
            {
                // disables P.A's hourly rate textbox
                pA_HourlyRate = 0;
                textBoxPaHourlyRate.Disable();

                // disables P.A's hours spent boxing textbox
                pA_Ho



            } // end if

            Calculupdate();

        }
        #endregion

        #region checkBoxRoughBox_CheckChanged Event
        /// <summary>
        /// Shows/Hides extra boxes controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxRoughBoxes_CheckedChanged(object sender, EventArgs e)
        {
            // if extra boxes checkbox is checked
            if (checkBoxShowFlatBoxEst.Checked)
            {
                // show extra boxes controls
                labelRoughBoxCost.Show();
                textBoxRoughBoxCost.Show();
                labelRoughShippingCost.Show();
                textBoxRoughShippingCost.Show();
                Calculupdate();

            }
            // else if extra boxes checkbox is not checked
            else
            {
                // show extra boxes controls
                labelRoughBoxCost.Hide();
                textBoxRoughBoxCost.Hide();
                labelRoughShippingCost.Hide();
                textBoxRoughShippingCost.Hide();
                Calculupdate();

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
        }
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

        private void checkBoxEstPricesByQuantity_CheckedChanged(object sender, EventArgs e)
        {
            // if checkbox is checked
            if (checkBoxEstPricesByQuantity.Checked)
            {
                // calculates values
                Calculupdate();
            }
            // else if checkbox is not checked
            else
            {
                // resets price values
                goodyTotal = 0;
                oliviaTotal = 0;
                // resets price displays
                textBoxGoodyCost.Text = "$0";
                textBoxOliviaCost.Text = "$0";

            } // end if

        } // end void 

        private void checkBoxDeductHoursSpent_CheckedChanged(object sender, EventArgs e)
        {
            Calculupdate();

        } // end void

    } // end class

} // end namespace

