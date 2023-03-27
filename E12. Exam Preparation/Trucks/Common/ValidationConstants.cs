// ReSharper disable InconsistentNaming
namespace Trucks.Common
{
    public static class ValidationConstants
    {
        // Truck
        public const int TruckRegistrationNumberLength = 8;
        public const int TruckVinNumberLength = 17;
        public const string TruckRegistrationNumberRegEx =
            @"[A-Z]{2}\d{4}[A-Z]{2}";
        public const int TruckTankCapacityMinValue = 950;
        public const int TruckTankCapacityMaxValue = 1420;
        public const int TruckCargoCapacityMinValue = 5000;
        public const int TruckCargoCapacityMaxValue = 29000;
        public const int TruckCategoryTypeMinValue = 0;
        public const int TruckCategoryTypeMaxValue = 3;
        public const int TruckMakeTypeMinValue = 0;
        public const int TruckMakeTypeMaxValue = 4;

        // Client
        public const int ClientNameMinLength = 3;
        public const int ClientNameMaxLength = 40;
        public const int ClientNationalityMinLength = 2;
        public const int ClientNationalityMaxLength = 40;

        // Despatcher
        public const int DespatcherNameMinLength = 2;
        public const int DespatcherNameMaxLength = 40;
    }
}
