﻿using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PrimeService.Model.Settings.Payments;
using PrimeService.Model.Settings.Tickets;

namespace FC.PrimeService.Common.Settings.Dialog;

public partial class PaymentTagDialog
{
        #region Global Variables
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    private bool _loading = false;
    private string _title = string.Empty;
    [Parameter] public PaymentTags _PaymentTags { get; set; } 
    private bool _processing = false;
    MudForm form;
    private PaymentTags _inputMode;
    string _outputJson;
    bool success;
    string[] errors = { };
    private bool _isReadOnly = false;
    #endregion

    #region Load Async
    protected override async Task OnInitializedAsync()
    {
        if (_PaymentTags == null)
        {
            //Dialog box opened in "Add" mode
            _inputMode = new PaymentTags()
            {
                Title = "Main Account",
                InitialFund = 10000,
                Category = PaymentCategory.Income
            };
            _title = "Add Payment Tag";
        }
        else
        {
            //Dialog box opened in "Edit" mode
            _inputMode = _PaymentTags;
            _title = "Edit Payment Tag";
        }
    }
    #endregion
    
    #region Cancel & Close
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    #endregion

    #region Submit Button with Animation
    async Task ProcessSomething()
    {
        _processing = true;
        await Task.Delay(2000);
        _processing = false;
    }

    private async Task Submit()
    {
        await form.Validate();

        if (form.IsValid)
        {
            // //Todo some animation.
            await ProcessSomething();

            //Do server actions.
            _outputJson = JsonSerializer.Serialize(_inputMode);

            //Success Message
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.SnackbarVariant = Variant.Filled;
            //Snackbar.Configuration.VisibleStateDuration  = 2000;
            //Can also be done as global configuration. Ref:
            //https://mudblazor.com/components/snackbar#7f855ced-a24b-4d17-87fc-caf9396096a5
            Snackbar.Add("Submitted!", Severity.Success);
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