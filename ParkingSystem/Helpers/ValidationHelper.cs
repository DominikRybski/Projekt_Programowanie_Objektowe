using System.Text.RegularExpressions;

namespace ParkingSystem.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Waliduje numer rejestracyjny pojazdu.
        /// Format: 2-3 litery, 2-5 cyfr/liter (np. KR123, WA987, GD12345)
        /// </summary>
        public static bool WalidujNumerRejestracyjny(string nrRejestracyjny, out string? bladWalidacji)
        {
            bladWalidacji = null;

            if (string.IsNullOrWhiteSpace(nrRejestracyjny))
            {
                bladWalidacji = "Numer rejestracyjny nie może być pusty.";
                return false;
            }

            if (nrRejestracyjny.Length < 4 || nrRejestracyjny.Length > 8)
            {
                bladWalidacji = "Numer rejestracyjny musi mieć od 4 do 8 znaków.";
                return false;
            }

            // Regex: 2-3 litery na początku + 2-5 cyfr/liter
            var regex = new Regex(@"^[A-Z]{2,3}[A-Z0-9]{2,5}$", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(nrRejestracyjny))
            {
                bladWalidacji = "Numer rejestracyjny ma nieprawidłowy format (oczekiwany: np. KR123, WA987).";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Waliduje współrzędne parkingu
        /// </summary>
        public static bool WalidujWspolrzedne(int wiersz, int kolumna, int maxWierszy, int maxKolumn, out string? bladWalidacji)
        {
            bladWalidacji = null;

            if (wiersz < 0 || wiersz >= maxWierszy)
            {
                bladWalidacji = $"Wiersz {wiersz} jest poza zakresem (0-{maxWierszy - 1}).";
                return false;
            }

            if (kolumna < 0 || kolumna >= maxKolumn)
            {
                bladWalidacji = $"Kolumna {kolumna} jest poza zakresem (0-{maxKolumn - 1}).";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Waliduje typ operacji transakcji
        /// </summary>
        public static bool WalidujTypOperacji(string typOperacji, out string? bladWalidacji)
        {
            bladWalidacji = null;

            if (string.IsNullOrWhiteSpace(typOperacji))
            {
                bladWalidacji = "Typ operacji nie może być pusty.";
                return false;
            }

            var dozwoloneTypy = new[] { "Przyjazd", "Odjazd" };
            if (!dozwoloneTypy.Contains(typOperacji, StringComparer.OrdinalIgnoreCase))
            {
                bladWalidacji = $"Typ operacji '{typOperacji}' jest niedozwolony. Dozwolone: {string.Join(", ", dozwoloneTypy)}.";
                return false;
            }

            return true;
        }
    }
}
