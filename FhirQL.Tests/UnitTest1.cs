using System;
using Superpower;
using Xunit;

namespace FhirQL.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Select_Name_From_Patient()
        {
            var tokenizer = new SqlTokenizer();
            var tokens = tokenizer.Tokenize("select gender, birthDate from Patient");

            var result = SqlParser.SelectClause.Parse(tokens);

            var sql = FhirSqlFormatter.ToSql(result);

            Assert.Equal("SELECT JSON_VALUE(RawResourceData, '$.gender') AS [gender], JSON_VALUE(RawResourceData, '$.birthDate') AS [birthDate] FROM Resources WHERE ResourceTypeName= 'Patient' AND IsHistory= 0  ",
                sql);
        }

        [Fact]
        public void Select_Nested_From_Patient()
        {
            var tokenizer = new SqlTokenizer();
            var tokens = tokenizer.Tokenize("select name[0].family, name[0].given[0], gender, birthDate from Patient");

            var result = SqlParser.SelectClause.Parse(tokens);

            var sql = FhirSqlFormatter.ToSql(result);

            Assert.Equal("SELECT JSON_VALUE(RawResourceData, '$.name[0].family') AS [name0family], JSON_VALUE(RawResourceData, '$.name[0].given[0]') AS [name0given0], JSON_VALUE(RawResourceData, '$.gender') AS [gender], JSON_VALUE(RawResourceData, '$.birthDate') AS [birthdate] FROM Resources WHERE ResourceTypeName= 'Patient' AND IsHistory= 0  ",
                sql);
        }
    }
}
