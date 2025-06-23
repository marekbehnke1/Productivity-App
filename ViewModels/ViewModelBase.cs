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

    }
}
