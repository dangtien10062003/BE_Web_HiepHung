namespace MyHiep.Api.Services;

public class DistanceService : IDistanceService
{
    public double CalculateKm(double fromLat, double fromLng, double toLat, double toLng)
    {
        const double earthRadiusKm = 6371;
        var dLat = ToRadians(toLat - fromLat);
        var dLng = ToRadians(toLng - fromLng);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(fromLat)) * Math.Cos(ToRadians(toLat)) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }

    private static double ToRadians(double value) => value * Math.PI / 180;
}
