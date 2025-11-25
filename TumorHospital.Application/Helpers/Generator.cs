namespace TumorHospital.Application.Helpers
{
    public static class Generator
    {
        private static char[] upperCaseLetters = {
            'A','B','C','D','E','F','G','H','I','J','K','L','M',
            'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };
        private static char[] lowerCaseLetters = {
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z'
            };
        private static char[] numbers = {
            '0','1','2','3','4','5','6','7','8','9'
            };
        private static char[] specialChars = {
            '!','@','#','$','%','^','&','*','(',')','_','+','-','=','{','}','[',']','|',':',';','<','>',',','.','?','/','~'
            };


        public static string GenerateRandomPassword()
        {
            string password = "";
            password += GetRandomString(upperCaseLetters, 3);
            password += GetRandomString(lowerCaseLetters, 3);
            password += GetRandomString(numbers, 3);
            password += GetRandomString(specialChars, 3);
            return password;
        }
        public static string GenerateRandomBillCode()
            => GetRandomString(numbers, 12);
        private static string GetRandomString(char[] array, int numberOfChars)
        {
            string text = "";
            Random random = new Random();
            for (int i = 0; i < numberOfChars; i++)
            {
                var randomNumber = random.NextInt64(0, array.Length);
                char randomChar = array[randomNumber];
                text += randomChar;
            }
            return text;
        }


        
    }
}