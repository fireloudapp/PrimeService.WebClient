﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FireCloud.WebClient.PrimeService.Service.QueryString;
using MongoDB.Bson;
using PrimeService.Model;
using MudBlazor;
using PrimeService.Utility.Helper;

namespace FireCloud.WebClient.PrimeService.Pages.Auth;

public partial class Login
{
    bool PasswordVisibility;
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
    private Model model = new Model();
    private string error;
    private bool _processing = false;
    /// <summary>
    /// Current Domain URL 
    /// </summary>
    private string _domainURL = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _log.LogInformation("Info Logging");
        _log.LogWarning("Warning Logging");
        _log.LogTrace("Trace Logging");
        _domainURL = _navigationManager.BaseUri;
        User userInfo = await _localStore.GetItemAsync<User>("user");
        
        if (userInfo != null)
        {
            //User Existing Login. User already logged in.
            NavigationManager.NavigateTo("Index");
        }
        
    }

    private class Model
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }

    private async Task LoginInvoke()
    {
        _processing = true;
        try
        {
            User request = new User()
            {
                Username = model.Username,
                Password = model.Password,
                DomainURL = NavigationManager.BaseUri,
                UserType = UserCategory.FcUser.ToString(), //As JSON is not able to detect this enum value.
            };
            User user = await AuthenticationService.Login(request);
            Utilities.ConsoleMessage($"Login User : {user.ToJson()} ");
            if (user.IsSuccess)
            {
                var returnUrl = NavigationManager.QueryString("returnUrl") ?? "";
                await _JsRuntime.InvokeAsync<bool>("setLogRocketUser",null);
                NavigationManager.NavigateTo($"/index?{returnUrl}");
                Snackbar.Add($"Welcome {user.Username}!", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Failed to Login.!<br/>{user.Message} ", Severity.Error);    
                return;
            }
        }
        catch (Exception ex)
        {
            error = ex.Message;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
            Snackbar.Add(ex.Message, Severity.Error);
            StateHasChanged();            
        }
        finally
        {
            _processing = false;
        }        
    }

    private Task LoginGoogle()
    {
        //Console.WriteLine(_configuration["App:GoogleAuth"]);
        NavigationManager.NavigateTo(_appSettings.App.GoogleAuth + _domainURL);
        Utilities.ConsoleMessage(_domainURL);
        return Task.CompletedTask;
    }

    void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
}