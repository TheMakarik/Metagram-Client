
//Community Toolkit
global using CommunityToolkit.Mvvm.ComponentModel;

//Microsoft.Extensions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

//.NET
global using System.Windows;
global using System.Data;
global using System.Diagnostics;
global using System.Windows.Controls;
global using System.Windows.Input;
global using System.Windows.Media;
global using System.Windows.Threading;

//Project usings
global using Metagram.Models.Options;
global using Metagram.Views;
global using Metagram.Services.AppDataServices.Abstractions;
global using Metagram.Factories.Abstractions;
global using Metagram.Factories;
global using Metagram.Services.AppDataServices;
global using Metagram.Models.Entities;
global using Metagram.Models.Repositories.Abstractions;
global using Metagram.Services.PollingServices;
global using Metagram.Services;
global using Metagram.Services.ViewServices;
global using Metagram.ViewModels;
global using Metagram.Services.ViewServices.Abstractions;
global using Metagram.ViewModels.Abstractions;

//Dapper
global using Dapper;

// Telegrator
global using Telegrator;
global using Telegrator.MadiatorCore;
global using Telegrator.Handlers.Components;
global using Telegrator.MadiatorCore.Descriptors;

//Telegram.Bot
global using Telegram.Bot;
global using Telegram.Bot.Polling;
global using Telegram.Bot.Requests;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;

//Sqlite
global using Microsoft.Data.Sqlite;

//Logging providers
global using NReco.Logging.File;
