using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnAvalonia.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        //////////////////////////////////
        // Displaying data from backend //
        //////////////////////////////////
        // essentially using viewmodelbase that derives from observable object adds a shit load of functionality for free.
        // It just includes all of the property notification and updating automatically without having to use INotifyPropertyChanged etc
        // Having an empty class that all your other viewmodels derive from, is a good way to give you space for future extensibility
        // You will be able to add features to the viewmodel base that will then in turn be added to all of your viewmodels
        // Its the fact that it derives from observable object that gives you all the property features for free.

        // This is just where im adding more notes for now:
        // You need to connect the models namespace to your xaml.
        // To generate xaml elements from an observable collection, you use an ItemsControl
        // you then bind the itemsource property to your collection
        // INside the itemscontrol you can specify a template container for your items using <Itemscontrol.itemspanel>
        // You then give it an <Itemspaneltemplate>
        // and inside this you specify what container you want your items to be stored within
        //
        // Then we have to specify a template for the items themselves
        // with itemscontrol.itemtemplate
        // The itemscontrol then generates ui elements for each item in the collection you have passed to it as a binding.
        // You also need to specify a data template for the item - which is your Model for the item.
        // This means you have specified a ui template and data template for the item

        /*
         -------------------------------------------
         Updating Properties across different views
         -------------------------------------------
         1. In future when you make a User control - its a good idea to create the data model for it at the same time
            This can then be set as the datacontext for that item early on, and you can use normal bindings
            This saves a lot of faff with using relativesource and setting the item as its own data source etc
            Helps massively when controlling the items content from the ViewModel later down the road
            
            Sometimes you then also need to be very specific about describing where the model for that item is.
            xmlns:models="clr-namespace:LearnAvalonia.Models;assembly=LearnAvalonia"
			x:DataType="models:TaskItem"

        2.  Make TaskItem Observable
                TaskItem becomes a partial class that inherits from Observable Object
                This allows you to use set => SetProperty(ref _title, value);
                This fires a property changed event whenever this property is set.
                This allows other objects to listen for this event and then do something accordingly

        3.  Moved to a "Single Source of truth" architecture
                The only place taskitems exist is in the main tasks list.
                All other lists are created as filtered versions of this, which allows updates to be managed easily as data only exists in one place
                This will help in the future with potential sync issues. As we only ever need to manage the one list being synced.
                Other lists were created with computed properties from this main tasks list.
                At the end of the data all data only exists in one place, everything else is just a reference back to the orignal source.
        
        4.  Property Change Subscription system
                In the constructor for MainViewModel, when we create the tasks list, we add property change listeners to all tasks
                This allows the viewmodel to know whenever a property on any item changes

        5.  Automatic Subscription for new items
                When a new item is created, it is added to this network of property listeners automatically
                This allows any new items to be automatically kept track of

        6.  Changed UI binding method
                Changed the ui elements to bind to the data model instead of to itself.
                TwoWay bindings are important here, so that the data can flow in both directions, UI -> viewmodel - > model && model -> viewmodel -> UI


                    The Magic Flow
            Here's what happens when you edit a task title:

            User types in TextBox → UI binding triggers
            TaskItem.Title setter called → SetProperty() fires PropertyChanged
            Property change contains "Title" → OnTaskItemPropertyChanged() receives it
            Since it's not "TaskPriority" → No collection refresh needed
            But other UIs are bound to same TaskItem → They automatically update via data binding!

            When you change priority:

            User selects new priority → TaskItem.TaskPriority changes
            PropertyChanged fires with "TaskPriority" → OnTaskItemPropertyChanged() catches it
            RefreshFilteredCollections() called → All filtered views regenerate
            Task appears in new list, disappears from old → Automatic migration!

            Why This Architecture Works So Well
            🔄 Automatic Synchronization: All views stay in sync because they're all looking at the same underlying data
            ⚡ Efficient Updates: Only refreshes when necessary (priority changes), not on every keystroke
            🎯 Single Source of Truth: Tasks collection is the only real data store - everything else is computed
            🔌 Self-Managing: New tasks automatically get plugged into the system without manual wiring
            🏗️ Scalable: Easy to add new filtered views or properties without breaking existing functionality
            This is a textbook implementation of the Observer pattern combined with MVVM architecture - exactly how professional apps handle complex data synchronization!

                


        XX. Had a bug where you would lose focus every time you entered a character.
            This is because the UI was rebuilding every time there was a property change
            Changed this so that the we only rebuild the UI when a task changes priority - which is the only thing that would cause the UI
            to need to change anyways.



        ---------------
        Using EF Core
        ---------------

        EF Core is an ORM for C# & .NET
        Just like SQL Alchemy etc for Python

        It basically translates your C# objects into tables and vica versa.
        This allows you to just write in C# and let EF Core handle the translation into SQL.

        SQlite is a super lightweight sigle file database, and therefore is perfect for local storage with apps.
        It's also easy to migrate from SQLite to cloud databases later on

        To work with your database you need a DbContext - this is a database connection manager.
        This class essentially coordinates all database operations
        It updates tables, handles the connections and tracks any changes

        Db Context & DbSet
        --------------------
        DbContext acts as a bridge between your C# Classes and the database
        Db Set represents a table in the database, with each task become a row in that table
            EF Core automatically creates the table columns based on the properties of the task object
            In this way, your data model becomes the blueprint for your database structure

        Connecting
        --------------
        An override of OnConfiguring is used to tell EF core how to connect to the database.

        You can set the path you want your databse to exist in and save it in a variable.
        Things like Environment.SpecialFolder.LocalApplicationData can be useful for returning the path of a local appdata folder.
        This is often where applications store data.

        It is good to then check this generated path actually exists.

            var directory = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(directory) && directory != null)
            {
                Directory.CreateDirectory(directory);
            }

        This will check that the path is valid, and if the directory does not exist, then create it
        
        Once the checks are valid you can then configure the SQL connection to use that path.
            optionsBuilder.UseSqlite($"Data Source={dbPath}");


        Mapping classes to the db
        -------------------------
        An override of OnModelCreating is used to define how your classes will map to the database tables

        This is used to map the properties on TaskItem to the database.
        You need to add an id property to the object you are mapping to the database.
        This gets set when the object is added to the db, and allows to db to track which object is which.

        modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(t => t.Description)
                    .HasMaxLength(1000);



        Creating the methods to interact with the DB
        ---------------------------------------------

        You need to define the ways in which your program will interact with the database.
        An interface is perfect for this. Because any class which implemenents that interface, must provide all
        of the methods required to interact with the database.
        It also allows for easy testing, as you can create fake data which targets the interface methods.
        You are essentially defining all the things that the TaskService needs to do.

        You then define the actual TaskService class which will interact directly with the database.

         */

    }
}
