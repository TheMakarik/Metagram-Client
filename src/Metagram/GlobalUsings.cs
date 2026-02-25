// Core
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

// View
global using Avalonia;
global using ReactiveUI;
global using ReactiveUI.Avalonia;
global using ReactiveUI.Builder;
global using ReactiveUI.SourceGenerators;

// System
global using System;
global using System.Threading.Tasks;
global using System.Collections.Immutable;
global using System.Collections.ObjectModel;
global using System.Diagnostics;

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
global using Telegram.Bot.Exceptions;

// Models
global using Metagram.Models;
global using Metagram.Models.Authorization;
global using Metagram.Models.Polling;
global using Metagram.Models.DataAccess.Tables;
global using Metagram.Models.Messages;
global using Metagram.Models.DataAccess.Dto;
global using Metagram.Models.DataAccess;
global using Metagram.Models.DataAccess.MappingExtensions;
global using Metagram.Models.Options;

//Repositories
global using Metagram.Repositories.Interfaces;
global using Metagram.Repositories;

// Services
global using Metagram.Services;
global using Metagram.Services.Authorization;
global using Metagram.Services.Polling;

//Entity framework
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Sqlite;
