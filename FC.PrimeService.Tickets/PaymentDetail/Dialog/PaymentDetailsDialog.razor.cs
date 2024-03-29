﻿using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PrimeService.Model;
using PrimeService.Model.Settings;
using PrimeService.Model.Settings.Payments;
using Model = PrimeService.Model.Tickets;

namespace FC.PrimeService.Tickets.PaymentDetail.Dialog;

public partial class PaymentDetailsDialog
{
    #region Global Variables
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    private bool _loading = false;
    private string _title = string.Empty;
    [Parameter] public Model.PaymentDetails _PaymentDetails { get; set; } 
    private bool _processing = false;
    MudForm form;
    private Model.PaymentDetails _inputMode;
    string _outputJson;
    bool success;
    string[] errors = { };
    private bool _isReadOnly = false;
    #endregion

    #region Load Async
    protected override async Task OnInitializedAsync()
    {
        if (_PaymentDetails == null)
        {
            //Dialog box opened in "Add" mode
            //No Add mode.
            _inputMode = new Model.PaymentDetails();
            _title = "Add Payments";
        }
        else
        {
            //Dialog box opened in "Edit" mode
            _inputMode = _PaymentDetails;
            _title = "Edit Payments";
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
    private async Task GetFakeData()
    {
        throw new NotImplementedException();
    } 
    #endregion
    
    #region PaymentTags Search - Autocomplete
    
    public List<PaymentTags> _paymentTags = new List<PaymentTags>()
    {
        new PaymentTags(){ Title = "Primary Account"},
        new PaymentTags(){ Title = "Secondary Account"},
        new PaymentTags(){ Title = "Expense Account"},
        new PaymentTags(){ Title = "Income Account"},
    };

    private async Task<IEnumerable<PaymentTags>> PaymentAccount_SearchAsync(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);
        Console.WriteLine($"Find Account : '{value}'");
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
        {
            return _paymentTags;
        }

        var result = _paymentTags.Where(x => x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        if (result.FirstOrDefault() == null)
        {
            result = new List<PaymentTags>()
            {
                new PaymentTags() { Title = "Not Found"}
            };
        }

        return result;
    }

    #endregion
    
    #region PaymentTags Search - Autocomplete
    
    public List<PaymentMethods> _paymentMethods = new List<PaymentMethods>()
    {
        new PaymentMethods(){ Title = "Card"},
        new PaymentMethods(){ Title = "Cash"},
        new PaymentMethods(){ Title = "QR"},
    };

    private async Task<IEnumerable<PaymentMethods>> PaymentMethods_SearchAsync(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);
        Console.WriteLine($"Find Method : '{value}'");
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
        {
            return _paymentMethods;
        }

        var result = _paymentMethods.Where(x => x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        if (result.FirstOrDefault() == null)
        {
            result = new List<PaymentMethods>()
            {
                new PaymentMethods() { Title = "Not Found"}
            };
        }

        return result;
    }

    #endregion
}