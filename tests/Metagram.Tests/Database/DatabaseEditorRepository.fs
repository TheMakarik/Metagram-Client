module Metagram.Tests.Database.DatabaseEditorRepository

open Metagram.Models.DataAccess
open Microsoft.EntityFrameworkCore
open Xunit
open System

let options = DbContextOptionsBuilder<MetagramDbContext>()
                  .UseInMemoryDatabase("TestDB")

[<Fact>]
let GetTables_MustReturnsTableNames() = task {
    
    //Arrange
    
    
    use context = new MetagramDbContext(options.Options)
    let! context.Database.EnsureCreatedAsync()
    
    //Act
    
    //Assert
}
