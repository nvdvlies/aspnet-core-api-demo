using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence.Configuration;

public static class SequenceConfiguration
{
    internal static void ConfigureSequences(this ModelBuilder builder)
    {
        builder.HasSequence<int>(Sequences.CustomerCode)
            .StartsAt(1)
            .HasMin(1)
            .HasMax(9999999)
            .IsCyclic(false)
            .IncrementsBy(1);

        builder.HasSequence<int>(Sequences.InvoiceNumber)
            .StartsAt(100000)
            .HasMin(100000)
            .HasMax(999999)
            .IsCyclic()
            .IncrementsBy(1);
    }
}
