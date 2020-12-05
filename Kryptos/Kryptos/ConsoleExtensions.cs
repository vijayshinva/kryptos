using System;
using System.CommandLine.Rendering;

namespace Kryptos
{
    public static class ConsoleExtensions
    {
        static TextSpanFormatter Formatter { get; } = new TextSpanFormatter();

        public static TextSpan Underline(this string value) => new ContainerSpan(StyleSpan.UnderlinedOn(), new ContentSpan(value), StyleSpan.UnderlinedOff());
        public static TextSpan Rgb(this string value, byte r, byte g, byte b) => new ContainerSpan(ForegroundColorSpan.Rgb(r, g, b), new ContentSpan(value), ForegroundColorSpan.Reset());
        public static TextSpan LightGreen(this string value) => new ContainerSpan(ForegroundColorSpan.LightGreen(), new ContentSpan(value), ForegroundColorSpan.Reset());
        public static TextSpan White(this string value) => new ContainerSpan(ForegroundColorSpan.White(), new ContentSpan(value), ForegroundColorSpan.Reset());
        public static TextSpan ToTextSpan(this FormattableString formattableString) => Formatter.ParseToSpan(formattableString);
        public static TextSpan ToTextSpan(this object obj) => Formatter.Format(obj);
    }
}
