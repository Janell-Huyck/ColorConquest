using System.Globalization;
using ColorConquest.Core;
using ColorConquest.Core.Converters;
using Xunit;

namespace ColorConquest.Tests.Converters
{
    public class BoardSizeToBoolConverterTests
    {
        private readonly BoardSizeToBoolConverter _converter = new();

        [Theory]
        [InlineData(BoardSize.Easy, BoardSize.Easy, true)]
        [InlineData(BoardSize.Easy, BoardSize.Medium, false)]
        [InlineData(BoardSize.Medium, BoardSize.Medium, true)]
        [InlineData(BoardSize.Hard, BoardSize.Easy, false)]
        public void Convert_EnumParameter_Works(BoardSize value, BoardSize param, bool expected)
        {
            var result = _converter.Convert(value, typeof(bool), param, CultureInfo.InvariantCulture);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(BoardSize.Easy, "Easy", true)]
        [InlineData(BoardSize.Easy, "Medium", false)]
        [InlineData(BoardSize.Medium, "Medium", true)]
        [InlineData(BoardSize.Hard, "Easy", false)]
        public void Convert_StringParameter_Works(BoardSize value, string param, bool expected)
        {
            var result = _converter.Convert(value, typeof(bool), param, CultureInfo.InvariantCulture);
            Assert.Equal(expected, result);
        }
    }
}
