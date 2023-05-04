using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Repository.Configurations
{
    public class MovieRatingConfiguration : IEntityTypeConfiguration<MovieRating>
    {
        public void Configure(EntityTypeBuilder<MovieRating> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Comment).HasMaxLength(1000);
            //builder.Property(x => x.Rating >= 1 && x.Rating <= 10);
            builder.Property(x => x.MovieId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

        }
    }
}
