using System.Globalization;

namespace TypeConversionAssignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1.Implicit Conversion
            //assigning int to a double
            //why safe?
            //integer type can be represented by a double datatype without any loss of data so this is safe conversion.
            int intNumber = 1;
            double doubleNumber = intNumber;

            Console.WriteLine($"Int number: {intNumber}");
            Console.WriteLine($"Converted to double: {doubleNumber} \n");

            //assigning char to an int
            //why safe?
            //interger being 32 bit datatype and char being 16 bit datatype, integer type can represent all the possible value of the char datatype, so no loss of data take place.
            char charValue = 'A';
            int unicodeValue = charValue;
            Console.WriteLine($"Char value:{charValue}");
            Console.WriteLine($"Unicode value: {unicodeValue} \n");

            //2.Explicit Conversion
            //converting double to an int
            double doubleValue = 3.14;
            int intValue = (int)doubleValue; //explicitly telling compliler to convert from double to int
            Console.WriteLine($"Double number: {doubleValue}");
            Console.WriteLine($"Converted to int: {intValue}  \n");
            //here we can see the floating point value (.14) is lost while converting from double to int


            //converting from float to byte
            float floatValue = 8.14f;
            byte byteValue = (byte)floatValue; //explicit conversion
            Console.WriteLine($"Float number: {floatValue}");
            Console.WriteLine($"Converted to byte: {byteValue} \n");
            //here we can see the floating point value (.14) is lost while converting from float to byte


            //3.using Convert class
            string strValue = "100";
            //converting string to int
            int strToIntValue = Convert.ToInt32(strValue);

            //converting string to double
            double strToDoubleValue = Convert.ToDouble(strValue);

            //converting string to bool
            bool strToBoolValue = Convert.ToBoolean("True");

            Console.WriteLine($"string Value:{strValue}");
            Console.WriteLine($"Converted to int:{strToIntValue}");
            Console.WriteLine($"Converted to Double:{strToDoubleValue}");
            Console.WriteLine($"Converted to bool:{strToBoolValue}\n");

            //4.Parsing strings
            string strValue1 = "250";
            int strToIntByParse = int.Parse(strValue1);
            Console.WriteLine($"String Value: {strValue1}");
            Console.WriteLine($"Convert to int by parse{strToIntByParse} \n");

            string strValue2 = "25A";
            try
            {
                int strToIntByParse2 = int.Parse(strValue2); //trying to convert "25A" to int
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // here we can see the exception message as incorrect format, as "25A" cannot be converted to int



            //5. Try parsing
            string input1 = "999";
            string input2 = "99X";
            //trying to convert from string to int, returns bool ifsuccess or not and result of parsing
            bool isSuccess1 = int.TryParse(input1, out int number1);

            Console.WriteLine($"\n Successfylly Parsed :{isSuccess1}, value={number1}");

            bool isSuccess2 = int.TryParse(input2, out int number2);

            Console.WriteLine($" Successfylly Parsed :{isSuccess2}, value={number2} \n");

            //6.Converting between object(is/as)
            object obj1 = "hello";
            string str1 = obj1 as string;

            Console.WriteLine($" object to string using 'as': {str1}");

            if (obj1 is string) // check if obj is string
            {
                string str2 = (string)obj1;
                Console.WriteLine($"object to string using 'is': {str2} \n");
            }

            //7.Boxing and Unboxing
            int originalValue = 42;
            Console.WriteLine($"Original int value: {originalValue}");

            object boxedValue = originalValue; //converting from int to object
            Console.WriteLine($"Boxed value (as object): {boxedValue}");

            int unboxedValue = (int)boxedValue; //converting from object to int
            Console.WriteLine($"Unboxed value (back to int): {unboxedValue}");
        }
    }
}
