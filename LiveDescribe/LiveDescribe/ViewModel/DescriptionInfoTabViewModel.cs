﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveDescribe.Model;

namespace LiveDescribe.ViewModel
{
    public class DescriptionInfoTabViewModel : ViewModelBase
    {
        #region Constants
        private const int SpaceTab = 0;
        private const int RegularDescriptionTab = 1;
        private const int ExtendedDescriptionTab = 2;
        #endregion

        #region Instance Variables
        private readonly DescriptionViewModel _descriptionViewModel;
        private readonly SpacesViewModel _spacesViewModel;
        private Description _selectedRegularDescription;
        private Description _selecetedExtendedDescription;
        private Space _selectedSpace;
        private String _descriptionAndSpaceText;
        private int _tabSelectedIndex;
        #endregion

        public DescriptionInfoTabViewModel(DescriptionViewModel descriptionViewModel, SpacesViewModel spaceViewModel)
        {
            _descriptionViewModel = descriptionViewModel;
            _spacesViewModel = spaceViewModel;

            SaveDescriptionTextCommand = new RelayCommand(SaveDescriptionText, SaveDescriptionTextStateCheck);
            ClearDescriptionTextCommand = new RelayCommand(ClearDescriptionText, () => true);

            SelectedRegularDescription = null;
            SelectedExtendedDescription = null;
            SelectedSpace = null;
            DescriptionAndSpaceText = null;
        }

        #region Commands

        public RelayCommand SaveDescriptionTextCommand { private set; get; }

        public RelayCommand ClearDescriptionTextCommand { private set; get; }

        #endregion

        #region Binding Properties
        /// <summary>
        /// Sets or gets the regular description selected in the tab control
        /// </summary>
        public Description SelectedRegularDescription
        {
            set
            {
                if (_selectedRegularDescription != null && value == null)
                {
                    _selectedRegularDescription.IsSelected = false;
                    _selectedRegularDescription.PropertyChanged -= DescriptionFinishedPlaying;
                }

                // Unselect previous descriptions and spaces selected
                if (_selectedRegularDescription != null)
                {
                    _selectedRegularDescription.IsSelected = false;
                    _selectedRegularDescription = null;
                }

                if (SelectedExtendedDescription != null)
                {
                    SelectedExtendedDescription.IsSelected = false;
                    SelectedExtendedDescription = null;
                }

                if (SelectedSpace != null)
                {
                    SelectedSpace.IsSelected = false;
                    SelectedSpace = null;
                }

                _selectedRegularDescription = value;

                if (_selectedRegularDescription != null)
                {
                    _selectedRegularDescription.IsSelected = true;
                    _selectedRegularDescription.PropertyChanged += DescriptionFinishedPlaying;
                }

                RaisePropertyChanged();
            }
            get
            {
                return _selectedRegularDescription;
            }
        }

        /// <summary>
        /// Sets or gets the extended description selected in the tab control
        /// </summary>
        public Description SelectedExtendedDescription
        {
            set
            {

                if (_selecetedExtendedDescription != null && value == null)
                {
                    _selecetedExtendedDescription.IsSelected = false;
                    _selecetedExtendedDescription.PropertyChanged -= DescriptionFinishedPlaying;
                }

                // Unselect previous descriptions and spaces selected
                if (SelectedRegularDescription != null)
                {
                    SelectedRegularDescription.IsSelected = false;
                    SelectedRegularDescription = null;
                }

                if (_selecetedExtendedDescription != null)
                {
                    _selecetedExtendedDescription.IsSelected = false;
                    _selecetedExtendedDescription = null;
                }

                if (SelectedSpace != null)
                {
                    SelectedSpace.IsSelected = false;
                    SelectedSpace = null;
                }

                _selecetedExtendedDescription = value;

                if (_selecetedExtendedDescription != null)
                {
                    _selecetedExtendedDescription.IsSelected = true;
                    _selecetedExtendedDescription.PropertyChanged += DescriptionFinishedPlaying;
                }

                RaisePropertyChanged();
            }
            get
            {
                return _selecetedExtendedDescription;
            }
        }

        /// <summary>
        /// Sets or gets the space selected in the tab control
        /// </summary>
        public Space SelectedSpace
        {
            set
            {
                if (_selectedSpace != null && value == null)
                    _selectedSpace.IsSelected = false;


                // Unselect previous descriptions and spaces selected
                if (SelectedRegularDescription != null)
                {
                    SelectedRegularDescription.IsSelected = false;
                    SelectedRegularDescription = null;
                }

                if (SelectedExtendedDescription != null)
                {
                    SelectedExtendedDescription.IsSelected = false;
                    SelectedExtendedDescription = null;
                }

                if (_selectedSpace != null)
                {
                    _selectedSpace.IsSelected = false;
                    _selectedSpace = null;
                }

                _selectedSpace = value;

                if (_selectedSpace != null)
                    _selectedSpace.IsSelected = true;
                RaisePropertyChanged();
            }
            get
            {
                return _selectedSpace;
            }
        }

        /// <summary>
        /// The index of the current tab selected in the tab control
        /// </summary>
        public int TabSelectedIndex
        {
            set
            {
                _tabSelectedIndex = value;
                RaisePropertyChanged();
            }
            get
            {
                return _tabSelectedIndex;
            }
        }

        /// <summary>
        /// The description text to be saved to the selected description
        /// </summary>
        public string DescriptionAndSpaceText
        {
            set
            {
                _descriptionAndSpaceText = value;
                RaisePropertyChanged();
            }
            get
            {
                return _descriptionAndSpaceText;
            }
        }

        /// <summary>
        /// Returns a list of all the extended descriptions to be viewed in the list view inside the
        /// tab control for extended descriptions
        /// </summary>
        public ObservableCollection<Description> ExtendedDescriptions
        {
            get
            {
                return _descriptionViewModel.ExtendedDescriptions;
            }
        }

        /// <summary>
        /// Returns a list of all the regular descriptions to be viewed in the list view inside the
        /// tab control for regular descriptions
        /// </summary>
        public ObservableCollection<Description> RegularDescriptions
        {
            get
            {
                return _descriptionViewModel.RegularDescriptions;
            }
        }

        /// <summary>
        /// returns a list of all the spaces to be viewed in the tab control
        /// </summary>
        public ObservableCollection<Space> Spaces
        {
            get
            {
                return _spacesViewModel.Spaces;
            }
        }

        #endregion

        #region Binding Functions

        /// <summary>
        /// Clears the description text
        /// </summary>
        public void ClearDescriptionText()
        {
            DescriptionAndSpaceText = "";
        }
        /// <summary>
        /// Depending on which tab is selected it will overwrite the appropriate description text
        /// </summary>
        public void SaveDescriptionText()
        {
            if (TabSelectedIndex == RegularDescriptionTab)
                SelectedRegularDescription.DescriptionText = DescriptionAndSpaceText;
            else if (TabSelectedIndex == ExtendedDescriptionTab)
                SelectedExtendedDescription.DescriptionText = DescriptionAndSpaceText;
            else if (TabSelectedIndex == SpaceTab)
                SelectedSpace.SpaceText = DescriptionAndSpaceText;

        }

        public void DescriptionFinishedPlaying(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals("IsPlaying"))
            {
                var description = (Description) sender;
                if (!description.IsPlaying)
                {
                    description.IsSelected = false;
                    UnSelectDescriptionsAndSpaceSelectedInList();
                }
            }
        }
        #endregion

        #region State Checks
        /// <summary>
        /// Returns the state of whether the save text button can be shown or not
        /// </summary>
        /// <returns></returns>
        public bool SaveDescriptionTextStateCheck()
        {
            if (SelectedExtendedDescription != null && TabSelectedIndex == ExtendedDescriptionTab)
                return true;
            if (SelectedRegularDescription != null && TabSelectedIndex == RegularDescriptionTab)
                return true;
            if (SelectedSpace != null && TabSelectedIndex == SpaceTab)
                return true;

            return false;
        }
        #endregion

        #region Helper Functions

        public void UnSelectDescriptionsAndSpaceSelectedInList()
        {
            SelectedRegularDescription = null;
            SelectedExtendedDescription = null;
            SelectedSpace = null;
        }
        #endregion
    }
}
