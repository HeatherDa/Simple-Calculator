using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnCalculate_Click(object sender, EventArgs e)//calculate answer
        {
            try
            {
                TextBox[] operands = new TextBox[2] { txtOp1, txtOp2 };
                if (dataValidation(operands))//check for data validity
                {
                    decimal op1 = Convert.ToDecimal(txtOp1.Text);
                    decimal op2 = Convert.ToDecimal(txtOp2.Text);
                    string oper = txtOper.Text;

                    decimal answer=calculate(op1, oper, op2);
                    txtAnswer.Text = Convert.ToString(answer); //another way to do this would be an if statement where txtAnswer is set depending on whether /by0 bool was set to true.
                    divideByZero(); //handle divide by zero if it happened.
                }
            }
            catch(Exception ex)//not really sure whay kind of exception I'm expecting.
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.GetType().ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)//exit button
        {
            this.Close();
        }
        public bool IsPresent (TextBox textBox, string name) //is there data in the text box?
        {
            if (textBox.Text == "")
            {
                MessageBox.Show(name + " is a required field.", "Entry Error");
                textBox.Focus();
                return false;
            }
            return true;
        }
        public bool IsNumber(TextBox textBox, string name) //is the data numeric?
        {
            decimal numdec = 0m;
            if (Decimal.TryParse(textBox.Text, out numdec))
            {
                return true;
            }
            else
            {
                MessageBox.Show(name + " must be a number.", "Entry Error");
                textBox.Focus();
                return false;
            }
        }
        public bool IsInRange(TextBox textBox, string name, decimal min, decimal max) //is the number in the right range?
        {
            decimal number = Convert.ToDecimal(textBox.Text);
            if (number < min || number > max)
            {
                MessageBox.Show(name + " must be between " + min.ToString() + " and "+ max.ToString() + ".", "Entry Error");
                textBox.Focus();
                return false;
            }
            return true;
        }
        private bool IsOperator()//check if the operator is valid
        {
            string oper = txtOper.Text.Trim();
            if (!IsPresent(txtOper, "Operator")) return false;//check that the text box contains something
            else if (oper == "*" || oper == "/" || oper == "-" || oper == "+") return true; //check that it contains the right thing
            else //what to do if the textbox contains the wrong thing
            {
                MessageBox.Show("Operator must be *, /, -, or +.", "Entry Error");
                txtOper.Focus();
                return false;
            }
        }
        private void divideByZero()//test for divide by zero and return error message
        {
            if (txtOper.Text == "/" && (txtOp2.Text == "0"))
            {
                txtAnswer.Text="";
                MessageBox.Show("Cannot divide by zero.", "Entry Error");
                txtOp2.Focus();
            }
        }
        private bool dataValidation(TextBox[] textBoxes)//can only return one error message at a time. (what if both textboxes wrong?)
        {
            
            foreach(TextBox t in textBoxes)
            {
                string name = "";
                if (t.Name.Contains("1")) name = "Operand 1";
                else if (t.Name.Contains("2")) name = "Operand 2";
                if (!(IsPresent(t, name) && IsNumber(t, name) && IsInRange(t, name, 0, 1000000) && IsOperator()))
                {
                    return false; //if any txtbox evaluates to false, stop checking.
                }
            }

            return true; //should only reach this if they are all true

        }

        private void textChange(object sender, EventArgs e)
        {
            //if text is changed in any textbox, clear answer textbox
            txtAnswer.Text = "";
        }

        private decimal calculate(decimal operand1, string operator1, decimal operand2)
        {
            decimal answer = 0;
            try
            {
                
                switch (operator1)
                {
                    case "/":
                        if (operand2 == 0) break; //exit loop without calculating or assigning a value if it would divide by zero
                        else answer = operand1 / operand2;
                        return answer;
                    case "*":
                        answer = operand1 * operand2;
                        return answer;
                    case "-":
                        answer = operand1 - operand2;
                        return answer;
                    case "+":
                        answer = operand1 + operand2;
                        return answer;
                }
                answer = Math.Round(answer, 4);//round to 4 decimal places
            }
            catch(OverflowException)//this should never happen, because of data validation.
            {
                MessageBox.Show("An overflow exception has occured.  Please enter smaller values.", "Entry Error");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
            return answer; //returns decimal.  
            //I originally wrote it to set txtAnswer.Text directly, but changed it because of the instructions.
            //Is there a reason why it's better to return the decimal and then assign it?
        }
    }
}
