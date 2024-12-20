﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polaby.Repositories.Entities;

namespace Polaby.Repositories
{
    public class AppDbContext : IdentityDbContext<Account, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<CommentLike> CommentLike { get; set; }
        public DbSet<CommunityPost> CommunityPost { get; set; }
        public DbSet<CommunityPostLike> CommunityPostLike { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<DishIngredient> DishIngredient { get; set; }
        public DbSet<Emotion> Emotion { get; set; }
        public DbSet<EmotionTypeMapping> EmotionTypeMapping { get; set; }
        public DbSet<NoteEmotion> NoteEmotion { get; set; }
        public DbSet<ExpertRegistration> ExpertRegistration { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Health> Health { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Meal> Meal { get; set; }
        public DbSet<MealDish> MealDish { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuMeal> MenuMeal { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationSetting> NotificationSetting { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<SafeFood> SafeFood { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<UserMenu> UserMenu { get; set; }
        public DbSet<WeeklyPost> WeeklyPost { get; set; }
        public DbSet<IngredientSearch> IngredientSearch { get; set; }
        public DbSet<SubscriptionForm> SubscriptionForm { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(x => x.FirstName).HasMaxLength(50);
                entity.Property(x => x.LastName).HasMaxLength(50);
                entity.Property(x => x.PhoneNumber).HasMaxLength(15);
                entity.Property(x => x.VerificationCode).HasMaxLength(6);
                entity.Property(x => x.BabyName).HasMaxLength(50);
            });

            modelBuilder.Entity<CommunityPost>(entity => { entity.Property(x => x.Title).HasMaxLength(256); });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(x => x.Name);
                entity.Property(x => x.Description);
            });

            modelBuilder.Entity<ExpertRegistration>(entity =>
            {
                entity.Property(x => x.FirstName).HasMaxLength(50);
                entity.Property(x => x.LastName).HasMaxLength(50);
                entity.Property(x => x.PhoneNumber).HasMaxLength(15);
                entity.Property(x => x.Email).HasMaxLength(156);
            });
            
            modelBuilder.Entity<SubscriptionForm>(entity =>
            {
                entity.Property(x => x.FullName).HasMaxLength(100);
                entity.Property(x => x.PhoneNumber).HasMaxLength(15);
                entity.Property(x => x.Email).HasMaxLength(156);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(150);
                entity.Property(x => x.Description);
            });

            modelBuilder.Entity<Meal>(entity => { entity.Property(x => x.Name).HasMaxLength(250); });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(250);
                entity.Property(x => x.Description);
            });

            modelBuilder.Entity<NotificationType>(entity => { entity.Property(x => x.Name).HasMaxLength(156); });

            modelBuilder.Entity<SafeFood>(entity => { entity.Property(x => x.Name).HasMaxLength(150); });

            modelBuilder.Entity<Role>(entity => { entity.Property(x => x.Description).HasMaxLength(256); });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasOne(f => f.Expert)
                    .WithMany(a => a.Follows)
                    .HasForeignKey(f => f.ExpertId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(f => f.Receiver)
                    .WithMany(a => a.Notifications)
                    .HasForeignKey(f => f.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(f => f.Sender)
                    .WithMany()
                    .HasForeignKey(f => f.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasOne(f => f.Expert)
                    .WithMany(a => a.Ratings)
                    .HasForeignKey(f => f.ExpertId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Emotion>(entity =>
            {
                entity.HasMany(e => e.EmotionTypes)
                      .WithOne(et => et.Emotion)
                      .HasForeignKey(et => et.EmotionId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasMany(e => e.Notes)
                      .WithOne(n => n.Emotion)
                      .HasForeignKey(n => n.EmotionId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<EmotionTypeMapping>(entity =>
            {
                entity.HasOne(et => et.Emotion)
                      .WithMany(e => e.EmotionTypes)
                      .HasForeignKey(et => et.EmotionId);
            });

            modelBuilder.Entity<NoteEmotion>(entity =>
            {
                entity.Property(n => n.IsSelected).IsRequired();
                entity.HasOne(n => n.Emotion)
                      .WithMany(e => e.Notes)
                      .HasForeignKey(n => n.EmotionId);
            });
        }
    }
}