using System.Globalization;

namespace City.Common.Utils
{
    public class FormattingTools
    {
        private static long B = 1000000000;
        private static long M = 1000000;
        private static long K = 1000;

        public static string FormatCoins(long amount, CoinFormat format)
        {
            switch (format)
            {
                case CoinFormat.Commas:
                    return FormatCoins(amount);
                case CoinFormat.B_M_K_C:
                    return FormatBMKC(amount);
                case CoinFormat.M_K_C:
                    return FormatMKC(amount);
                case CoinFormat.PlainOver10K:
                    return FormatPlainOver10K(amount);
                case CoinFormat.CommaOver10KWithMillionsShortHand:
                    return FormatCommaOver10KWithMillionsShortHand(amount);
                case CoinFormat.Plain:
                default:
                    break;
            }

            return FormatCoins(amount);
        }

        public static string FormatCommaOver10KWithMillionsShortHand(long amount)
        {
            if (amount >= M)
            {
                return ((double) amount / M).ToString("#.#M", CultureInfo.InvariantCulture);
            }

            return amount >= 10000 || amount == 0 ? FormatCoins(amount) : amount.ToString();
        }

        public static string FormatCoins(long amount)
        {
            return amount != 0 ? amount.ToString("#,#", CultureInfo.InvariantCulture) : amount.ToString();
        }

        public static string FormatMKC(long amount)
        {
            if (amount >= M)
            {
                return ((double) amount / M).ToString("#.#M", CultureInfo.InvariantCulture);
            }

            if (amount >= K)
            {
                return ((double) amount / K).ToString("#.#K", CultureInfo.InvariantCulture);
            }

            return amount.ToString("#,#", CultureInfo.InvariantCulture);
        }

        public static string FormatBMKC(long amount)
        {
            if (amount >= B)
            {
                return ((double) amount / B).ToString("#.#B", CultureInfo.InvariantCulture);
            }

            return FormatMKC(amount);
        }

        public static string FormatPlainOver10K(long amount)
        {
            return amount >= 10000 || amount == 0 ? FormatCoins(amount) : amount.ToString();
        }

        public static string IntToOrdinal(int num)
        {
            return num.ToString() + Ordinal(num);
        }
        
        public static string Ordinal(int num)
        {
            if (num <= 0) return "";

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return "th";
            }

            switch (num % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }
    }

    public enum CoinFormat
    {
        Plain,
        M_K_C,
        PlainOver10K,
        CommaOver10KWithMillionsShortHand,
        Commas,
        B_M_K_C
    }
}