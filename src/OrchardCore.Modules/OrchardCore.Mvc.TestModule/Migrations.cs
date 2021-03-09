using OrchardCore.Data.Migration;
using TestModule.Indexes;
using YesSql.Sql;

namespace TestModule
{
    public class Migrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<PersonByAge>(_ => _
                .Column<int>(nameof(PersonByAge.Age))
                .Column<bool>(nameof(PersonByAge.Adult))
                .Column<string>(nameof(PersonByAge.Name))
            );

            SchemaBuilder.CreateMapIndexTable<AnotherPersonByAge>(_ => _
                .Column<int>(nameof(AnotherPersonByAge.Age))
                .Column<bool>(nameof(AnotherPersonByAge.Adult))
                .Column<string>(nameof(AnotherPersonByAge.Name))
                .Column<string>(nameof(AnotherPersonByAge.AdditionalField1))
                .Column<string>(nameof(AnotherPersonByAge.AdditionalField2))
                .Column<string>(nameof(AnotherPersonByAge.AdditionalField3))
            );

            return 1;
        }
    }
}
