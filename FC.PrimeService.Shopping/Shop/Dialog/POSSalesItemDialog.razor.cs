﻿using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PrimeService.Model.Settings.Forms;
using Model = PrimeService.Model.Shopping;

namespace FC.PrimeService.Shopping.Shop.Dialog;

public partial class POSSalesItemDialog
{
   #region Global Variables
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    private bool _loading = false;
    private string _title = string.Empty;
    [Parameter] public Model.PurchasedProduct _PurchasedProduct { get; set; } 
    private bool _processing = false;
    MudForm form;
    private Model.PurchasedProduct _inputMode;
    string _outputJson;
    bool success;
    string[] errors = { };
    private bool _isReadOnly = false;
    #endregion

    #region Load Async
    protected override async Task OnInitializedAsync()
    {
        if (_PurchasedProduct == null)
        {
            //Dialog box opened in "Add" mode
            //No Add mode.
        }
        else
        {
            //Dialog box opened in "Edit" mode
            _inputMode = _PurchasedProduct;
            _title = "Edit Product";
        }
    }
    #endregion
    
    #region Cancel & Close
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    #endregion

    #region Quantity Validation

    private string ValidateQuantity(int qty)
    {
        // if (_inputMode.Quantity == 0)
        // {
        //     _inputMode.Discount = 0;
        //     _inputMode.DiscountPrice = 0;
        //     _inputMode.Price = 0;
        //     _inputMode.SubTotal = 0;
        // }

        return null;
    }
    #endregion
    
    #region Submit Button with Animation
    async Task ProcessQuantity()
    {
        _processing = true;
        
        _processing = false;
    }

    private async Task Submit()
    {
        await form.Validate();

        if (form.IsValid)
        {
            // //Todo some animation.
            await ProcessQuantity();

            //Do server actions.
            _outputJson = JsonSerializer.Serialize(_inputMode);

            //Success Message
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.SnackbarVariant = Variant.Filled;
            //Snackbar.Configuration.VisibleStateDuration  = 2000;
            //Can also be done as global configuration. Ref:
            //https://mudblazor.com/components/snackbar#7f855ced-a24b-4d17-87fc-caf9396096a5
            Snackbar.Add("Submitted!", Severity.Success);
            MudDialog.Close();
        }
        else
        {
            _outputJson = "Validation Error occured.";
            Console.WriteLine(_outputJson);
        }
    }

    #endregion

    #region Generate Fake
    private Task GetFakeData()
    {
        throw new NotImplementedException();
    } 
    #endregion
}