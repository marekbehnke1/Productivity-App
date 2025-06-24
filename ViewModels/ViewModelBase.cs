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

        2. Now looks like we are moving away from computed properties - as these maybe caused the bug where you lose focus from 
            the text fields every time you enter a character.

        3. Lots of trickery was done to get the properties to change correctly.
            Will have to review what we did properly and get notes on it.
            Essentially it revolved around changing the setter method for the properties, and making the item an observable object.

         */

    }
}
