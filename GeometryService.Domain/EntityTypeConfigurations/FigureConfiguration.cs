using GeometryService.Domain.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeometryService.Domain.EntityTypeConfigurations
{
    public class FigureConfiguration : IEntityTypeConfiguration<Figure>
    {
        public void Configure(EntityTypeBuilder<Figure> builder)
        {
            builder.HasIndex(x => x.Id);
        }
    }
}