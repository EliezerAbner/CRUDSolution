using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                CREATE PROCEDURE InsertPerson 
                (
                    IN _PersonId CHAR(36),
                    IN _PersonName VARCHAR(40),
                    IN _Email VARCHAR(40),
                    IN _DateOfBirth DATETIME,
                    IN _Gender VARCHAR(10),
                    IN _CountryId CHAR(36),    
                    IN _Address VARCHAR(200),  
                    IN _ReceiveNewsLetters TINYINT(1)
                )
                BEGIN
                    INSERT INTO Persons (`PersonId`,`PersonName`,`Email`,`DateOfBirth`,`Gender`,`CountryId`,`Address`,`ReceiveNewsLetters`)
                    VALUES (_PersonId, _PersonName, _Email, _DateOfBirth, _Gender, _CountryId, _Address, _ReceiveNewsLetters);
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                DROP PROCEDURE InsertPerson
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}


