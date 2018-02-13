# dbEnumerator  [![NuGet](https://img.shields.io/badge/nuget-0.9-blue.svg)](https://www.nuget.org/packages/dbEnumerator/)

## What is dbEnumerator?

dbEnumerator is a small set of utilities and guidance that may help you to manage catalogue tables with Entity Framework Core, based on C# enums in an easy way

### The sample

If we had an entity (Superhero) with following fields

|Field   | Type  |
|---|---|
| Id  | int  |
| Name | string  |
| Age | int  |
| ComicEditor | ComicEditor (enum: DC/Marvel)  |

may be we would like to have Comic editor modeled as a catalogue table in our database, so our database model looks like this:

![Database model](https://github.com/snavarropino/dbEnumerator/blob/master/docs/images/Superhero_Diagram1.png?raw=true)

In adition, we would like to have in our superhero entity with an enum type for the comic editor, not simply an int type for it. Let's see how to get it with dbEnumerator:

## Getting started

- Install dbEnumerator in your project

    You can install [dbEnumerator via NuGet](https://www.nuget.org/packages/dbEnumerator):

        Install-Package dbEnumerator

    Or via the .NET CoreCLI:

        dotnet add package dbEnumerator

    Both commands will download and install all required dependencies (Entity Framework Core).

- Create an enum type and an entity for the catalogue table based on the previous enum type

    ```C#
        public enum ComicEditor
        {
            Marvel = 1,
            Dc = 2
        }
        public class ComicEditorCatalogue : EnumBasedEntity<ComicEditor> { }
    ```
    EnumBase class provided by dbEnumerator will contail: Id, Name and Description

- Modify you Superhero class to
  - create a navigation property to catalogue entity
  - create a property, based on enum, to easily interact with this entity.  This enum based property will be ignored by entity framework
  - hold a private field that will be used for the relationship foreig key
    ```C#
    public class Superhero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public ComicEditor ComicEditor
        {
            get => (ComicEditor)_comicEditorId;
            set => _comicEditorId = (int)value;
        }

        private int _comicEditorId;
        public ComicEditorCatalogue ComicEditorCatalogue { get; set; }
    }
    ```

- Instruct you database context to properly manage relationship

    ```C#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EnumBasedEntityConfigurator<Superhero, ComicEditorCatalogue>
                .Register(modelBuilder,
                          superhero => superhero.ComicEditor,            //Property based on enum that is goint to be ignored by Entity Framework Core
                          "_comicEditorId",                              //Name of private field that is going to act as Backing field
                          superhero => superhero.ComicEditorCatalogue ); //Navigation property to catalogue table
    }
    ```

- Seed data for the catalogue entity. This is done automatically by dbEnumerator

    ```C#
    await EnumBasedEntitySeeder.SeedEntityAsync<ComicEditorCatalogue, ComicEditor>(context.ComicEditors);
    ```

- Use and enjoy!
    Once we have performed previous steps we are able to use superhero entity in a simple way:

    ```C#
    await context.AddAsync(new Superhero() {Name = "Mento", Age = 30, ComicEditor = ComicEditor.Dc});
    await context.SaveChangesAsync();

    var mento = await context.Superheros.FirstOrDefaultAsync();
    Console.WriteLine($"Readed {mento.Name}, avaliable in {mento.ComicEditor} comics");
    ```
    As you can see we are managing just an enum for the comic editor. Relationship is properly managed by Entity Framework Core

## FAQ

- What happens if I add a new value to my enum type?
EnumSeeder.SeedEnumDataAsync methos will add a new row to catalogue table next time is executed

- Can you explain how dbEnumerator works?
Sure, it is based on backing fields with no property (https://docs.microsoft.com/es-es/ef/core/modeling/backing-field), that allow us to have a foreign key created over a private field, that is not visible outside our entity.
In addition an enum property, connected to that private field, is exposed to callers, using an enum type.

- Can I contribute to dbEnumerator?
Sure, pull requests are more thn welcome!

## Backlog

Several improvents are going to be added to dbEnumerator in order to simplify usage and include advanced features. Please look here (https://github.com/snavarropino/dbEnumerator/milestone/1)

## Build status

| Platform                    | Status                                                                                                                                  |
|-----------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| VSTS (.NET Core) | [![Build status](https://ci.appveyor.com/api/projects/status/nxoyeq5r03tk6cpq/branch/master?svg=true)](https://ci.appveyor.com/project/lurumad/aspnetcore-health/branch/master) |
