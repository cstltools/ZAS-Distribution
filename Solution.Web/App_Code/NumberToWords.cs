using System;

public static class NumberToWords
{
    static readonly string[] Ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
        "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
    static readonly string[] Tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

    public static string Convert(decimal amount)
    {
        long number = (long)Math.Abs(amount);
        if (number == 0) return "Zero";

        string result = "";

        result += ConvertGroup(number / 10000000); if (number / 10000000 > 0) result += " Crore ";
        number %= 10000000;
        // "Lakhs" (plural, always) matches the wording used by the original Crystal Group Invoice
        // report (verified against a real generated PDF: "Three Lakhs Thirteen Thousand...").
        result += ConvertGroup(number / 100000); if (number / 100000 > 0) result += " Lakhs ";
        number %= 100000;
        result += ConvertGroup(number / 1000); if (number / 1000 > 0) result += " Thousand ";
        number %= 1000;
        result += ConvertGroup(number);

        return System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ").Trim();
    }

    static string ConvertGroup(long number)
    {
        if (number == 0) return "";
        string result = "";

        if (number >= 100)
        {
            result += Ones[number / 100] + " Hundred ";
            number %= 100;
        }

        if (number >= 20)
        {
            result += Tens[number / 10] + " ";
            number %= 10;
        }

        if (number > 0)
        {
            result += Ones[number] + " ";
        }

        return result.Trim();
    }
}
