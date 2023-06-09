﻿namespace Domain.Entities
{
    public class AnimalVisitedLocation
    {
        public long Id { get; set; }
        public long LocationPointId { get; set; }
        public long AnimalId { get; set; }
        public DateTimeOffset DateTimeOfVisitLocationPoint { get; set; }
        public LocationPoint LocationPoint { get; set; } = null!;
        public Animal Animal { get; set; }
    }
}