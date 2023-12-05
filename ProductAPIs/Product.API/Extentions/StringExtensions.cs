using System.Text;

namespace Product.API.Extentions
{
    public static class StringExtensions
    {
        public static Stream ToStream(this string input)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            return new MemoryStream(byteArray);
        }
    }
}
