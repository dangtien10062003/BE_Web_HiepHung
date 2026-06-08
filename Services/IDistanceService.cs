namespace MyHiep.Api.Services;

public interface IDistanceService
{
    double CalculateKm(double fromLat, double fromLng, double toLat, double toLng);
}
