﻿using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PrimeService.Model.Settings;
using PrimeService.Model.Settings.Tickets;

namespace FC.PrimeService.Common.Settings.Dialog;

public partial class TaxDialog
{
    
    #region Global Variables
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    private bool _loading = false;
    private string _title = string.Empty;
    [Parameter] public Tax _Tax { get; set; } 
    private bool _processing = false;
    MudForm form;
    private Tax _inputMode;
    string _outputJson;
    bool success;
    string[] errors = { };
    private bool _isReadOnly = false;

    
    #endregion

    #region Load Async
    protected override async Task OnInitializedAsync()
    {
        if (_Tax == null)
        {
            //Dialog box opened in "Add" mode
            _inputMode = new Tax()
            {
                Title = "0.00% - No Tax",
                TaxRate = 0.0f,
                Description = "No tax applied for the goods."
            };
            _title = "Add Tax";
        }
        else
        {
            //Dialog box opened in "Edit" mode
            _inputMode = _Tax;
            _title = "Edit Tax";
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