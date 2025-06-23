using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CLDV6211_EventEase_POE.Models;

namespace CLDV6211_EventEase_POE.Data
{
    public class CLDV6211_EventEase_POEContext : DbContext
    {
        public CLDV6211_EventEase_POEContext(DbContextOptions<CLDV6211_EventEase_POEContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Booking { get; set; } = default!;
        public DbSet<Event> Event { get; set; } = default!;
        public DbSet<Venue> Venue { get; set; } = default!;
        public DbSet<EventType> EventType { get; set; } = default!;
        public DbSet<eventease> eventeases { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Booking → Venue
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany()
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            // Event → EventType
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Event)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seeding EventType data
            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeId = 1, Name = "Wedding" },
                new EventType { EventTypeId = 2, Name = "Concert" },
                new EventType { EventTypeId = 3, Name = "Birthday" },
                new EventType { EventTypeId = 4, Name = "Corporate" }
            );
        }
    }
}
