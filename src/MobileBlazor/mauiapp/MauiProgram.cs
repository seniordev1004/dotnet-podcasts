﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Infrastructure;
using NetPodsMauiBlazor.Services;
using Podcast.Components;
using Podcast.Pages.Data;
using Podcast.Shared;

namespace NetPodsMauiBlazor;

public static class MauiProgram
{
    //public static string BaseWeb = $"{Base}:5002/listentogether";
    //public static string Base = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";
    //public static string APIUrl = $"{Base}:5000/v1/";
    //public static string ListenTogetherUrl = $"{Base}:5001/listentogether";

    public static string BaseWeb = $"https://dotnetconf2021-netpodcast.azurewebsites.net/listentogether";
    public static string APIUrl = $"https://podcastapica.gentlesea-ba4a72f3.canadacentral.azurecontainerapps.io";
    public static string ListenTogetherUrl = $"https://dotnetconf2021-netpodcast-hub.azurewebsites.net/listentogether";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddHttpClient<PodcastService>(client =>
        {
            client.BaseAddress = new Uri(APIUrl);
        });

#if WINDOWS
        builder.Services.AddSingleton<INativeAudioService, Platforms.Windows.NativeAudioService>();
#elif ANDROID
        builder.Services.AddSingleton<INativeAudioService, Platforms.Android.Services.NativeAudioService>();
#elif MACCATALYST
        builder.Services.AddSingleton<INativeAudioService, Platforms.MacCatalyst.Services.NativeAudioService>();
#elif IOS
        builder.Services.AddSingleton<INativeAudioService, Platforms.iOS.Services.NativeAudioService>();
#endif

        builder.Services.AddScoped<ThemeInterop>();
        builder.Services.AddScoped<IAudioInterop, AudioInteropService>();
        builder.Services.AddScoped<LocalStorageInterop>();
        builder.Services.AddScoped<ClipboardInterop>();
        builder.Services.AddScoped<SubscriptionsService>();
        builder.Services.AddScoped<ListenLaterService>();
        builder.Services.AddSingleton<PlayerService>();
        builder.Services.AddScoped<ListenTogetherHubClient>(_ =>
            new ListenTogetherHubClient(ListenTogetherUrl));
        builder.Services.AddScoped<ComponentStatePersistenceManager>();
        builder.Services.AddScoped<PersistentComponentState>(sp => sp.GetRequiredService<ComponentStatePersistenceManager>().State);

        return builder.Build();
    }
}
