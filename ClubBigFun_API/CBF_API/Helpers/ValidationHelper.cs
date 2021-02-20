using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CBF_API.Helpers
{

    public class ValidationHelper
    {

        public ValidationHelper()
        {

        }

        //public class Validate
        //{

        public static string StrongPAsswordValidation(string password)
        {
            //string regex =  (?=^.{8,25}$)(?=(?:.*?\d){1})(?=.*[a-z])(?=(?:.*?[A-Z]){2})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$

            string result = "";
            Regex re = new Regex(@"(?=^.{8,25}$)(?=(?:.*?\d){1})(?=.*[a-z])(?=(?:.*?[A-Z]){1})(?=(?:.*?[!@#$%*()_+^&}{:;?.,';~`]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&}{:;?.,';~`^&]*$");
            if (!String.IsNullOrEmpty(password.Trim()) && re.IsMatch(password))
            {
                return result;
            }
            else
            {
                return "Password does not match with rule. Please see the password instruction.";
            }
        }

        public static string RedactSSN(string stringToRedact)
        {
            const string pattern = @"\d{3}-\d{2}-\d{4}";
            stringToRedact = Regex.Replace(
                stringToRedact,
                pattern,
                m => "***-**-" + m.Value.Substring(m.Value.Length - 4, 4));
            return stringToRedact;
        }


        public static string IsValidEmail(string inputEmail)
        {
            string result = "";
            Regex re = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(inputEmail.Trim()) && re.IsMatch(inputEmail))
            {
                return result;
            }
            else
            {
                return "Invalid Email Address";
            }
        }

        public static string IsValidEmail(string inputEmail, string message)
        {
            string result = "";
            Regex re = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(inputEmail.Trim()) && re.IsMatch(inputEmail))
            {
                return result;
            }
            else
            {
                return message;
            }
        }
        public static string IsValidUrl(string inputUrl)
        {
            string result = "";
            string url = inputUrl;
            if (!String.IsNullOrEmpty(inputUrl.Trim()) && Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"))
            {
                return result;
            }
            else
            {
                return "Invalid Url";
            }
        }
        public static string IsRequired(string inputReq, string fieldName)
        {
            string result = "";
            if (!String.IsNullOrEmpty(inputReq.Trim()))
            {
                return result;
            }
            else
            {
                return fieldName + "";
            }
        }
        public static string IsOnlyAlphabet(string inputReq, string message, bool withMsg)
        {
            string result = "";
            Regex re = new Regex(@"^[a-zA-Z\s\.\,\'\-]+$", RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(inputReq.Trim()) && re.IsMatch(inputReq))
            {
                return result;
            }
            else
            {
                return message;
            }
        }

        public static string IsOnlyAlphabet(string inputReq, string message)
        {
            string result = "";
            Regex re = new Regex(@"^[a-zA-Z\s\.\'\-]+$", RegexOptions.IgnoreCase);
            if (!String.IsNullOrEmpty(inputReq.Trim()) && re.IsMatch(inputReq))
            {
                return result;
            }
            else
            {
                return message;
            }
        }
        public static string IsRequired(string inputReq, string message, bool withmessage)
        {
            string result = "";
            if (!String.IsNullOrEmpty(inputReq.Trim()))
            {
                return result;
            }
            else
            {
                return message;
            }
        }

        public static string IsRequiredDDL(string inputReq, string errormessage)
        {
            string result = "";
            if (inputReq != "-1" && inputReq != "Select")
            {
                return result;
            }
            else
            {
                return errormessage;
            }
        }

        public static string IsValidDate(string inputDate)
        {
            string result = "";
            string date = inputDate;
            if (!String.IsNullOrEmpty(inputDate.Trim()) && Regex.IsMatch(date, @"^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"))
            {
                return result;
            }
            else
            {
                return "Invalid Date";
            }
        }
        public static string IsValidDateMMDDYYYY(string inputDate, string errormessage)
        {
            //string result = "";
            //string date = inputDate;
            //if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"^(((((0[13578])|([13578])|(1[02]))[\-\/\s]?((0[1-9])|([1-9])|([1-2][0-9])|(3[01])))|((([469])|(11))[\-\/\s]?((0[1-9])|([1-9])|([1-2][0-9])|(30)))|((02|2)[\-\/\s]?((0[1-9])|([1-9])|([1-2][0-9]))))[\-\/\s]?\d{4})(\s(((0[1-9])|([1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"))
            //    return result;
            //else
            //    return errormessage;

            string result = "";
            string date = inputDate;
            if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"))
            {
                return result;
            }
            if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"^(((((0[13578])|([13578])|(1[02]))[\-\/\s]?((0[1-9])|([1-9])|([1-2][0-9])|(3[01])))|((([469])|(11))[\-\/\s]?((0[1-9])|([1-9])|([1-2][0-9])|(30)))|((02|2)[\-\/\s]?((0[1-9])|([1-9])|([1-2][0-9]))))[\-\/\s]?\d{4})(\s(((0[1-9])|([1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$"))
            {
                return result;
            }
            else if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"^(\d{1,2})/(\d{1,2})/(\d{4})?$"))
            {
                return result;
            }
            else if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"^(0?[1-9]|[12][0-9]|3[01])/(0?[1-9]|1[012])/((19|20)\d\d)?$"))
            {
                return result;
            }
            else
            {
                return errormessage;
            }
        }
        public static string IsValideDateYear(string inputDate, string errormessage)
        {
            string result = "";
            string date = inputDate;
            if (date != "" && Regex.IsMatch(date, @"^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"))
            {
                DateTime inputDateValue = Convert.ToDateTime(inputDate);
                if (!String.IsNullOrEmpty(inputDate) && inputDateValue.Date.Year <= DateTime.Now.AddYears(100).Year)
                {
                    return result;
                }
                else
                {
                    return errormessage;
                }
            }
            else
            {
                return errormessage;
            }
        }

        public static string IsValidIntDecimal(string inputInt, string errormessag)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(date) && Regex.IsMatch(date, @"\d+(\.\d{1,2})?$"))
            {
                return result;
            }
            else
            {
                return errormessag;
            }
        }
        public static string IsValidYear(string year, string errormessag)
        {
            string result = "";

            if (!String.IsNullOrEmpty(year) && Regex.IsMatch(year, @"^(19|20)\d{2}$"))
            {
                return result;
            }
            else
            {
                return errormessag;
            }
        }
        public static string IsValidMonth(string month, string errormessag)
        {
            string result = "";

            if (!String.IsNullOrEmpty(month) && Regex.IsMatch(month, @"^(0?[1-9]|1[012])$"))
            {
                return result;
            }
            else
            {
                return errormessag;
            }
        }
        public static string IsValidDateFormates(string inputDate, string errormessage)
        {
            string result = "";
            string date = inputDate;
            if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"(^(1[0-2]|0[1-9]|\d)\/(20\d{2}|19\d{2}|0(?!0)\d|[1-9]\d)$)"))
            {
                return result;
            }
            else if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"(^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$)"))
            {
                return result;
            }
            else if (!String.IsNullOrEmpty(inputDate) && Regex.IsMatch(date, @"((0|1)[0-2]\/[0-3][0-9]\/[0-9]{2})"))
            {
                return result;
            }
            else
            {
                return errormessage;
            }
        }


        public static string IsValidInt(string inputInt)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^\d+$"))
            {
                return result;
            }
            else
            {
                return "Invalid Number";
            }
        }

        public static string IsValidInt(string inputInt, string message)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^\d+$"))
            {
                return result;
            }
            else
            {
                return message;
            }
        }

        public static string IsValidCreditCardNumber(string inputValue, string message)
        {
            string result = "";
            string data = inputValue.Trim();
            char[] inputCharactors = data.ToCharArray();
            if (inputCharactors.Length > 17)
            {
                return message;
            }
            if (inputCharactors.Length < 13)
            {
                return message;
            }

            if (!String.IsNullOrEmpty(data.ToString()) && Regex.IsMatch(data.ToString(), @"^[0-9]+$"))
            {
                // nothing
            }
            else
            {
                result = message;
            }



            return result;

        }

        public static string IsValidUSZIP(string inputInt)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^(?!0{5})"))
            {
                return result;
            }
            else
            {
                return "Invalid ZIP Code";
            }
        }
        public static string IsValidUSPhoneNo(string inputInt, string Message)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
            {
                return result;
            }
            else
            {
                return Message;
            }
        }
        public static string IsValidUSPhoneNo(string inputInt)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
            {
                return result;
            }
            else
            {
                return "Invalid Phone Number";
            }
        }
        public static string IsValidUSMobileNo(string inputInt)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^[+]447\d{9}$"))
            {
                return result;
            }
            else
            {
                return "Invalid Mobile Number";
            }
        }
        public static string IsValidUSMobileNo(string inputInt, string msg)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^[+]447\d{9}$"))
            {
                return result;
            }
            else
            {
                return msg;
            }
        }
        public static DateTime GetDefaultDate()
        {
            return DateTime.Now.AddYears(-100);
        }
        // }

        //public static string IsValidCreditcard(string inputInt)
        //{
        //    string result = "";
        //    string date = inputInt;
        //    if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$"))
        //        return result;
        //    else
        //        return "Please enter valid card# Visa(13 or 16 digit) and Master(16 digit).";
        //}

        //public static string IsValidCreditcard(string inputInt, string message)
        //{
        //    string result = "";
        //    string date = inputInt;
        //    if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$"))
        //        return result;
        //    else
        //        return message;
        //}
        public static string IsValidAccNo(string inputInt, string message)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"^\d{10,}$"))
            {
                return result;
            }
            else
            {
                return message;
            }
        }
        public static string IsValidUSZIP(string inputInt, string errormessage)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt) && Regex.IsMatch(date, @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)"))
            {
                return result;
            }
            else
            {
                return errormessage;
            }
        }
        public static string IsValidUSCanadaZip(string inputInt)
        {
            string result = "";
            string date = inputInt;
            if (!String.IsNullOrEmpty(inputInt.Trim()) && Regex.IsMatch(date, @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)"))
            {
                return result;
            }
            else
            {
                return "Please enter valid ZIP code eg: xxxxx-xxxx or xxxxx or T2X 1V4 or T2X1V4.";
            }
        }

        public static string GetErrorListFromModelState
                                               (ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return string.Join("<br/> ", errorList.Select(x => x));
        }
    }

}
